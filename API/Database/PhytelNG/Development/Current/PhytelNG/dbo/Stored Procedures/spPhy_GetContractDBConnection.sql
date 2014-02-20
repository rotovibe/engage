CREATE PROCEDURE [dbo].[spPhy_GetContractDBConnection] 
    @ContractId INT = NULL, 
    @ContractNumber VARCHAR(30) = NULL,
    @DatabaseType VARCHAR(10) = 'SQL',
    @SystemType VARCHAR(10) = 'Contract'
AS
	SET NOCOUNT ON
	
	SELECT
		c.[ContractId]
		, [Name]
		, [Number] 
		, srv.IpAddress as [Server]
		, cd.DatabaseName as [Database]
		, cd.UserName
		, cd.[Password]
		, cd.[IsPrimary]
		, srv.ReplicationSetName
	FROM [dbo].[Contract] c with(nolock)
	INNER JOIN ContractDatabase cd  with(nolock) on c.ContractId = cd.ContractId 
	INNER JOIN [Servers] srv  with(nolock) on cd.ServerId = srv.ServerId
	WHERE c.ContractId = COALESCE(@ContractId, c.ContractId)
	AND c.Number = COALESCE(@ContractNumber, c.Number)
	AND cd.DatabaseType = @DatabaseType 
	AND cd.SystemType = @SystemType
	ORDER BY cd.[IsPrimary] DESC