CREATE PROC [dbo].[spPhy_GetAllContracts] 
AS 
	SELECT 
		c.[ContractId]
		, [Name]
		, [Number] 
		, srvs.IpAddress as [Server]
		, cd.DatabaseName as [Database]
		, cd.UserName
		, cd.[Password]
		, c.ContractID as PhytelContractId
		, 0 as DefaultContract
	FROM [dbo].[Contract] c with(nolock)
	INNER JOIN ContractDatabase cd with(nolock) on c.ContractId = cd.ContractId
	INNER JOIN [Servers] srvs with(nolock) on cd.ServerId = srvs.ServerId
	WHERE cd.DatabaseType = 'SQL' And cd.SystemType = 'Contract'

