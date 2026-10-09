-- Gordeev / Part 1: Module4DB
-- Run in SQL Server Management Studio (SSMS). This script is idempotent for tables/data.
IF DB_ID(N'Module4DB') IS NULL CREATE DATABASE Module4DB;
GO
USE Module4DB;
GO
IF OBJECT_ID('dbo.Statuses','U') IS NULL CREATE TABLE dbo.Statuses(ID int NOT NULL PRIMARY KEY, Name nvarchar(50) NOT NULL UNIQUE);
IF OBJECT_ID('dbo.Roles','U') IS NULL CREATE TABLE dbo.Roles(ID int NOT NULL PRIMARY KEY, Name nvarchar(50) NOT NULL UNIQUE);
IF OBJECT_ID('dbo.Accounts','U') IS NULL
BEGIN
 CREATE TABLE dbo.Accounts(
  ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  Login nvarchar(50) NOT NULL UNIQUE,
  PasswordHash nvarchar(256) NOT NULL,
  FullName nvarchar(150) NOT NULL,
  LastDateAuthorization date NULL,
  RoleID int NOT NULL CONSTRAINT FK_Accounts_Roles REFERENCES dbo.Roles(ID),
  StatusID int NOT NULL CONSTRAINT FK_Accounts_Statuses REFERENCES dbo.Statuses(ID),
  BadLoginTry int NOT NULL CONSTRAINT DF_Accounts_BadLoginTry DEFAULT(0),
  NewUser bit NOT NULL CONSTRAINT DF_Accounts_NewUser DEFAULT(1),
  CreatedAt datetime2 NOT NULL CONSTRAINT DF_Accounts_CreatedAt DEFAULT(SYSDATETIME())
 );
END;
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE ID=1) INSERT dbo.Roles(ID,Name) VALUES(1,N'Admin');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE ID=2) INSERT dbo.Roles(ID,Name) VALUES(2,N'User');
IF NOT EXISTS(SELECT 1 FROM dbo.Statuses WHERE ID=1) INSERT dbo.Statuses(ID,Name) VALUES(1,N'Active');
IF NOT EXISTS(SELECT 1 FROM dbo.Statuses WHERE ID=2) INSERT dbo.Statuses(ID,Name) VALUES(2,N'Blocked');
-- Demo credentials are intentionally changed from the source archive. Password is plain demo seed only;
-- application code should use a password hash in real deployment.
IF NOT EXISTS(SELECT 1 FROM dbo.Accounts WHERE Login=N'admin')
 INSERT dbo.Accounts(Login,PasswordHash,FullName,RoleID,StatusID,BadLoginTry,NewUser)
 VALUES(N'admin',N'Admin123!',N'Администратор демонстрационный',1,1,0,0);
IF NOT EXISTS(SELECT 1 FROM dbo.Accounts WHERE Login=N'user')
 INSERT dbo.Accounts(Login,PasswordHash,FullName,RoleID,StatusID,BadLoginTry,NewUser)
 VALUES(N'user',N'User123!',N'Пользователь демонстрационный',2,1,0,1);
GO
SELECT a.ID,a.Login,a.FullName,r.Name AS Role,s.Name AS Status,a.LastDateAuthorization,a.BadLoginTry
FROM dbo.Accounts a JOIN dbo.Roles r ON r.ID=a.RoleID JOIN dbo.Statuses s ON s.ID=a.StatusID;
GO
