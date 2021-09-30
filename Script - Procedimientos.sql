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
			SET @vCount = (SELECT SUM(Quantity) FROM Cart_Detail WHERE CartId = @vCartId);
		END
	END 

	SELECT @vCount;
END;
GO


