/*
Purchase Bill Application - SQL Server Database Setup
Database: PurchaseBillDb

IMPORTANT:
This script creates the schema manually.
The application also uses EF Core migrations and may automatically run
Database.MigrateAsync() at startup.

Recommended setup for a cloned project:
1. Create only the database (or let SQL Server create it).
2. Run the application / `dotnet ef database update`.
3. Let EF Core create the tables and migration history.

Use the FULL schema section below only when you intentionally want a
manual SQL-based database setup instead of EF Core migrations.
*/

USE master;
GO

IF DB_ID(N'PurchaseBillDb') IS NULL
BEGIN
    CREATE DATABASE PurchaseBillDb;
END
GO

USE PurchaseBillDb;
GO

IF OBJECT_ID(N'dbo.Location_Details', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Location_Details
    (
        Id            INT IDENTITY(1,1) NOT NULL,
        LocationCode  NVARCHAR(100) NOT NULL,
        LocationName  NVARCHAR(200) NOT NULL,
        StockHandle   INT NOT NULL,
        Address       NVARCHAR(500) NOT NULL,
        Phone         NVARCHAR(50) NOT NULL,
        Status        INT NOT NULL,
        CompanyCode   NVARCHAR(100) NOT NULL,

        CONSTRAINT PK_Location_Details PRIMARY KEY (Id)
    );

    CREATE UNIQUE INDEX IX_Location_Details_CompanyCode_LocationCode
        ON dbo.Location_Details (CompanyCode, LocationCode);
END
GO

IF OBJECT_ID(N'dbo.Purchase_Bills', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Purchase_Bills
    (
        Id          INT IDENTITY(1,1) NOT NULL,
        CreatedAt   DATETIME2 NOT NULL,

        CONSTRAINT PK_Purchase_Bills PRIMARY KEY (Id)
    );
END
GO

IF OBJECT_ID(N'dbo.Purchase_Bill_Items', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Purchase_Bill_Items
    (
        Id                    INT IDENTITY(1,1) NOT NULL,
        PurchaseBillId        INT NOT NULL,
        ItemName              NVARCHAR(100) NOT NULL,
        LocationCode          NVARCHAR(100) NOT NULL,
        BatchName             NVARCHAR(200) NOT NULL,
        StandardCost          DECIMAL(18,2) NOT NULL,
        StandardPrice         DECIMAL(18,2) NOT NULL,
        Quantity              INT NOT NULL,
        DiscountPercentage    DECIMAL(5,2) NOT NULL,
        TotalCost             DECIMAL(18,2) NOT NULL,
        TotalSelling          DECIMAL(18,2) NOT NULL,

        CONSTRAINT PK_Purchase_Bill_Items PRIMARY KEY (Id),

        CONSTRAINT FK_Purchase_Bill_Items_Purchase_Bills_PurchaseBillId
            FOREIGN KEY (PurchaseBillId)
            REFERENCES dbo.Purchase_Bills (Id)
            ON DELETE CASCADE
    );

    CREATE INDEX IX_Purchase_Bill_Items_PurchaseBillId
        ON dbo.Purchase_Bill_Items (PurchaseBillId);
END
GO

PRINT 'PurchaseBillDb schema setup completed.';
GO

SELECT
    TABLE_SCHEMA,
    TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;
GO
