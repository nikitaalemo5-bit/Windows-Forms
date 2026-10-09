using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
namespace GordeevApp {
 public class LoginForm : Form {
  TextBox login = new TextBox(), password = new TextBox(); Label message = new Label(); int attempts=0;
  // Change server name if your SQL Server instance is not localhost\SQLEXPRESS.
  public const string ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Module4DB;Integrated Security=True;TrustServerCertificate=True";
  public LoginForm(){ Text="Gordeev — Авторизация"; StartPosition=FormStartPosition.CenterScreen; Size=new Size(390,260); FormBorderStyle=FormBorderStyle.FixedDialog; MaximizeBox=false;
   var title=new Label{Text="Авторизация пользователя",Font=new Font("Segoe UI",14,FontStyle.Bold),AutoSize=true,Left=35,Top=20};
   var l1=new Label{Text="Логин",Left=35,Top=75,AutoSize=true}; login.SetBounds(125,70,210,27);
   var l2=new Label{Text="Пароль",Left=35,Top=115,AutoSize=true}; password.SetBounds(125,110,210,27); password.PasswordChar='●';
   var btn=new Button{Text="Войти",Left=125,Top=150,Width=100,Height=32}; btn.Click+=Authenticate;
   message.SetBounds(35,190,320,35); message.ForeColor=Color.Firebrick;
   Controls.AddRange(new Control[]{title,l1,login,l2,password,btn,message}); AcceptButton=btn;
  }
  void Authenticate(object sender, EventArgs e){
   if(attempts>=3){ message.Text="Превышено число попыток. Перезапустите приложение."; return; }
   try { using(var c=new SqlConnection(ConnectionString)) using(var cmd=new SqlCommand(@"SELECT a.ID,a.FullName,r.Name FROM dbo.Accounts a JOIN dbo.Roles r ON r.ID=a.RoleID JOIN dbo.Statuses s ON s.ID=a.StatusID WHERE a.Login=@login AND a.PasswordHash=@pass AND s.Name=N'Active'",c)) {
    cmd.Parameters.AddWithValue("@login",login.Text.Trim()); cmd.Parameters.AddWithValue("@pass",password.Text); c.Open(); using(var rd=cmd.ExecuteReader()){
     if(rd.Read()){ int id=rd.GetInt32(0); string name=rd.GetString(1), role=rd.GetString(2); rd.Close(); using(var update=new SqlCommand("UPDATE dbo.Accounts SET LastDateAuthorization=CONVERT(date,GETDATE()),BadLoginTry=0 WHERE ID=@id",c)){update.Parameters.AddWithValue("@id",id);update.ExecuteNonQuery();} Hide(); using(var f=new UsersForm(id,name,role)) f.ShowDialog(); Close(); }
     else { attempts++; message.Text="Неверный логин/пароль или учетная запись заблокирована. Попыток: "+attempts+"/3"; }
    }
   }} catch(Exception ex){ MessageBox.Show("Не удалось подключиться к базе. Проверьте SQL Server и строку подключения.\n\n"+ex.Message,"Ошибка подключения",MessageBoxButtons.OK,MessageBoxIcon.Error); }
  }
 }
}
