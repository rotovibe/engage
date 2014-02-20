-- =============================================
-- Author:		Josh Gattis
-- Create date: 05/06/2011
-- Description:	Sets FailedPasswordAttemptCount
--				to given count for the given 
--				UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetFailedPasswordAttemptCount]
	@UserId uniqueidentifier,
	@NewCount int
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	UPDATE Membership
	SET FailedPasswordAttemptCount = @NewCount
	WHERE UserId = @UserId
	
END
