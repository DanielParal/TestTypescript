
-- ======================================
-- 2021/09/07 14:28
-- Create database
-- ======================================

CREATE DATABASE IS_Db;

-- ======================================
-- 2021/09/07 14:42
-- Create Users table
-- ======================================

USE IS_Db;

CREATE TABLE Users (
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Guid nvarchar(255) NOT NULL,
    Email nvarchar(255) NOT NULL,
    Password nvarchar(MAX) NOT NULL,
    RefreshToken nvarchar(MAX) NULL,
    RefreshTokenExpiryTime datetimeoffset(7) NULL,
	DateCreated datetimeoffset(7) NOT NULL,
);

-- ======================================
-- 2021/09/07 15:08
-- Seed Users table
-- Test password for both is: Juice123
-- ======================================

USE IS_Db

INSERT INTO [dbo].[Users] ([Guid],[Email],[Password],[RefreshToken],[RefreshTokenExpiryTime],[DateCreated])
     VALUES ('957f7412-bc11-46a3-a430-d63a89bc4074','dparal@seznam.cz','C/J74/ScRBnPZnnO1JHkwc1JZtBbcbk9sLc4v38lI2A=:iEmf4t4EKzBo+G8ceIYnlA==',null,null,GETDATE());

INSERT INTO [dbo].[Users] ([Guid],[Email],[Password],[RefreshToken],[RefreshTokenExpiryTime],[DateCreated])
     VALUES ('d73ba1b8-60cb-48c0-8619-864509af92dd','dparal2@seznam.cz','C/J74/ScRBnPZnnO1JHkwc1JZtBbcbk9sLc4v38lI2A=:iEmf4t4EKzBo+G8ceIYnlA==',null,null,GETDATE());
