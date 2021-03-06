SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Current_Email]
AS

SELECT    TOP (100) PERCENT ContactId, Text, OptOut, Preferred
                          ,(SELECT     Name
                            FROM          dbo.RPT_CommTypeLookUp AS t with (nolock) 
                            WHERE      (CommTypeId = ce.TypeId)) AS Type
FROM         dbo.RPT_ContactEmail AS ce with (nolock) 
WHERE     ([Delete] = 'False') --AND ce.Preferred = 'True'  
AND ce.OptOut = 'False'
GO
