﻿create database Cafina
use Cafina

cREATE TABLE [Role](
	id int identity primary key,
	RoName nvarchar(50) not null
)

Create table Users(
	id int identity primary key,
	FullName nvarchar(100),
	email varchar(100),
	Phone_Number varchar(20),
	BirthDay date,
	Gender nvarchar(10) check(Gender in('nam',N'nữ')),
	Role_Id int foreign key references [Role] (id)
	on delete cascade on update cascade
)
Create table Category(
	id int identity primary key,
	CateName nvarchar(100) not null
)
Create table Product(
	ProductId varchar(100) primary key,
	title nvarchar(100) not null,
	price int not null,
	discount int,
	[description] nvarchar(100),
	ChatLieu nvarchar(50),
	HuongDanSuDung nvarchar(200),
	size varchar(10),
	color varchar(10)
)

alter table Product
add  CateId int  foreign key references Category(id)
on update cascade on delete cascade

create table Galery(
	id int identity primary key,
	ProductId varchar(100) foreign key references Product(ProductId)
	on delete cascade on update cascade,
	thumbnail varchar(200)
)
create table [Order](
	id int identity primary key,
	[user_id] int foreign key references Users(id)
	on update cascade on delete cascade,
	fullName nvarchar(50),
	email varchar(50),
	phone_number varchar(20),
	[address] nvarchar(100),
	note nvarchar(200),
	order_date date,
	[status] int
)
create table Order_Details(
	id int identity primary key,
	OrderId int foreign key references [Order](id)
	on delete cascade on update cascade,
	ProductId varchar(100) foreign key references Product(ProductId)
	on update cascade on delete cascade,
	Price int,
	Amount int
)
