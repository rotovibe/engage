CREATE PROCEDURE [dbo].[spPhy_SetLockOut]
	@UserId uniqueidentifier
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	UPDATE [Membership]
	SET IsLockedOut = 1, LastLockoutDate = GETDATE()
	WHERE UserId = @UserId
	
END
