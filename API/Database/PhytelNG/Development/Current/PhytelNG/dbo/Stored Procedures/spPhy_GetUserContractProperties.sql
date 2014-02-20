CREATE PROCEDURE [dbo].[spPhy_GetUserContractProperties]
	@UserId UniqueIdentifier, 
    @ContractId INT
AS 
	SET NOCOUNT ON 
	
	SELECT 
		[Key],
		[Value]
	FROM 
		[dbo].[UserContractProperty] ucp with(nolock)
	WHERE ucp.UserId = @UserId  
		AND ucp.ContractId = @ContractId
