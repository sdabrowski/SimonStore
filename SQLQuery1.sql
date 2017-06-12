--I create a view to give a reusable, queryable object that can be selected from and joint to:
CREATE VIEW vw_AddressCities

AS

SELECT DISTINCT City FROM [Address]

GO

--Reading from a view
SELECT * FROM vw_AddressCities
--Joining to a view
SELECT * FROM vw_AddressCities INNER JOIN Address ON vw_AddressCities = Address.City