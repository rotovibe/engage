-- =============================================
-- Author:		Josh Gattis
-- Create date: 05/06/2011
-- Description:	Sets FailedPasswordAnswerAttemptCount
--				to given count for the given 
--				UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetFailedPasswordAnswerAttemptCount]
	@UserId uniqueidentifier,
	@NewCount int
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	UPDATE Membership
	SET FailedPasswordAnswerAttemptCount = @NewCount
	WHERE UserId = @UserId
	
END
