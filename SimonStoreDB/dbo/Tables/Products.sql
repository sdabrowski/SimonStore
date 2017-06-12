CREATE TABLE [dbo].[Products] (
    [SKU]               NVARCHAR(50)             NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [UPC]              NVARCHAR(50)            NULL,
    [Price]            MONEY          NULL,
	[ManufacturerID] INT NULL,
    [Description]  NTEXT NULL,
    [Weight] NVARCHAR(50) NULL,
    [CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    [LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([SKU] ASC),
    CONSTRAINT [FK_Product_Manufacturer] FOREIGN KEY ([ManufacturerID]) REFERENCES [dbo].[Manufacturers] ([ManufacturerID]),
);

