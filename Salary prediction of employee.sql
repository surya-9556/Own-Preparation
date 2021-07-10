create database SalaryPrediction
go

use SalaryPrediction

go
create schema Employee

go
create schema Logins

go
create table Employee.Employee(
EmpId int identity(100,1) primary key,
EmployeeName varchar(200) not null,
BirthDate date,
HireDate date,
Gender nchar(10),
MaritalStatus nchar(10),
Email nvarchar(550),
PhoneNumber nvarchar(max))

go
create table Employee.Address(
AddressId int identity(1,1) primary key,
EmployeeId int,
Address nvarchar(200),
City nvarchar(100),
State nvarchar(100),
PinCode nvarchar(max),
Constraint FK_Employee foreign key (EmployeeId) references Employee.Employee(EmpId))

go
create schema Driver

go
create table Driver.Driver(
DriverId int identity(100,1) primary key,
DriverName nvarchar(200),
PhoneNumber nvarchar(max))

go
create table Employee.Vechile(
VechileId int identity(1000,1) primary key,
EmployeeId int,
AddressId int,
DriverId int,
VechileName nvarchar(100),
VechileRoute nvarchar(100),
VechileNumber nvarchar(100),
Capacity int,
Constraint FK_Emp foreign key (EmployeeId) references Employee.Employee(EmpId),
Constraint FK_Address foreign key (AddressId) references Employee.Address(AddressId),
Constraint FK_Driver foreign key (DriverId) references Driver.Driver(DriverId))

go
create table Employee.Salary(
SalaryId int identity(1,1) primary key,
EmployeeId int,
BasicSalary decimal(18,2),
HRA decimal(18,2),
DA decimal(18,2),
Travel decimal(18,2),
Constraint FK_Employees foreign key (EmployeeId) references Employee.Employee(EmpId))

go
create table Logins.Regestration(
Id int identity(1,1) primary key,
UsersId int not null,
UserName nvarchar(200) unique not null,
Email nvarchar(550) unique not null,
PassWord nvarchar(550) not null,
ConformPassword nvarchar(550) not null,
Constraint FK_Users foreign key (UsersId) references Logins.Users(UserId))

go
create table Logins.Roles(
RoleId int identity(1,1) primary key,
UserId int,
Role nvarchar(100) not null,
Constraint FK_RolesOfUs foreign key (UserId) references Logins.Users(UserId))

go
create table Logins.Users(
UserId int identity(1,1) primary key,
UserName nvarchar(100),
Password nvarchar(100))

go
create proc Employee.Crud(
@EmpId int = 0,
@EmployeeName nvarchar(550),
@BirthDate date,
@HireDate date,
@Gender nchar(10),
@MaritalStatus nchar(10),
@Email nvarchar(550),
@PhoneNumber nvarchar(max),
@Choice nvarchar(100))
as 
begin
	if @Choice = 'insert' or @Choice = 'Insert' or @Choice = 'INSERT'
	begin
		insert into Employee.Employee(EmployeeName,BirthDate,HireDate,Gender,MaritalStatus,Email,PhoneNumber) 
		values (@EmployeeName,@BirthDate,@HireDate,@Gender,@MaritalStatus,@Email,@PhoneNumber)
	end

	if @Choice = 'update' or @Choice = 'Update' or @Choice = 'UPDATE'
	begin
		Update Employee.Employee set
		EmployeeName=@EmployeeName,
		BirthDate=@BirthDate,
		HireDate=@HireDate,
		Gender=@Gender,
		MaritalStatus=@MaritalStatus,
		Email=@Email,
		PhoneNumber=@PhoneNumber where EmpId = @EmpId
	end

	if @Choice = 'delete' or @Choice = 'Delete' or @Choice = 'DELETE'
	begin
		delete from Employee.Employee where EmpId = @EmpId
	end
end

select * from Employee.Employee
select * from Employee.Address
select * from Employee.Vechile
select * from Employee.Salary
select * from Driver.Driver

go
create proc Employee.Salary_predict( @EmpId int, @Tax int out)
as begin
declare
	@total float,
	@taxpayable float
	set @total= (select sum(BasicSalary*12+HRA+DA+Travel) as total_salary 
				from Employee.Salary S join Employee.Employee E 
				on S.EmployeeId = E.EmpId
				where EmpId = @EmpId)
	if(@total < 250000)
		set @tax = 0
	else if(@total > 250000 and @total < 350000)
		set @tax = 5
	else
		set @tax = 10
	set @taxpayable = @total*@tax/1000
	print @taxpayable
end

declare
@mytax float
exec Employee.Salary_predict 100,@mytax out
print @mytax

go
create view Employee.EmployeeSal
as
select E.EmployeeName, sum(BasicSalary*12+HRA+DA+Travel) as total_salary 
from Employee.Salary S join Employee.Employee E 
on S.EmployeeId = E.EmpId where exists
(select EmployeeName from Employee.Employee)
group by EmployeeName

go
create trigger SalTrigger
on Employee.Salary
after insert
as
begin
	if((select sum(BasicSalary*12+HRA+DA+Travel)*5/1000 from Employee.Salary) < 300000)
		select concat('Amout of tax payable is 5% i.e.,',sum(BasicSalary*12+HRA+DA+Travel)*5/1000) from inserted
	else
		select concat('Amout of tax payable is 0% i.e.,',sum(BasicSalary*12+HRA+DA+Travel)*0/1000) from inserted
end

select DATEDIFF(year,BirthDate,getdate()) as Age from Employee.Employee
select DATEDIFF(DAY,HireDate,getdate()) as Age from Employee.Employee

go
create view Employee.EmpVech
as
select EmpId,EmployeeName,VechileName,VechileRoute,DriverName from 
Employee.Employee E join Employee.Vechile V 
on E.EmpId = V.EmployeeId join Driver.Driver D 
on D.DriverId = V.DriverId

go
select * from Employee.EmpVech

select * from Logins.Users

select * from Logins.Roles

select UserId,Role from Logins.Roles where UserId = 7