declare @description VARCHAR(255) = null;
declare @maxPrice decimal(19,2) = null;
declare @minPrice decimal(19,2) = null;

SELECT a.Id,
	   a.CategoryId,
	   a.Description,
	   a.BarCode,
	   a.Price,
	   a.Image
FROM Article a
INNER JOIN  Category c
ON c.Id = a.CategoryId
WHERE a.Status = 'ACTIVADO'
AND @description IS NULL OR a.Description LIKE '%'+@description +'%'
AND @maxPrice IS NULL OR a.Price <= @maxPrice
AND @minPrice IS NULL OR a.Price >= @minPrice