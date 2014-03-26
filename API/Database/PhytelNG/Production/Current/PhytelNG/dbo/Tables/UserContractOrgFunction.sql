CREATE TABLE [dbo].[UserContractOrgFunction] (
    [UserContractOrgFunctionId] INT              IDENTITY (1, 1) NOT NULL,
    [ContractOrgFunctionId]     INT              NOT NULL,
    [UserId]                    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_UserContractOrgFunction] PRIMARY KEY CLUSTERED ([UserContractOrgFunctionId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_UserContractOrgFunction_ContractOrgFunction] FOREIGN KEY ([ContractOrgFunctionId]) REFERENCES [dbo].[ContractOrgFunction] ([ContractOrgFunctionId]),
    CONSTRAINT [FK_UserContractOrgFunction_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCIDX_UserContractOrgFunction_UniqueConstraint]
    ON [dbo].[UserContractOrgFunction]([UserContractOrgFunctionId] ASC, [UserId] ASC) WITH (FILLFACTOR = 90);

