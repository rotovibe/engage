-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 11/1/2013
-- Description:	Inserts a UserToken record for a one time login request from a application
-- =============================================
CREATE PROCEDURE spPhy_GenerateUserToken
	@UserID uniqueidentifier,
	@Token varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO APIUserToken(UserID, Token) Values (@UserID, @Token)
	
END
