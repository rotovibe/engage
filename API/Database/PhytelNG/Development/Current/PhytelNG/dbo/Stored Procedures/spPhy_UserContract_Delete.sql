CREATE PROC [dbo].[spPhy_UserContract_Delete] 
    @ContractId int,
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   
		[dbo].[UserContract]
	WHERE  
		[ContractId] = @ContractId
	AND 
		[UserId] = @UserId

	COMMIT
