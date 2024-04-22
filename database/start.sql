CREATE DATABASE FileStorage;
USE FileStorage;

CREATE TABLE Users(
ID int primary key not null IDENTITY(1,1),
Login varchar(55),
Password varchar(55)
);