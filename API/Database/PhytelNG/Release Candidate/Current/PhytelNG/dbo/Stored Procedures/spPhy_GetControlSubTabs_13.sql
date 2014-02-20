CREATE PROC [dbo].[spPhy_GetControlSubTabs_13] 
AS 
	SET NOCOUNT ON 
	
	SELECT 
		v.[ControlId]
		, v.[Description]
		, v.[DisplayOrder]
		, v.[Name]
		, v.[Path]
		, v.[DisplayOrder]
		, v.[IsTab]
		, v.[IsSubTab]
		, v.[ParentControlId]
		, c2.Name as ParentControlName
	FROM vw_Control_13 v with(nolock)
		LEFT OUTER JOIN [Control] c2 with(nolock) ON c2.ControlId = v.ParentControlId 
	WHERE IsSubTab = 1
	ORDER BY v.[DisplayOrder] ASC

