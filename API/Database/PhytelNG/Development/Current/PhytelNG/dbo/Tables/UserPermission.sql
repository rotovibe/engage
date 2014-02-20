CREATE TABLE [dbo].[UserPermission] (
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [PermissionId] INT              NOT NULL,
    [Include]      BIT              CONSTRAINT [DF__UserPermi__Inclu__4316F928] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED ([UserId] ASC, [PermissionId] ASC),
    CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([PermissionId]),
    CONSTRAINT [FK_UserPermission_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

