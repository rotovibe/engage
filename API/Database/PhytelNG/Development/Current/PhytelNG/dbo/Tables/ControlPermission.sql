CREATE TABLE [dbo].[ControlPermission] (
    [ControlId]    INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT [PK_ControlPermission] PRIMARY KEY CLUSTERED ([ControlId] ASC, [PermissionId] ASC),
    CONSTRAINT [FK_ControlPermission_Control] FOREIGN KEY ([ControlId]) REFERENCES [dbo].[Control] ([ControlId]),
    CONSTRAINT [FK_ControlPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([PermissionId])
);

