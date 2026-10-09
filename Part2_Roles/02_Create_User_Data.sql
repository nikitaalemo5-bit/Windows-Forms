-- Gordeev / Part 2: role-based navigation database
IF DB_ID(N'User_Data') IS NULL CREATE DATABASE User_Data;
GO
USE User_Data;
GO
IF OBJECT_ID('dbo.Roles','U') IS NULL CREATE TABLE dbo.Roles(ID int NOT NULL PRIMARY KEY, Name nvarchar(50) NOT NULL UNIQUE);
IF OBJECT_ID('dbo.Users','U') IS NULL CREATE TABLE dbo.Users(
 ID int IDENTITY(1,1) PRIMARY KEY,
 Login nvarchar(50) NOT NULL UNIQUE,
 PasswordHash nvarchar(256) NOT NULL,
 First_Name nvarchar(50) NOT NULL,
 Second_Name nvarchar(50) NULL,
 PicturePath nvarchar(260) NULL,
 RoleID int NOT NULL REFERENCES dbo.Roles(ID),
 IsActive bit NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT(1),
 CreatedAt date NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT(CONVERT(date,GETDATE()))
);
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE ID=1) INSERT dbo.Roles(ID,Name) VALUES(1,N'Manager');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE ID=2) INSERT dbo.Roles(ID,Name) VALUES(2,N'Seller');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE ID=3) INSERT dbo.Roles(ID,Name) VALUES(3,N'Director');
IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Login=N'admin') INSERT dbo.Users(Login,PasswordHash,First_Name,Second_Name,RoleID) VALUES(N'admin',N'Admin123!',N'Никита',N'Гордеев',1);
IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Login=N'seller') INSERT dbo.Users(Login,PasswordHash,First_Name,Second_Name,RoleID) VALUES(N'seller',N'Seller123!',N'Иван',N'Примеров',2);
GO
SELECT u.ID,u.Login,u.First_Name,u.Second_Name,r.Name AS Role,u.IsActive,u.CreatedAt
FROM dbo.Users u JOIN dbo.Roles r ON r.ID=u.RoleID ORDER BY u.ID;
GO
