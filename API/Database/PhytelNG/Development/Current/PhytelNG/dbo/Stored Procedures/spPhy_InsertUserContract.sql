CREATE PROC [dbo].[spPhy_InsertUserContract] 
    @ContractId int,
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	IF NOT EXISTS(SELECT ContractId FROM UserContract WHERE UserId = @UserId AND ContractId = @ContractId)
	BEGIN
	
	INSERT INTO [dbo].[UserContract] ([ContractId], [UserId])
	SELECT @ContractId, @UserId
	
	-- Begin Return Select <- do not remove
	SELECT [ContractId], [UserId], [DefaultContract]
	FROM   [dbo].[UserContract] with(nolock)
	WHERE  [ContractId] = @ContractId
	       AND [UserId] = @UserId
	-- End Return Select <- do not remove
         
    END      
	COMMIT
