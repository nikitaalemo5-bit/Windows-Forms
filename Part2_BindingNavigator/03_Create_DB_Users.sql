-- Gordeev / Part 2: BindingNavigator practice database
IF DB_ID(N'DB_Users') IS NULL CREATE DATABASE DB_Users;
GO
USE DB_Users;
GO
IF OBJECT_ID('dbo.Roles','U') IS NULL CREATE TABLE dbo.Roles(Name nvarchar(30) NOT NULL PRIMARY KEY);
IF OBJECT_ID('dbo.Users','U') IS NULL CREATE TABLE dbo.Users(
 ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 Login nvarchar(50) NOT NULL UNIQUE,
 PasswordHash nvarchar(256) NOT NULL,
 First_Name nvarchar(50) NOT NULL,
 Role_Name nvarchar(30) NOT NULL REFERENCES dbo.Roles(Name),
 CreatedAt date NOT NULL CONSTRAINT DF_BN_CreatedAt DEFAULT(CONVERT(date,GETDATE()))
);
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name=N'Admin') INSERT dbo.Roles VALUES(N'Admin');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name=N'Director') INSERT dbo.Roles VALUES(N'Director');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name=N'Seller') INSERT dbo.Roles VALUES(N'Seller');
IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Login=N'admin') INSERT dbo.Users(Login,PasswordHash,First_Name,Role_Name) VALUES(N'admin',N'Admin123!',N'Никита',N'Admin');
IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Login=N'seller') INSERT dbo.Users(Login,PasswordHash,First_Name,Role_Name) VALUES(N'seller',N'Seller123!',N'Иван',N'Seller');
GO
SELECT ID,Login,First_Name,Role_Name,CreatedAt FROM dbo.Users ORDER BY ID;
GO
