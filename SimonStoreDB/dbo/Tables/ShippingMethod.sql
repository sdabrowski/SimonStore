CREATE TABLE [dbo].[ShippingMethod]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[ServiceName] NVARCHAR(50) NOT NULL, 
    [Carrier] NVARCHAR(50) NOT NULL, 
	[CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    [LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    CONSTRAINT [PK_ShippingMethod] PRIMARY KEY ([ID])
)
