CREATE PROCEDURE [dbo].[spPhy_GetContractById] 
    @ContractId INT
AS 
	SET NOCOUNT ON 
	
	SELECT 
		c.[ContractId]
		, [Name]
		, [Number] 
		, srvs.IpAddress as [Server]
		, srvsM.IpAddress as [MongoServer]
		, cds.DatabaseName as [Database]
		, cdm.DatabaseName as [MongoDatabase]
		, cds.UserName
		, cds.[Password]
		, cdm.UserName as MongoUserName
		, cdm.[Password] as MongoPassword
		, c.ContractID as PhytelContractId
		, 0 as DefaultContract
	FROM [dbo].[Contract] c with(nolock)
	INNER JOIN ContractDatabase cds  with(nolock) on c.ContractId = cds.ContractId AND cds.DatabaseType = 'SQL' and cds.SystemType = 'Contract'
	INNER JOIN ContractDatabase cdm with(nolock) on c.ContractId = cdm.ContractId AND cdm.DatabaseType = 'MONGO' and cdm.SystemType = 'Contract'
	INNER JOIN [Servers] srvs  with(nolock) on cds.ServerId = srvs.ServerId
	INNER JOIN [Servers] srvsM  with(nolock) on cdm.ServerId = srvsM.ServerId
	WHERE c.ContractId = @ContractId
