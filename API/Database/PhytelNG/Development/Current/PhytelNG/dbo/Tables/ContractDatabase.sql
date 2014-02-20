CREATE TABLE [dbo].[ContractDatabase] (
    [ContractDatabaseID] INT           IDENTITY (1, 1) NOT NULL,
    [ContractID]         INT           NOT NULL,
    [ServerID]           INT           NOT NULL,
    [DatabaseName]       VARCHAR (100) NOT NULL,
    [UserName]           VARCHAR (100) NOT NULL,
    [Password]           VARCHAR (500) NOT NULL,
    [DatabaseType]       VARCHAR (10)  NOT NULL,
    [SystemType]         VARCHAR (10)  NOT NULL,
    [IsPrimary]          BIT           CONSTRAINT [DF_ContractDatabase_IsPrimary] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ContractDatabase] PRIMARY KEY CLUSTERED ([ContractDatabaseID] ASC),
    CONSTRAINT [CK_ContractDatabase] CHECK ([DatabaseType]='MONGO' OR [DatabaseType]='SQL'),
    CONSTRAINT [FK_ContractDatabase_Contract] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_ContractDatabase_Servers] FOREIGN KEY ([ServerID]) REFERENCES [dbo].[Servers] ([ServerID])
);

