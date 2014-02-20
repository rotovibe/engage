CREATE PROC [dbo].[spPhy_GetStatuses]
AS

SELECT DISTINCT Status
FROM Membership with(nolock)
UNION ALL
SELECT DISTINCT 'Locked' as Locked
FROM Membership with(nolock)
WHERE IsLockedOut = 1
