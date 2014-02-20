CREATE PROC [dbo].[spPhy_SaveUserSession] @UserId uniqueidentifier, @SessionId nvarchar(88), @SessionTimeOut int, @RemoveSession bit
AS

SET NOCOUNT ON

IF (@RemoveSession = 1)
BEGIN
	DELETE UserSession
	WHERE UserId = @UserId
	AND SessionId = @SessionId
END
ELSE
BEGIN

	IF EXISTS (SELECT 1 FROM UserSession WHERE UserID = @UserId AND SessionID = @SessionId)
	BEGIN
		UPDATE UserSession
		SET LastActivityDate = getdate(),
			ExpirationDate = DATEADD(mi,@SessionTimeOut, getdate())
		WHERE UserId = @UserId
		AND SessionId = @SessionId
	END
	ELSE
	BEGIN
		DECLARE @Date DATETIME, @Expiration DATETIME
		SELECT @Date = getdate()
		SET @Expiration = (SELECT DATEADD(mi, @SessionTimeOut, @Date))
		INSERT UserSession (UserId, SessionId, LastActivityDate, ExpirationDate)
		VALUES (@UserId, @SessionId, @Date, @Expiration)
	END
END

SELECT ExpirationDate
FROM UserSession
WHERE UserId = @UserId
AND SessionId = @SessionId
