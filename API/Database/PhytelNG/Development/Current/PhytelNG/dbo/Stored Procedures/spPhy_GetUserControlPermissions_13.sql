CREATE PROCEDURE [dbo].[spPhy_GetUserControlPermissions_13] 
@UserId uniqueidentifier,
@ContractId int
AS
BEGIN
	SET NOCOUNT ON

	CREATE TABLE #UserRoles (RoleId uniqueidentifier)
	
	CREATE TABLE #Permissions
	(
		PermissionId int
		,Name varchar(200)
		,[Description] varchar(1000)
		,PermissionTypeId int
		,[Path] varchar(500)
		,DisplayOrder int
		,IsTab bit
	)
	
	IF (SELECT UserTypeId FROM [User] WHERE UserId = @UserId) = 1 --Phytel User
	BEGIN
		Insert Into #UserRoles(RoleId)
		SELECT RoleId FROM UserRole where UserId = @UserId
	END
	ELSE --Assume user has contract
	BEGIN
		Insert Into #UserRoles(RoleId)
		Select RoleId From fnPhy_GetUserRolesByContract(@UserId, @ContractId)
	END
	
	--Get all Role Permissions for that user
	INSERT INTO #Permissions
	SELECT DISTINCT 
		p.PermissionId
		, p.Name
		, p.[Description]
		, p.PermissionTypeId
		, c.[Path_13] AS [Path]
		, cprop.DisplayOrder
		, COALESCE(cprop.IsTab, 0) as IsTab
	FROM #UserRoles ur with(nolock)
	INNER JOIN RolePermission rp with(nolock) ON rp.RoleId = ur.RoleId
	INNER JOIN Permission p with(nolock) ON p.PermissionId = rp.PermissionId		
	INNER JOIN ControlPermission cp with(nolock) ON cp.PermissionId = p.PermissionId
	INNER JOIN [Control] c with(nolock) ON c.ControlId = cp.ControlId
	LEFT OUTER JOIN ControlProperty cprop with(nolock) ON c.ControlId = cprop.ControlId	
	WHERE p.PermissionId NOT IN
	(
		SELECT PermissionId
		FROM UserPermission with(nolock)
		WHERE Include = 0
		AND UserId = @UserId
	)
	
	--Insert the Included Permissions outside of a role
	INSERT INTO #Permissions
	SELECT DISTINCT
		p.PermissionId
		, p.Name
		, p.[Description]
		, p.PermissionTypeId
		, c.[Path_13] AS [Path]
		, cprop.DisplayOrder
		, COALESCE(cprop.IsTab, 0) as IsTab
	FROM UserPermission up with(nolock)
	INNER JOIN Permission p with(nolock) ON p.PermissionId = up.PermissionId		
	INNER JOIN [User] u with(nolock) ON u.UserId = up.UserId
	LEFT OUTER JOIN ControlPermission cp with(nolock) ON cp.PermissionId = p.PermissionId
	LEFT OUTER JOIN [Control] c with(nolock) ON c.ControlId = cp.ControlId
	LEFT OUTER JOIN ControlProperty cprop with(nolock) ON c.ControlId = cprop.ControlId	
	WHERE u.UserId = @UserId
	and up.Include = 1
	
	--Grab user permissions
	SELECT DISTINCT
		PermissionId
		,Name
		,[Description]
		,PermissionTypeId
		,[Path]
		,DisplayOrder
		,IsTab
	FROM #Permissions
	ORDER BY DisplayOrder
	
	DROP TABLE #Permissions

END
