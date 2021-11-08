USE MARKET_EXPRESS
GO

---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS ARTICLE
---------------------------------------------------------------------------------------------------------------
--Obtiene info de articulos para mostrar en la vista de detalles de articulos
CREATE PROCEDURE Sp_Article_GetAllForCartDetails
(
	@userId UNIQUEIDENTIFIER = null
)
AS
BEGIN
	SELECT a.Id,
		   a.Description,
		   a.BarCode,
		   a.Price,
		   a.Image,
		   ct.Name AS CategoryName,
		   cd.Quantity
	FROM Cart_Detail cd
	INNER JOIN Article a
	ON a.Id = cd.ArticleId
	INNER JOIN Category ct
	ON a.CategoryId = ct.Id
	WHERE cd.CartId = (SELECT TOP 1 c.Id
					   FROM Cart c
					   INNER JOIN Client cl
					   ON c.ClientId = cl.Id
					   WHERE c.Status = 'ABIERTO'
					   AND cl.AppUserId = @userId);
END;

-- Obtiene los articulos ordenados por popularidad con la opcion de obtener "x" cantidad únicamente
CREATE PROCEDURE Sp_Article_GetMostPopular
(
	@take INT = null -- Si este valor viene null entonces retorna todos los productos activos con categoría
)
AS
BEGIN
	SELECT a.Id,
		   a.CategoryId,
		   a.Description,
		   a.BarCode,
		   a.Price,
		   a.Image,
		   (SELECT COUNT(1)
		    FROM Order_Detail od
		    WHERE od.ArticleId = a.Id) repeatedCount
	FROM Article a, Category c
	WHERE a.CategoryId = c.Id
	AND c.Status = 'ACTIVADO'
	AND a.Status = 'ACTIVADO'
	ORDER BY repeatedCount DESC
	OFFSET 0 ROWS 
	FETCH NEXT COALESCE(@take,(SELECT COUNT(1)
							   FROM Article a
							   WHERE a.Status = 'ACTIVADO'
							   AND a.CategoryId IS NOT NULL)) ROWS ONLY; 
END;
GO

-- Obtiene los articulos para la vista de busqueda de articulos para agregar al carrito
CREATE PROCEDURE Sp_Article_GetAllForSearch
(
	@description VARCHAR(255) = null,
	@maxPrice decimal(19,2) = null,
	@minPrice decimal(19,2) = null,
	@category varchar(MAX) = null,
	@pageNumber int = null,
	@pageSize int= null,
	@totalPages int = null OUTPUT,
	@totalCount int = null OUTPUT,
	@userId UNIQUEIDENTIFIER = null
)
AS
BEGIN
	DECLARE @skip int = @pageNumber*@pageSize;
	DECLARE @cartId UNIQUEIDENTIFIER;

	IF @userId IS NOT NULL
	BEGIN
	SET @cartId = (SELECT TOP 1 c.Id 
				   FROM Cart c 
				   WHERE c.ClientId = (SELECT TOP 1 cl.Id 
									   FROM Client cl 
									   WHERE cl.AppUserId = @userId)
				   AND c.Status = 'ABIERTO'
				   ORDER BY c.OpeningDate DESC);
	END;
	
	SET @totalCount = (SELECT COUNT(1) 
					   FROM Article a 
					   INNER JOIN  Category c
					   ON c.Id = a.CategoryId WHERE a.Status = 'ACTIVADO'
					   AND c.Status = 'ACTIVADO' 
					   AND (@description IS NULL OR a.Description LIKE '%'+@description +'%') 
					   AND (@maxPrice IS NULL OR a.Price <= @maxPrice) 
					   AND (@minPrice IS NULL OR a.Price >= @minPrice) 
					   AND (CONVERT(VARCHAR(100),a.CategoryId) IN ((SELECT VALUE FROM STRING_SPLIT(@category,',')))));
	
	SET @totalPages = CEILING(@totalCount / CONVERT(decimal,@pageSize));
	
	SELECT a.Id,
			a.CategoryId,
			a.Description,
			a.BarCode,
			a.Price,
			a.Image,
			(SELECT COALESCE(SUM(cd.Quantity),0)
			 FROM Cart_Detail cd 
			 WHERE cd.CartId = @cartId
			 AND cd.ArticleId = a.Id) CountInCart
	FROM Article a
	INNER JOIN  Category c
	ON c.Id = a.CategoryId
	WHERE a.Status = 'ACTIVADO'
	AND c.Status = 'ACTIVADO'
	AND (@description IS NULL OR a.Description LIKE '%'+@description +'%')
	AND (@maxPrice IS NULL OR a.Price <= @maxPrice)
	AND (@minPrice IS NULL OR a.Price >= @minPrice)
	AND (CONVERT(VARCHAR(100),a.CategoryId) IN ((SELECT VALUE FROM STRING_SPLIT(@category,','))))
	ORDER BY a.Description
	OFFSET @skip ROWS 
	FETCH NEXT @pageSize ROWS ONLY; 
END;
GO

---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS APPUSER
---------------------------------------------------------------------------------------------------------------
-- Obtiene los permisos del usuario segun los roles asignados
CREATE PROCEDURE Sp_AppUser_GetPermissions
(
	@Id UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT DISTINCT 
		   p.Id,
		   p.PermissionCode,
		   p.Name,
		   p.Description,
		   p.Type
	FROM AppUser u, AppUser_Role ur, Role r, Role_Permission rp, Permission p
	WHERE ur.AppUserId = @Id
	AND ur.RoleId = r.Id
	AND ur.RoleId = rp.RoleId
	AND p.Id = rp.PermissionId
END;
GO


---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS CART
---------------------------------------------------------------------------------------------------------------
--Obtiene la cantidad de articulos en el carrito
CREATE PROCEDURE Sp_Cart_GetArticlesCount
(
	@UserId UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @vCount		INT = 0;
	DECLARE @vCartId	UNIQUEIDENTIFIER;

	SET @vCartId = (SELECT TOP 1 ca.Id 
					FROM Cart ca
					INNER JOIN Client cl
					ON ca.ClientId = cl.Id
					WHERE cl.AppUserId = @UserId);

	IF @vCartId IS NOT NULL 
	BEGIN
		----
		IF (SELECT Status FROM Cart WHERE Id = @vCartId) = 'ABIERTO'
		BEGIN
			SET @vCount = (SELECT COUNT(1) FROM Cart_Detail WHERE CartId = @vCartId);
		END
		----
		/*
		IF (SELECT Status FROM Cart WHERE Id = @vCartId) = 'ABIERTO'
		BEGIN
			IF (SELECT COUNT(1) FROM Cart_Detail WHERE CartId = @vCartId) > 0
			BEGIN
				SET @vCount = (SELECT SUM(Quantity) FROM Cart_Detail WHERE CartId = @vCartId);
			END
		END
		*/
	END 

	SELECT @vCount;
END;
GO

--Obtiene la cantidad de carritos que tienen "X" articulo en el
CREATE PROCEDURE Sp_Cart_GetOpenCountByArticleId
(
	@articleId UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT COUNT(1)
	FROM Cart c, Cart_Detail cd
	WHERE c.Id = cd.CartId
	AND c.Status = 'ABIERTO'
	AND cd.ArticleId = @articleId;
END;
GO


---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS ADDRESS
---------------------------------------------------------------------------------------------------------------
--Obtiene las direcciones del cliente por Id de usuario
CREATE PROCEDURE Sp_Address_GetAllByClient
(
	@UserId UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT ad.Id,
		   ad.Name,
		   ad.Detail
	FROM AppUser a, Client c, Address ad
	WHERE a.Id = @UserId
	AND c.AppUserId = a.Id
	AND ad.ClientId = c.Id
END;
GO


---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS APPUSERROLE
---------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Sp_AppUserRole_GetUserCountUsingARole
(
	@roleId UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT (SELECT COUNT(1)
			FROM AppUser_Role ar, AppUser au
			WHERE ar.RoleId = @roleId
			AND ar.AppUserId = au.Id
			AND au.Status = 'ACTIVADO') ActiveUsers,
		   (SELECT COUNT(1)
			FROM AppUser_Role ar, AppUser au
			WHERE ar.RoleId = @roleId
			AND ar.AppUserId = au.Id
			AND au.Status = 'DESACTIVADO') DisabledUsers;
END;
GO


---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS PERMISSION
---------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Sp_Permission_GetAllByRoleId
(
	@Id UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT p.Id,
		   p.Name,
		   p.Description,
		   p.PermissionCode,
		   p.Type
	FROM Role_Permission rp,
		 Permission p
	WHERE rp.RoleId = @Id
	AND p.Id = rp.PermissionId
	ORDER BY p.Type ASC;
END;
GO

--Obtiene todos los tipos de permisos
CREATE PROCEDURE Sp_Permission_GetAllTypes
AS
BEGIN
	SELECT DISTINCT p.Type
	FROM Permission p
	ORDER BY p.Type ASC
END;
GO

---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS CATEGORY
---------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Sp_Category_GetMostPopular
(
	@take int = null
)
AS
BEGIN
	SELECT c.Id,
		   c.Name,
		   c.Description,
		   c.Image,
		   (SELECT COUNT(1) 
			FROM (SELECT a.CategoryId
				  FROM Article a,
					   Order_Detail od
				  WHERE a.Id = od.ArticleId
				  AND a.CategoryId = c.Id) AS CategorieIdsByArticleFromOrderDetail) repeated
	FROM Category c
	WHERE c.Status = 'ACTIVADO'
	ORDER BY repeated DESC
	OFFSET 0 ROWS 
	FETCH NEXT COALESCE(@take,(SELECT COUNT(1)
							   FROM Category c
							   WHERE c.Status = 'ACTIVADO')) ROWS ONLY; 
END;
GO

--Obtiene cantidad de articulos asignados a la categoria por Id
CREATE PROCEDURE Sp_Category_GetArticleDetails
(
	@CategoryId UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @ArticlesEnabledCount INT = 0;
	DECLARE @ArticlesDisabledCount INT = 0;

	IF EXISTS (SELECT 1 FROM Category WHERE Id = @CategoryId)
	BEGIN
		SET @ArticlesEnabledCount = ISNULL((SELECT COUNT(1) 
											FROM Article a 
											WHERE a.CategoryId = @CategoryId 
											AND a.Status = 'ACTIVADO'), 0);

		SET @ArticlesDisabledCount = ISNULL((SELECT COUNT(1) 
											 FROM Article a 
											 WHERE a.CategoryId = @CategoryId 
										     AND a.Status = 'DESACTIVADO'), 0);
	END;

	SELECT @ArticlesEnabledCount,
		   @ArticlesDisabledCount;

END;
GO


--Obtiene todas las categorias activas y la cantidad de articulos asignados
CREATE PROCEDURE Sp_Category_GetAllAvailableForSearch
AS
BEGIN
	SELECT c.Id,
	       c.Name,
		   c.Description,
		   c.Status,
		   c.image,
		   c.CreationDate,
		   c.ModificationDate,
		   c.AddedBy,
		   c.ModifiedBy,
		   (SELECT COUNT(1) 
		    FROM Article a 
			WHERE a.Status = 'ACTIVADO' 
			AND a.CategoryId = c.Id) AS ArticlesCount
	FROM Category c
	WHERE c.Status = 'ACTIVADO'
	ORDER BY ArticlesCount DESC;

END;
GO

---------------------------------------------------------------------------------------------------------------
-- PROCEDIMIENTOS ROLE
---------------------------------------------------------------------------------------------------------------
--Obtiene los roles de un usuario
CREATE PROCEDURE Sp_Role_GetAllByUserId
(
	@userId UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT r.Id,
		   r.Name,
		   r.Description,
		   r.CreationDate,
		   r.ModificationDate,
		   r.AddedBy,
		   r.ModifiedBy
	FROM dbo.[Role] r, AppUser_Role ar
	WHERE r.Id = ar.RoleId
	AND ar.AppUserId = @userId
	ORDER BY r.Name;
END;
GO

---------------------------------------------------------------------------------------------------------------
-- TRIGGERS ARTICLE
---------------------------------------------------------------------------------------------------------------
--Registra movimiento de insercion en la tb Article
CREATE TRIGGER TRG_Article_RegMovement_Insert
ON Article
FOR INSERT
AS
BEGIN
	DECLARE curArticles CURSOR FOR SELECT i.Description,
										  i.BarCode,
										  i.CreationDate,
										  i.AddedBy
								   FROM inserted i;
	
	DECLARE @Description VARCHAR(255);
	DECLARE @BarCode VARCHAR(255);
	DECLARE @CreationDate DATETIME;
	DECLARE @AddedBy VARCHAR(40);

	OPEN curArticles
	FETCH NEXT FROM curArticles INTO @Description, @BarCode, @CreationDate, @AddedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@AddedBy,@CreationDate,'INSERT','INSERT Articulo ' + @Description + ' | '+@BarCode);

		FETCH NEXT FROM curArticles INTO @Description, @BarCode, @CreationDate, @AddedBy
	END
	CLOSE curArticles
	DEALLOCATE curArticles
END;
GO

--Registra movimiento de actualizacion en la tb Article
CREATE TRIGGER TRG_Article_RegMovement_Update
ON Article
FOR UPDATE
AS
BEGIN
	DECLARE curArticles CURSOR FOR SELECT i.Description,
										  i.BarCode,
										  i.ModificationDate,
										  i.ModifiedBy
								   FROM inserted i;
	
	DECLARE @Description VARCHAR(255);
	DECLARE @BarCode VARCHAR(255);
	DECLARE @ModificationDate DATETIME;
	DECLARE @ModifiedBy VARCHAR(40);

	OPEN curArticles
	FETCH NEXT FROM curArticles INTO @Description, @BarCode, @ModificationDate, @ModifiedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@ModifiedBy,@ModificationDate,'UPDATE','UPDATE Articulo ' + @Description + ' | '+@BarCode);

		FETCH NEXT FROM curArticles INTO @Description, @BarCode, @ModificationDate, @ModifiedBy
	END
	CLOSE curArticles
	DEALLOCATE curArticles
END;
GO

---------------------------------------------------------------------------------------------------------------
-- TRIGGERS CATEGORY
---------------------------------------------------------------------------------------------------------------
CREATE TRIGGER TRG_Category_RegMovement_Insert
ON Category
FOR INSERT
AS
BEGIN
										  
	DECLARE curCategories CURSOR FOR SELECT i.Name,
										  i.Description,
										  i.CreationDate,
										  i.AddedBy
								   FROM inserted i;
	
	DECLARE @Name VARCHAR(50);
	DECLARE @Description VARCHAR(255);
	DECLARE @CreationDate DATETIME;
	DECLARE @AddedBy VARCHAR(40);

	OPEN curCategories
	FETCH NEXT FROM curCategories INTO @Name, @Description, @CreationDate, @AddedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@AddedBy,@CreationDate,'INSERT','INSERT Categoria ' + @Name);

		FETCH NEXT FROM curCategories INTO @Name, @Description, @CreationDate, @AddedBy
	END
	CLOSE curCategories
	DEALLOCATE curCategories
END;
GO

CREATE TRIGGER TRG_Category_RegMovement_Update
ON Category
FOR UPDATE
AS
BEGIN
										  
	DECLARE curCategories CURSOR FOR SELECT i.Name,
										  i.Description,
										  i.ModificationDate,
										  i.ModifiedBy
								   FROM inserted i;
	
	DECLARE @Name VARCHAR(50);
	DECLARE @Description VARCHAR(255);
	DECLARE @ModificationDate DATETIME;
	DECLARE @ModifiedBy VARCHAR(40);

	OPEN curCategories
	FETCH NEXT FROM curCategories INTO @Name, @Description, @ModificationDate, @ModifiedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@ModifiedBy,@ModificationDate,'UPDATE','UPDATE Categoria ' + @Name);

		FETCH NEXT FROM curCategories INTO @Name, @Description, @ModificationDate, @ModifiedBy
	END
	CLOSE curCategories
	DEALLOCATE curCategories
END;
GO



---------------------------------------------------------------------------------------------------------------
-- TRIGGERS ROLE
---------------------------------------------------------------------------------------------------------------
CREATE TRIGGER TRG_Role_RegMovement_Insert
ON [Role]
FOR INSERT
AS
BEGIN
										  
	DECLARE curRoles CURSOR FOR SELECT i.Name,
										  i.Description,
										  i.CreationDate,
										  i.AddedBy
								   FROM inserted i;
	
	DECLARE @Name VARCHAR(50);
	DECLARE @Description VARCHAR(255);
	DECLARE @CreationDate DATETIME;
	DECLARE @AddedBy VARCHAR(40);

	OPEN curRoles
	FETCH NEXT FROM curRoles INTO @Name, @Description, @CreationDate, @AddedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@AddedBy,@CreationDate,'INSERT','INSERT Rol ' + @Name);

		FETCH NEXT FROM curRoles INTO @Name, @Description, @CreationDate, @AddedBy
	END
	CLOSE curRoles
	DEALLOCATE curRoles
END;
GO

CREATE TRIGGER TRG_Role_RegMovement_Update
ON [Role]
FOR UPDATE
AS
BEGIN
										  
	DECLARE curRoles CURSOR FOR SELECT i.Name,
										  i.Description,
										  i.ModificationDate,
										  i.ModifiedBy
								   FROM inserted i;
	
	DECLARE @Name VARCHAR(50);
	DECLARE @Description VARCHAR(255);
	DECLARE @ModificationDate DATETIME;
	DECLARE @ModifiedBy VARCHAR(40);

	OPEN curRoles
	FETCH NEXT FROM curRoles INTO @Name, @Description, @ModificationDate, @ModifiedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@ModifiedBy,@ModificationDate,'UPDATE','UPDATE Rol ' + @Name);

		FETCH NEXT FROM curRoles INTO @Name, @Description, @ModificationDate, @ModifiedBy
	END
	CLOSE curRoles
	DEALLOCATE curRoles
END;
GO

/*
CREATE TRIGGER TRG_Role_RegMovement_Delete
ON [Role]
FOR DELETE
AS
BEGIN
										  
	DECLARE curRoles CURSOR FOR SELECT d.Name,
										  d.Description,
										  d.ModifiedBy
								   FROM deleted d;
	
	DECLARE @Name VARCHAR(50);
	DECLARE @Description VARCHAR(255);
	DECLARE @ModificationDate DATETIME = GETDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'UTC';
	DECLARE @ModifiedBy VARCHAR(40);

	OPEN curRoles
	FETCH NEXT FROM curRoles INTO @Name, @Description, @ModifiedBy
	WHILE @@fetch_status = 0
	BEGIN
		INSERT INTO Binnacle_Movement(PerformedBy,MovementDate,Type,Detail)
		VALUES(@ModifiedBy,@ModificationDate,'DELETE','DELETE Rol ' + @Name);

		FETCH NEXT FROM curRoles INTO @Name, @Description, @ModifiedBy
	END
	CLOSE curRoles
	DEALLOCATE curRoles
END;
GO*/


/*

select bm.Type,
	   bm.Detail,
	   bm.MovementDate,
	   CASE
	   WHEN bm.PerformedBy = 'SYSTEM' THEN bm.PerformedBy
	   ELSE (SELECT Name FROM AppUser WHERE Id = CONVERT(UNIQUEIDENTIFIER,bm.PerformedBy)) END AS PerformedBY
from Binnacle_Movement bm

*/