-- =============================================
-- Author:		CHill
-- Create date: 4/13/2011
-- Description:	gets user password attributes
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetUserPassword] @UserName nvarchar(256)
AS
BEGIN
SET NOCOUNT ON;

SELECT @UserName, m.PasswordExpiration
FROM Membership m with(nolock)
INNER JOIN [User] u with(nolock) ON m.UserId = u.UserId
WHERE u.UserName = @UserName

END
