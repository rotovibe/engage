CREATE PROC [dbo].[spPhy_GetControlTabsByUserId_13] 
@UserId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON 

	SELECT 
		DISTINCT(v.[ControlId])
		, v.[Description]
		, v.[Name]
		, v.[Path]
		, v.[DisplayOrder]
		, v.[IsTab]
		, v.[IsSubTab]
		, v.[IsVisible]
		, v.[ParentControlId]
		, c2.Name as ParentControlName
		, ur.UserId
		, u.Lastname
		, u.FirstName
	INTO #tmpUserControls
	FROM vw_Control_13 v with(nolock)
	LEFT OUTER JOIN [Control] c2 with(nolock) ON c2.ControlId = v.ParentControlId 
	INNER JOIN ControlPermission cp with(nolock) ON cp.ControlId = v.ControlId
	INNER JOIN RolePermission rp with(nolock) ON rp.PermissionId = cp.PermissionId
	INNER JOIN UserRole ur with(nolock) ON ur.RoleId = rp.RoleId
	LEFT OUTER JOIN UserPermission up with(nolock) ON up.PermissionId = rp.PermissionId AND Include = 1
	INNER JOIN [User] u with(nolock) ON u.UserId = ur.UserId
	WHERE ur.UserId = @UserId
	AND IsVisible = 1
	AND (IsTab = 1 OR IsSubTab = 1)
	AND rp.PermissionId NOT IN
	(
		SELECT PermissionId
		FROM UserPermission with(nolock)
		WHERE Include = 0
		AND UserId = u.UserId
	)
	ORDER BY v.[DisplayOrder] ASC

	Update t
	Set [Path] = pst.[Path]
	From #tmpUserControls t, 
		(Select ParentControlid, Min(DisplayOrder) as DisplayOrder
			From #tmpUserControls
			Where IsSubTab = 1
			Group By ParentControlid) st
		Inner Join 
			(Select ParentControlid, Path, DisplayOrder
				From #tmpUserControls) pst on st.ParentControlId = pst.ParentControlId and st.DisplayOrder = pst.DisplayOrder
	Where t.ControlId = pst.ParentControlId
	
	Select * from #tmpUserControls ORDER BY IsTab DESC, DisplayOrder ASC
	
	DROP table #tmpUserControls
END
