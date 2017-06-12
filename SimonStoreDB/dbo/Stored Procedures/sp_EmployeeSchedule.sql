CREATE PROCEDURE sp_EmployeeSchedule
@date DATETIME,
@storeID INT
AS
SELECT 
	Employee.Name, 
	[Shift].StartTime, 
	[Shift].EndTime 
FROM
	Store
INNER JOIN
	[Shift] ON Store.ID = [Shift].StoreID
INNER JOIN 
	Employee ON [Shift].EmployeeID = Employee.ID
WHERE 
	Store.ID = @storeID AND 
	([Shift].StartTime BETWEEN (@date) AND DATEADD(DAY, 1, @date) OR
	[Shift].EndTime BETWEEN (@date) AND DATEADD(Day, 1, @date))

