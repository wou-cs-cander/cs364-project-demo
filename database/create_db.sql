USE master
GO

-- drop the PartStore database if it already exists
IF DB_ID('PartStore') IS NOT NULL
	DROP DATABASE PartStore
GO

CREATE DATABASE PartStore
GO

USE PartStore
GO



CREATE TABLE Items (
   ItemID  int IDENTITY(1,1) NOT NULL PRIMARY KEY
  ,Description VARCHAR(15) NOT NULL 
  ,MgfName     VARCHAR(6) NOT NULL
  ,Year        INTEGER  NOT NULL
);

INSERT INTO Items(Description,MgfName,Year) VALUES ('Hubcap','Toyota',2004);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Trunk Latch','Toyota',2004);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Air Filter','Toyota',2004);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Trunk Floor Mat','Toyota',2004);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Distributor Cap','Honda',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Spark Plug','Honda',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Starter Motor','Honda',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Air Filter','Dodge',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Spark Plug','Dodge',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Hubcap','Dodge',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Starter Motor','Dodge',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Cabin Air Fiter','Dodge',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Oil Filter','Dodge',1995);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Hubcap','Subaru',2005);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Starter Motor','Subaru',2005);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Cabin Air Fiter','Subaru',2005);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Oil Filter','Subaru',2005);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Spark Plug','Subaru',2005);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Distributor Cap','Subaru',2005);
INSERT INTO Items(Description,MgfName,Year) VALUES ('Engine','Subaru',2005);




CREATE TABLE Customers (
   ItemID  int IDENTITY(1,1) NOT NULL PRIMARY KEY
   ,Name  VARCHAR(40) NOT NULL
  ,Email VARCHAR(50) NOT NULL
);

INSERT INTO Customers(Name,Email) VALUES ('Larry Smith','larry@stupidtuesday.com');
INSERT INTO Customers(Name,Email) VALUES ('Lisa Simpson','lisa@harvard.edu');
INSERT INTO Customers(Name,Email) VALUES ('Joe Average','huh@average.com');


CREATE TABLE Stores(
   StoreID  int IDENTITY(1,1) NOT NULL PRIMARY KEY
   ,StoreName VARCHAR(30) NOT NULL
  ,Address   VARCHAR(40) NOT NULL
);

INSERT INTO Stores(StoreName,Address) VALUES ('Monmouth','123 Main Street');
INSERT INTO Stores(StoreName,Address) VALUES ('Salem','666 Satan Ave');
INSERT INTO Stores(StoreName,Address) VALUES ('Portland','1313 Anarchy Way');
INSERT INTO Stores(StoreName,Address) VALUES ('Independence','456 Monmouth Ave');


CREATE TABLE Inventories(
   InventoryId  int IDENTITY(1,1) NOT NULL PRIMARY KEY
   ,ItemId   INTEGER  NOT NULL
  ,StoreId  INTEGER  NOT NULL
  ,Quantity INTEGER  NOT NULL
  ,Price   MONEY NOT NULL
);

INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (1,1,3,12.34);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (2,1,1,34.56);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (3,2,5,12.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (1,2,21,12);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (5,2,3,5.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (6,2,8,6.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (7,3,13,62.49);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (8,3,4,11);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (9,1,9,9.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (10,2,4,60.39);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (10,1,1,59.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (20,3,1,2345.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (19,3,7,56.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (18,2,3,13.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (17,1,5,19.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (16,2,5,35.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (15,1,2,79.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (15,2,1,69.99);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (14,3,1,66.22);
INSERT INTO Inventories(ItemId,StoreId,Quantity,Price) VALUES (11,2,1,123.99);

