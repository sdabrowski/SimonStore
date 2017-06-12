CREATE TABLE [dbo].[Categories]
(
	[ID] INT IDENTITY (1,1) NOT NULL, 
	[ParentID] INT NULL, 
	[Name] NVARCHAR(50) NOT NULL,
	[CreatedOn] DATETIME NULL DEFAULT GetUtcDate(), 
    [LastModifiedOn] DATETIME NULL DEFAULT GetUtcDate(), 
	CONSTRAINT [PK_Categories] PRIMARY KEY ([ID]),
	CONSTRAINT [FK_Categories_Categories] FOREIGN KEY (ParentID) References Categories (ID)
)
