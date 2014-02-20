CREATE PROC [dbo].[spPhy_UserContractDeleteByUserId] 
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[UserContract]
	WHERE  [UserId] = @UserId

	COMMIT
