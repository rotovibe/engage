/************* SCHEMA CHANGES *********************************************/
Use [PhytelMaster]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoDatabaseServer' AND Object_ID = Object_ID(N'Contracts'))
ALTER TABLE dbo.Contracts ADD MongoDatabaseServer int NOT NULL CONSTRAINT DF_Contracts_MongoDatabaseServer DEFAULT ((0))
GO

Use [C3]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPassword' AND Object_ID = Object_ID(N'Contract'))
ALTER TABLE dbo.Contract ADD MongoPassword nvarchar(128) NULL
GO


/************* PROCEDURE CHANGES *********************************************/
Use [C3]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_GetContract]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_GetContract]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_GetContractById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].spPhy_GetContractById
GO

CREATE PROCEDURE [dbo].[spPhy_GetContract] 
    @ContractId INT = NULL, @ContractNumber VARCHAR(30) = NULL
AS 
	SET NOCOUNT ON 
	
	SELECT 
		c.[ContractId]
		, [Name]
		, [Number] 
		, srvs.IpAddress as [Server]
		, srvsM.IpAddress as [MongoServer]
		, cs.ContractDatabase as [Database]
		, cs.ContractDatabase as [MongoDatabase]
		, c.UserName
		, c.[Password]
		, c.UserName as MongoUserName
		, c.MongoPassword
		, cs.ContractID as PhytelContractId
		, 0 as DefaultContract
	FROM [dbo].[Contract] c with(nolock)
	INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.Contracts cs  with(nolock) on c.Number = cs.ContractNumber
	INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.[Servers] srvs  with(nolock) on cs.ContractDatabaseServer = srvs.ServerId
	LEFT JOIN DALSQLCLUSTER.PhytelMaster.dbo.[Servers] srvsM  with(nolock) on cs.MongoDatabaseServer = srvsM.ServerId
	WHERE c.ContractId = COALESCE(@ContractId, c.ContractId)
	AND c.Number = COALESCE(@ContractNumber, c.Number)
GO

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
		, cs.ContractDatabase as [Database]
		, cs.ContractDatabase as [MongoDatabase]
		, c.UserName
		, c.[Password]
		, c.UserName as MongoUserName
		, c.MongoPassword
		, cs.ContractID as PhytelContractId
		, 0 as DefaultContract
	FROM [dbo].[Contract] c with(nolock)
	INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.Contracts cs  with(nolock) on c.Number = cs.ContractNumber
	INNER JOIN DALSQLCLUSTER.PhytelMaster.dbo.[Servers] srvs  with(nolock) on cs.ContractDatabaseServer = srvs.ServerId
	LEFT JOIN DALSQLCLUSTER.PhytelMaster.dbo.[Servers] srvsM  with(nolock) on cs.MongoDatabaseServer = srvsM.ServerId
	WHERE  c.ContractId = @ContractId  	
GO



/************* DATA CHANGES *********************************************/
Use [PhytelMaster]
GO

Declare @NewServerID INT

IF NOT EXISTS(Select Top 1 1 From [Servers] Where [IpName] = 'DALMONGO')
BEGIN
	INSERT INTO [Servers]([Description],[TelephonyServerFlag],[ApplicationServerFlag],[WebServerFlag],[MediaServerFlag],[DatabaseServerFlag]
		   ,[HL7ServerFlag],[MessagingServerFlag],[VPNServerFlag],[RedirectFaxNumber],[IpAddress],[IpName],[ServerSiteID],[ContractID]
		   ,[DeleteFlag],[CreateDate],[CreatedBy],[UpdateDate],[UpdatedBy],[ClientPingInterval],[SMTPHostName],[POPServerName]
		   ,[ExternalIPName],[ExternalIPAddress],[ServerType],[CommServer],[ContractSetupFlag])
	VALUES
		   ('Mongo DB Server',0,0,0,0,1,0,0,0,null,'192.168.2.150','DALMONGO',3,null,0,GETDATE(),-10900,GETDATE(),-10900,600000,null
		   ,null,null,null,'PROD',0,0)
END

Select @NewServerID = ServerID From [Servers] Where [IpName] = 'DALMONGO'

Update Contracts Set MongoDatabaseServer = @NewServerID

GO

Use [C3]
GO

/********* Set Mongo Password for all contracts that don't have one to phyM0ng0 ************************/
Update Contract Set MongoPassword = 'b36mskomJ1MVj1m+gNAv4A==' Where MongoPassword IS NULL
GO
