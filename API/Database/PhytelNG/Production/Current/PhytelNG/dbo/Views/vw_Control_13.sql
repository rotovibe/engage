CREATE VIEW [dbo].[vw_Control_13]
AS
SELECT     
	dbo.Control.ControlId, 
	dbo.Control.Name, 
	dbo.Control.Path_13 AS [Path],
	dbo.Control.Description, 
	dbo.ControlProperty.DisplayOrder, 
	dbo.ControlProperty.IsSubTab, 
    dbo.ControlProperty.ParentControlId, 
    dbo.ControlProperty.IsVisible, 
    dbo.ControlProperty.IsTab
FROM
    dbo.Control INNER JOIN
    dbo.ControlProperty ON dbo.Control.ControlId = dbo.ControlProperty.ControlId
