CREATE TABLE [dbo].[Orders]
(
	[OrderID] INT IDENTITY(1,1) NOT NULL, 
	[CustomerEmail] NVARCHAR(256) NULL,
	[BillingStreetAddress1] NVARCHAR(250) NULL, 
	[BillingStreetAddress2] NVARCHAR(250) NULL, 
	[BillingCity] NVARCHAR(50) NULL, 
	[BillingState] NVARCHAR(50) NULL, 
	[BillingZip] NVARCHAR(50) NULL, 
	[ShippingStreetAddress1] NVARCHAR(250) NULL, 
	[ShippingStreetAddress2] NVARCHAR(250) NULL, 
	[ShippingCity] NVARCHAR(50) NULL, 
	[ShippingState] NVARCHAR(50) NULL, 
	[ShippingZip] NVARCHAR(50) NULL, 
	[AspNetUserID] NVARCHAR(128) NULL, 
	[ShippingService] INT NULL,
	[OrderCompletedDate] DATETIME NULL DEFAULT null, 
    [CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    [LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(),  
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderID]),
	CONSTRAINT [FK_Orders_Users] FOREIGN KEY ([AspNetUserID]) REFERENCES AspNetUsers (ID),
	CONSTRAINT [FK_Orders_ShippingMethod] FOREIGN KEY (ShippingService) REFERENCES ShippingMethod ([ID])
)
