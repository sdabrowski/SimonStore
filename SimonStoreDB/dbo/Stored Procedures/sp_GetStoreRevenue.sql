
CREATE PROCEDURE sp_GetStoreRevenue
@store INT
AS
SELECT
	SUM(Product.Price) AS TotalRevenue,
	DATEPART(Year, Purchase.Date) AS Year,
	DATEPART(Month, Purchase.Date) AS Month,
	DATEPART(Day, Purchase.Date) AS Day
FROM
	Store
INNER JOIN
	Purchase
ON 
	Store.ID = Purchase.ID
INNER JOIN
	Product
ON 
	Purchase.ProductID = Product.ID
WHERE
	StoreID = @store
GROUP BY
	DATEPART(Year, Purchase.Date),
	DATEPART(Month, Purchase.Date),
	DATEPART(Day, Purchase.Date)

