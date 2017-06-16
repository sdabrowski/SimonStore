CREATE TABLE [dbo].[ProductImages]
(
	[ID] INT IDENTITY (1,1) NOT NULL, 
	[SKU] NVARCHAR(50) NOT NULL, 
	[ImageLocation] NVARCHAR(100) NULL,
	[AlternateText] NVARCHAR(100) NULL, 
	[CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    [LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    CONSTRAINT [PK_ProductImages] PRIMARY KEY ([ID]),
	CONSTRAINT [FK_ProductImage_Products] FOREIGN KEY (SKU) REFERENCES Products (SKU)
)
