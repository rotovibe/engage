CREATE TABLE [dbo].[Contract] (
    [ContractId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [Number]     VARCHAR (30) NOT NULL,
    [StatusCode] VARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED ([ContractId] ASC) WITH (FILLFACTOR = 80)
);

