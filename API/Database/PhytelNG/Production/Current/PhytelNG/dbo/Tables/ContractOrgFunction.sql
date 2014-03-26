CREATE TABLE [dbo].[ContractOrgFunction] (
    [ContractOrgFunctionId] INT IDENTITY (1, 1) NOT NULL,
    [ContractId]            INT NOT NULL,
    [OrgFunctionId]         INT NOT NULL,
    [DeleteFlag]            BIT CONSTRAINT [DF_ContractOrgFunction_DeleteFlag] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContractOrgFunction] PRIMARY KEY CLUSTERED ([ContractOrgFunctionId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ContractOrgFunction_Contract] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_ContractOrgFunction_OrgFunction] FOREIGN KEY ([OrgFunctionId]) REFERENCES [dbo].[OrgFunction] ([OrgFunctionId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCIDX_ContractOrgFunction_UniqueConstraint]
    ON [dbo].[ContractOrgFunction]([ContractId] ASC, [OrgFunctionId] ASC, [DeleteFlag] ASC) WITH (FILLFACTOR = 90);

