
/****** Object:  Table [dbo].[ContractDatabase]    Script Date: 10/03/2014 11:39:10 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContractDatabase_Contract]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContractDatabase]'))
ALTER TABLE [dbo].[ContractDatabase] DROP CONSTRAINT [FK_ContractDatabase_Contract]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContractDatabase_Servers]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContractDatabase]'))
ALTER TABLE [dbo].[ContractDatabase] DROP CONSTRAINT [FK_ContractDatabase_Servers]
GO

IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_ContractDatabase]') AND parent_object_id = OBJECT_ID(N'[dbo].[ContractDatabase]'))
ALTER TABLE [dbo].[ContractDatabase] DROP CONSTRAINT [CK_ContractDatabase]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ContractDatabase_IsPrimary]') AND type = 'D')
ALTER TABLE [dbo].[ContractDatabase] DROP CONSTRAINT [DF_ContractDatabase_IsPrimary]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContractDatabase]') AND type in (N'U'))
DROP TABLE [dbo].[ContractDatabase]
GO


/****** Object:  Table [dbo].[Servers]    Script Date: 10/03/2014 11:38:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Servers]') AND type in (N'U'))
DROP TABLE [dbo].[Servers]
GO

CREATE TABLE [dbo].[Servers](
	[ServerID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[IpAddress] [varchar](50) NOT NULL,
	[IpName] [varchar](50) NOT NULL,
	[DeleteFlag] [tinyint] NOT NULL,
	[ServerType] [char](10) NOT NULL,
	[ReplicationSetName] [varchar](50) NULL,
 CONSTRAINT [PK_SERVERS] PRIMARY KEY CLUSTERED 
(
	[ServerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ContractDatabase](
	[ContractDatabaseID] [int] IDENTITY(1,1) NOT NULL,
	[ContractID] [int] NOT NULL,
	[ServerID] [int] NOT NULL,
	[DatabaseName] [varchar](100) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[Password] [varchar](500) NOT NULL,
	[DatabaseType] [varchar](10) NOT NULL,
	[SystemType] [varchar](10) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_ContractDatabase] PRIMARY KEY CLUSTERED 
(
	[ContractDatabaseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractDatabase]  WITH CHECK ADD  CONSTRAINT [FK_ContractDatabase_Contract] FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contract] ([ContractId])
GO

ALTER TABLE [dbo].[ContractDatabase] CHECK CONSTRAINT [FK_ContractDatabase_Contract]
GO

ALTER TABLE [dbo].[ContractDatabase]  WITH CHECK ADD  CONSTRAINT [FK_ContractDatabase_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO

ALTER TABLE [dbo].[ContractDatabase] CHECK CONSTRAINT [FK_ContractDatabase_Servers]
GO

ALTER TABLE [dbo].[ContractDatabase]  WITH CHECK ADD  CONSTRAINT [CK_ContractDatabase] CHECK  (([DatabaseType]='MONGO' OR [DatabaseType]='SQL'))
GO

ALTER TABLE [dbo].[ContractDatabase] CHECK CONSTRAINT [CK_ContractDatabase]
GO

ALTER TABLE [dbo].[ContractDatabase] ADD  CONSTRAINT [DF_ContractDatabase_IsPrimary]  DEFAULT ((1)) FOR [IsPrimary]
GO

/************* PROCEDURE CHANGES *********************************************/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_GetContractDBConnection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_GetContractDBConnection]
GO

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
	FROM [dbo].[Contract] c with(nolock)
	INNER JOIN ContractDatabase cd  with(nolock) on c.ContractId = cd.ContractId 
	INNER JOIN [Servers] srv  with(nolock) on cd.ServerId = srv.ServerId
	WHERE c.ContractId = COALESCE(@ContractId, c.ContractId)
	AND c.Number = COALESCE(@ContractNumber, c.Number)
	AND cd.DatabaseType = @DatabaseType 
	AND cd.SystemType = @SystemType
GO

/************* DATA CHANGES *********************************************/

--Go get all servers from PhytelMaster and import into Servers table
SET IDENTITY_INSERT [Servers] ON

Insert Into [Servers](ServerID, Description, IpAddress, IpName, DeleteFlag, ServerType, ReplicationSetName)
Select ServerID, Description, IpAddress, IpName, 0, ServerType, Null
From DALSQLCLUSTER.PhytelMaster.DBO.Servers Where DeleteFlag = 0 And DatabaseServerFlag = 1

SET IDENTITY_INSERT [Servers] OFF

Insert Into ContractDatabase(ContractID, ServerID, DatabaseName, UserName, [Password], DatabaseType, SystemType, IsPrimary)
Select pmc.ContractID, cs.ServerID, pmc.ContractDatabase, c.Username, c.[Password], 'SQL', 'Contract', 1
From PhytelMaster.DBO.Contracts pmc
	Inner Join [Contract] c on pmc.ContractID = c.ContractID
	Inner Join [Servers] cs on pmc.ContractDatabaseServer = cs.ServerID
Union
Select pmc.ContractID, cs.ServerID, pmc.ContractDatabase, c.Username, c.[Password], 'Mongo', 'Contract', 1
From PhytelMaster.DBO.Contracts pmc
	Inner Join [Contract] c on pmc.ContractID = c.ContractID
	Inner Join [Servers] cs on pmc.MongoDatabaseServer = cs.ServerID
