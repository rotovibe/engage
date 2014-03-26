-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/27/2011
-- Description:	Sets NewUser flag to 0
--				for the given UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetFirstTimeUser]
	@UserId uniqueidentifier
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	UPDATE [User]
	SET NewUser = 0
	WHERE UserId = @UserId
	
END
