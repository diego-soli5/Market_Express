USE MARKET_EXPRESS
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
		   p.Description,
		   p.Name
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
		IF (SELECT Status FROM Cart WHERE Id = @vCartId) = 'ABIERTO'
		BEGIN
			IF (SELECT COUNT(1) FROM Cart_Detail WHERE CartId = @vCartId) > 0
			BEGIN
				SET @vCount = (SELECT SUM(Quantity) FROM Cart_Detail WHERE CartId = @vCartId);
			END
		END
	END 

	SELECT @vCount;
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
-- PROCEDIMIENTOS CATEGORY
---------------------------------------------------------------------------------------------------------------
--Obtiene las direcciones del cliente por Id de usuario
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
	
	DECLARE @Name VARCHAR(20);
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
	
	DECLARE @Name VARCHAR(20);
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