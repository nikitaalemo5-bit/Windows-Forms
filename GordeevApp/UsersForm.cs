using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
namespace GordeevApp {
 public class UsersForm : Form {
  readonly int currentId; readonly string fullName, role; DataGridView grid=new DataGridView(); BindingSource source=new BindingSource(); BindingNavigator nav; TextBox login=new TextBox(), name=new TextBox(), pass=new TextBox(); ComboBox roleBox=new ComboBox();
  string cs=LoginForm.ConnectionString;
  public UsersForm(int id,string person,string userRole){currentId=id;fullName=person;role=userRole; Text="Gordeev — Пользователи"; WindowState=FormWindowState.Maximized;
   var top=new Panel{Dock=DockStyle.Top,Height=105}; top.Controls.Add(new Label{Text="Пользователь: "+fullName+" | Роль: "+role,Left=12,Top=8,AutoSize=true,Font=new Font("Segoe UI",11,FontStyle.Bold)});
   if(role.Equals("Admin",StringComparison.OrdinalIgnoreCase)){ AddField(top,"Логин",login,12,42); AddField(top,"ФИО",name,190,42); AddField(top,"Пароль",pass,430,42); AddField(top,"Роль ID",roleBox,670,42); roleBox.DropDownStyle=ComboBoxStyle.DropDownList; roleBox.Items.AddRange(new object[]{"1","2"}); roleBox.SelectedIndex=1;
    var add=new Button{Text="Добавить",Left=820,Top=40,Width=90}; add.Click+=AddUser; top.Controls.Add(add); var del=new Button{Text="Удалить выбранного",Left=915,Top=40,Width=150}; del.Click+=DeleteUser; top.Controls.Add(del);
   }
   grid.Dock=DockStyle.Fill;grid.ReadOnly=role!="Admin";grid.AllowUserToAddRows=false;grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;grid.DataSource=source;
   nav=new BindingNavigator(true); nav.Dock=DockStyle.Bottom; nav.BindingSource=source;
   Controls.Add(grid);Controls.Add(nav);Controls.Add(top); LoadUsers();
  }
  void AddField(Control parent,string label,Control box,int x,int y){parent.Controls.Add(new Label{Text=label,Left=x,Top=y+5,AutoSize=true});box.SetBounds(x+55,y,125,25);parent.Controls.Add(box);}
  void LoadUsers(){try{using(var c=new SqlConnection(cs))using(var da=new SqlDataAdapter("SELECT a.ID,a.Login,a.FullName,r.Name AS Role,s.Name AS Status,a.LastDateAuthorization,a.BadLoginTry FROM dbo.Accounts a JOIN dbo.Roles r ON r.ID=a.RoleID JOIN dbo.Statuses s ON s.ID=a.StatusID ORDER BY a.ID",c)){var t=new DataTable();da.Fill(t);source.DataSource=t;}}catch(Exception ex){MessageBox.Show(ex.Message,"Ошибка загрузки");}}
  void AddUser(object sender,EventArgs e){if(string.IsNullOrWhiteSpace(login.Text)||string.IsNullOrWhiteSpace(name.Text)||string.IsNullOrEmpty(pass.Text)){MessageBox.Show("Заполните логин, ФИО и пароль.");return;}try{using(var c=new SqlConnection(cs))using(var cmd=new SqlCommand("INSERT dbo.Accounts(Login,PasswordHash,FullName,RoleID,StatusID) VALUES(@l,@p,@n,@r,1)",c)){cmd.Parameters.AddWithValue("@l",login.Text.Trim());cmd.Parameters.AddWithValue("@p",pass.Text);cmd.Parameters.AddWithValue("@n",name.Text.Trim());cmd.Parameters.AddWithValue("@r",int.Parse(roleBox.Text));c.Open();cmd.ExecuteNonQuery();}LoadUsers();login.Clear();name.Clear();pass.Clear();}catch(Exception ex){MessageBox.Show(ex.Message,"Не удалось добавить пользователя");}}
  void DeleteUser(object sender,EventArgs e){if(grid.CurrentRow==null)return;int id=Convert.ToInt32(grid.CurrentRow.Cells["ID"].Value);if(id==currentId){MessageBox.Show("Нельзя удалить текущую учетную запись.");return;}if(MessageBox.Show("Удалить выбранную учетную запись?","Подтверждение",MessageBoxButtons.YesNo)!=DialogResult.Yes)return;try{using(var c=new SqlConnection(cs))using(var cmd=new SqlCommand("DELETE dbo.Accounts WHERE ID=@id",c)){cmd.Parameters.AddWithValue("@id",id);c.Open();cmd.ExecuteNonQuery();}LoadUsers();}catch(Exception ex){MessageBox.Show(ex.Message,"Не удалось удалить");}}
 }
}
