CREATE TABLE [dbo].[OrgFunction] (
    [OrgFunctionId]   INT           IDENTITY (1, 1) NOT NULL,
    [OrgFunctionName] VARCHAR (256) NOT NULL,
    [Description]     VARCHAR (256) NULL,
    [DeleteFlag]      BIT           CONSTRAINT [DF_OrgFunction_DeleteFlag] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OrgFunction] PRIMARY KEY CLUSTERED ([OrgFunctionId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCIDX_OrgFunction_UniqueConstraint]
    ON [dbo].[OrgFunction]([OrgFunctionName] ASC, [DeleteFlag] ASC) WITH (FILLFACTOR = 90);

