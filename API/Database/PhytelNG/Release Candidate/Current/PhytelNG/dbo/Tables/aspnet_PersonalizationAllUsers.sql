CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers] (
    [PathId]          UNIQUEIDENTIFIER NOT NULL,
    [PageSettings]    IMAGE            NOT NULL,
    [LastUpdatedDate] DATETIME         NOT NULL,
    CONSTRAINT [PK__aspnet_PerAllUsers] PRIMARY KEY CLUSTERED ([PathId] ASC),
    CONSTRAINT [FK__aspnet_PerAllUsers_Path] FOREIGN KEY ([PathId]) REFERENCES [dbo].[Path] ([PathId])
);

