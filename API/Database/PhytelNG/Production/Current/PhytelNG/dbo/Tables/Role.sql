CREATE TABLE [dbo].[Role] (
    [ApplicationId]   UNIQUEIDENTIFIER NOT NULL,
    [RoleId]          UNIQUEIDENTIFIER CONSTRAINT [DF__Role__RoleId__440B1D61] DEFAULT (newid()) NOT NULL,
    [RoleName]        NVARCHAR (256)   NOT NULL,
    [LoweredRoleName] NVARCHAR (256)   NOT NULL,
    [Description]     NVARCHAR (256)   NULL,
    [UserTypeId]      INT              NULL,
    [ContractId]      INT              NULL,
    [Level]           INT              CONSTRAINT [DF_Role_Level] DEFAULT ((1)) NOT NULL,
    [RoleTypeId]      TINYINT          CONSTRAINT [DF_Role_RoleTypeId] DEFAULT ((4)) NOT NULL,
    [IsStandard]      BIT              CONSTRAINT [DF__Role__IsStandard__633A7B4E] DEFAULT ((0)) NOT NULL,
    [IsInternal]      BIT              CONSTRAINT [DF__Role__IsInternal__642E9F87] DEFAULT ((0)) NOT NULL,
    [StatusTypeId]    INT              CONSTRAINT [DF__Role__StatusType__5793BE78] DEFAULT ((1)) NOT NULL,
    [DeleteFlag]      BIT              CONSTRAINT [DF__Role__DeleteFlag__705F6C42] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY NONCLUSTERED ([RoleId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Role_Application_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_Role_RoleType] FOREIGN KEY ([RoleTypeId]) REFERENCES [dbo].[RoleType] ([RoleTypeId]),
    CONSTRAINT [FK_Role_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([UserTypeId])
);


GO
CREATE CLUSTERED INDEX [IDX_Role_ApplicationID_LoweredRoleName]
    ON [dbo].[Role]([ApplicationId] ASC, [LoweredRoleName] ASC) WITH (FILLFACTOR = 80);

