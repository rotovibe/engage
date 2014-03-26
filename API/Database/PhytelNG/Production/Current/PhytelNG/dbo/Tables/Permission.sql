CREATE TABLE [dbo].[Permission] (
    [PermissionId]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (200)  NOT NULL,
    [Description]      VARCHAR (1000) NULL,
    [PermissionTypeId] INT            NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([PermissionId] ASC),
    CONSTRAINT [FK_Permission_PermissionType] FOREIGN KEY ([PermissionTypeId]) REFERENCES [dbo].[PermissionType] ([PermissionTypeId])
);

