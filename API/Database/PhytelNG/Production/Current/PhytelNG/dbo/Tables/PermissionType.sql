CREATE TABLE [dbo].[PermissionType] (
    [PermissionTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (100) NOT NULL,
    [Description]      VARCHAR (500) NULL,
    CONSTRAINT [PK_PermissionType] PRIMARY KEY CLUSTERED ([PermissionTypeId] ASC)
);

