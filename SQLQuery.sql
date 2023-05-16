create database Task_DataBase

use Task_DataBase

-- This is the User table
create table T_Users(
	U_ID int IDENTITY(1, 1) Primary key,
	U_Name varchar(255),
	U_User_Name varchar(255) unique,
	U_Password varchar(255),
	Roles varchar(255),
	U_Email varchar(255),
	Active_Status varchar(255)
)

-- This is the Task table
create table T_User_Tasks(
	T_ID int IDENTITY(1, 1) Primary key,
	T_Title varchar(255),
	T_Description nvarchar(max),
	T_Task_Creater varchar(255),
	T_Start_Date date,
	T_End_Date date,
	T_File varbinary(max)
)