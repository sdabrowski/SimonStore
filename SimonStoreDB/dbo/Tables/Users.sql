CREATE TABLE [dbo].[Users]
(
	[AspNetUserID] NVARCHAR(128) NOT NULL, 

	[FirstName] NVARCHAR(100) NOT NULL, 
	[LastName] NVARCHAR(100) NOT NULL, 
	[BillingStreetAddress1] NVARCHAR(250) NOT NULL, 
	[BillingStreetAddress2] NVARCHAR(250) NULL, 
	[BillingCity] NVARCHAR(50) NOT NULL, 
	[BillingState] NVARCHAR(50) NOT NULL, 
	[BillingZip] NVARCHAR(50) NOT NULL, 
	[ShippingStreetAddress1] NVARCHAR(250) NOT NULL, 
	[ShippingStreetAddress2] NVARCHAR(250) NULL, 
	[ShippingCity] NVARCHAR(50) NOT NULL, 
	[ShippingState] NVARCHAR(50) NOT NULL, 
	[ShippingZip] NVARCHAR(50) NOT NULL, 
	[CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
	[LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    CONSTRAINT [PK_Users] PRIMARY KEY ([AspNetUserID]), 
    CONSTRAINT [FK_Users_AspNetUsers] FOREIGN KEY (AspNetUserID) REFERENCES AspNetUsers(ID)
)
