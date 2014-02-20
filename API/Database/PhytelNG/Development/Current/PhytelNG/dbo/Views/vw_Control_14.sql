-- ==========================================================
-- Author:		Kishore Geddada
-- Create date: 08/30/2013
-- Description:	View to Get the Data from New Column TabType
-- ==========================================================

CREATE VIEW [dbo].[vw_Control_14]
AS
SELECT      dbo.Control.ControlId, 
			dbo.Control.Name, 
			dbo.Control.Path_13 AS Path, 
			dbo.Control.Description, 
			dbo.ControlProperty.DisplayOrder, 
            dbo.ControlProperty.ParentControlId, 
            dbo.ControlProperty.IsVisible, 
            dbo.ControlProperty.TabType, 
            dbo.ControlProperty.IsTab, 
            dbo.ControlProperty.IsSubTab
FROM         dbo.Control INNER JOIN
                      dbo.ControlProperty ON dbo.Control.ControlId = dbo.ControlProperty.ControlId
