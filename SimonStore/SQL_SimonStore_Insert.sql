DELETE FROM OrderedProducts

DELETE FROM ProductImages

DELETE FROM Products

INSERT INTO Products
(SKU, Name, Price, Description, Weight)
VALUES
('ELW-0001', 'Black - 1000'' Spool', 48, 'This is a 1000'' Spool of Black 550 Paracord', 48),
('ELW-0002', 'Charcoal Gray - 1000'' Spool', 48, 'This is a 1000'' Spool of Charcoal Gray 550 Paracord', 48),
('ELW-0003', 'White - 1000'' Spool', 48, 'This is a 1000'' Spool of White 550 Paracord', 48),
('ELW-0004', 'Dark Green - 1000'' Spool', 48, 'This is a 1000'' Spool of Dark Green 550 Paracord', 48)

INSERT INTO ProductImages
(SKU, [ImageLocation], AlternateText)
VALUES
('ELW-0001', '/Images/ELW-0001_spool.png', 'An image of a Black Spool of Paracord'),
('ELW-0002', '/Images/ELW-0002_spool.png', 'An image of a Charcoal Gray Spool of Paracord'),
('ELW-0003', '/Images/ELW-0003_spool.png', 'An image of a White Spool of Paracord'),
('ELW-0004', '/Images/ELW-0004_spool.png', 'An image of a Dark Green Spool of Paracord')