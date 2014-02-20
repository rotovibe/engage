CREATE PROC [dbo].[spPhy_GetAllControls] 
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
		, v.[IsVisible]
		, v.[ParentControlId]
		, c2.Name as ParentControlName
	FROM vw_Control v with(nolock) 
	LEFT OUTER JOIN [Control] c2 with(nolock) ON c2.ControlId = v.ParentControlId
