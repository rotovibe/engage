CREATE PROC [dbo].[spPhy_GetAllControlPermissions]
AS 
BEGIN

	SET NOCOUNT ON 
	
	SELECT 
		cp.[ControlId]
		,cp.[PermissionId]
		,p.[PermissionTypeId]
	FROM   
		ControlPermission cp
	INNER JOIN Permission p with(nolock) ON p.PermissionId= cp.PermissionId
	INNER JOIN PermissionType pt with(nolock) ON pt.PermissionTypeId = p.PermissionTypeId

END
