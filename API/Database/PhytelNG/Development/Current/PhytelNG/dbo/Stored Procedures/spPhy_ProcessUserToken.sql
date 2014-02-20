-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 11/1/2013
-- Description:	Inserts a UserToken record for a one time login request from a application
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_ProcessUserToken]
	@Token varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @UserID uniqueidentifier
	
    -- Insert statements for procedure here
	IF Exists(Select Top 1 1 From APIUserToken with(nolock) Where Token = @Token)
	BEGIN
		Select @UserID = UserID From APIUserToken  with(nolock) Where Token = @Token
	END
	
	DELETE FROM APIUserToken Where Token = @Token
	
	SELECT UserID, UserName, FirstName, LastName, SessionTimeout
	From [User] with(nolock)
	Where UserID = @UserID
	
	SELECT c.ContractId, c.Name, c.Number
	From [Contract] c with(nolock)
		Inner Join UserContract uc with(nolock) on c.ContractId = uc.ContractId
	Where uc.UserId = @UserID
	
END
