SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Contact_Address]
AS
SELECT     TOP (100) PERCENT ContactId, Line1, Line2, Line3, City, optout,
                          (SELECT     Code
                            FROM          dbo.RPT_StateLookUp AS st with (nolock) 
                            WHERE      (StateId = ca.StateId)) AS State, PostalCode AS ZIP,
                          (SELECT     Name
                            FROM          dbo.RPT_CommTypeLookUp AS t with (nolock) 
                            WHERE      (CommTypeId = ca.TypeId)) AS Type, 
                            --MIN(COALESCE (LastUpdatedOn, RecordCreatedOn)) AS date,
                            ca.Preferred
FROM         dbo.RPT_ContactAddress AS ca with (nolock) 
WHERE     ([Delete] = 'False')
AND ca.optout = 'False'
ORDER BY ca.Preferred DESC
GO
