-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/27/2011
-- Description:	Sets PasswordQuestion and
--				PasswordAnswer to the new values
--				passed in for the given UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetPasswordQuestionAndAnswer]
	@UserId uniqueidentifier,
	@PasswordQuestion nvarchar(256),
	@PasswordAnswer nvarchar(128)
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	UPDATE Membership
	SET PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer
	WHERE UserId = @UserId
	
END
