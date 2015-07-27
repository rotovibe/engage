IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_GetContractDBConnection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_GetContractDBConnection]
GO

CREATE PROCEDURE [dbo].[spPhy_GetContractDBConnection] 
    @ContractId INT = NULL, 
    @ContractNumber VARCHAR(30) = NULL,
    @DatabaseType VARCHAR(10) = 'SQL',
    @SystemType VARCHAR(10) = 'Contract'


AS
	IF(@DatabaseType = 'SQL')
	BEGIN
		SELECT
			c.[ContractId]
			, [Name]
			, [Number] 
			, srvs.IpAddress as [Server]
			, '' as [ReplicationSetName]
			, cs.ContractDatabase as [Database]
			, c.UserName
			, c.[Password]
		FROM 
			[dbo].[Contract] c with(nolock)
			INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.Contracts cs  with(nolock) on 
				c.Number = cs.ContractNumber
			INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.[Servers] srvs  with(nolock) on 
				cs.ContractDatabaseServer = srvs.ServerId
		WHERE 
			c.ContractId = COALESCE(@ContractId, c.ContractId)
		    AND c.Number = COALESCE(@ContractNumber, c.Number)
	END
	ELSE
	BEGIN
		SELECT
			c.[ContractId]
			, [Name]
			, [Number] 
			, srvm.IpAddress as [Server]
			, '' as [ReplicationSetName]
			, cs.ContractDatabase as [Database]
			, c.Username
			, c.MongoPassword as [Password]
		FROM 
			[dbo].[Contract] c with(nolock)
			INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.Contracts cs  with(nolock) on 
				c.Number = cs.ContractNumber
			INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.[Servers] srvm  with(nolock) on 
				cs.MongoDatabaseServer = srvm.ServerID
		
		WHERE 
			c.ContractId = COALESCE(@ContractId, c.ContractId)
			AND c.Number = COALESCE(@ContractNumber, c.Number)
	END


