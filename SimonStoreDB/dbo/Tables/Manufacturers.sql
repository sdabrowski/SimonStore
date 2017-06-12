CREATE TABLE [dbo].[Manufacturers] 
(
    [ManufacturerID] INT IDENTITY (1,1) NOT NULL,
	[StreetAddress1] NVARCHAR(250) NULL, 
    [StreetAddress2] NVARCHAR(250) NULL, 
    [City] NVARCHAR(50) NULL, 
    [State] NVARCHAR(50) NULL, 
    [Zip] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NULL, 
    [PhoneNumber] NVARCHAR(50) NULL, 
    [ContactName] NVARCHAR(50) NULL, 
    [Logo] NVARCHAR(50) NULL, 
    [LinkedProduct] NVARCHAR(50) NULL,
	[CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    [LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    CONSTRAINT [PK_Manufacturer] PRIMARY KEY ([ManufacturerID] ASC),
);

