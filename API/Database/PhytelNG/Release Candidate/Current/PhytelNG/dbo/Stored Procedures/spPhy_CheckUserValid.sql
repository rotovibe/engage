-- =============================================
-- Author:		CHill
-- Create date: 4/13/2011
-- Description:	gets user password attributes
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_CheckUserValid] @UserName nvarchar(256)
AS
BEGIN
SET NOCOUNT ON;

SELECT --CASE WHEN m.SessionID IS NULL OR DATALENGTH(m.SessionID) = 0 THEN 0 ELSE 1 END as ExistingSession,
Status as Condition,
CASE	WHEN Status = 'Inactive' THEN 'ERR_010'
		WHEN Status = 'Deleted' THEN 'ERR_011'
		WHEN Status = 'Locked' THEN 'ERR_012'
END as Message,
0 as ExistingSession,
DATEDIFF(dd, getdate(),m.PasswordExpiration) DaysTillPasswordExpiration 
FROM Membership m with(nolock) 
INNER JOIN [User] u with(nolock) ON m.UserId = u.UserId
WHERE u.UserName = @UserName

END
