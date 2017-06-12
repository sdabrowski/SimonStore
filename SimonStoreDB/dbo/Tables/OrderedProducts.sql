CREATE TABLE [dbo].[OrderedProducts]
(
    [OrderID] INT NOT NULL, 
	[SKU] NVARCHAR(50) NOT NULL , 
    [ProductPrice] MONEY NULL, 
    [Quantity] INT NULL, 
    [Weight] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_OrderedProducts] PRIMARY KEY ([OrderID], [SKU]), 
    CONSTRAINT [FK_OrderedProducts_Orders] FOREIGN KEY (OrderID) REFERENCES Orders(OrderID), 
    CONSTRAINT [FK_OrderedProducts_Products] FOREIGN KEY (SKU) REFERENCES Products(SKU), 
)
