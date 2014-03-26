-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/25/2011
-- Description:	Inserts the provided password
--				for the given UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetPasswordHistory]
	@UserId uniqueidentifier,
	@Password varchar(100)
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	DECLARE @Total INT
	DECLARE @UserCount INT
			
	SELECT @Total = CAST([Value] AS INT) FROM ApplicationSetting WHERE [Key] = 'PWD_HISTORY_COUNT'	
	SELECT @UserCount = COUNT([Password]) FROM PasswordHistory WHERE UserId = @UserId

	IF (@UserCount > (@Total - 1))
	BEGIN
		DELETE x
		FROM (	SELECT UserID, Password, PasswordChangedDate, 
				ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY PasswordChangedDate DESC) AS Seq
				FROM PasswordHistory) x
		WHERE x.Seq > (@Total - 1)
			AND x.UserId = @UserId		
	END
	
	INSERT INTO PasswordHistory(UserId, [Password], PasswordChangedDate) Values (@UserId, @Password, GETDATE())
	
END
