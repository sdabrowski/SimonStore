CREATE TABLE [dbo].[CategoriesProducts]
(
	[ID] INT IDENTITY (1,1) NOT NULL, 
	[Category] INT NOT NULL, 
	[ProductSKU] NVARCHAR(50) NOT NULL, 
	CONSTRAINT [PK_CategoryProducts] PRIMARY KEY ([ID]), 
	CONSTRAINT [FK_CategoriesProducts_Categories] FOREIGN KEY ([Category]) REFERENCES [Categories]([ID]),
	CONSTRAINT [FK_CategoriesProducts_Products] FOREIGN KEY (ProductSKU) REFERENCES [Products]([SKU])
)
