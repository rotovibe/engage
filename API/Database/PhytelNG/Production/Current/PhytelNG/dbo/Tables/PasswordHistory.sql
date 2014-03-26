CREATE TABLE [dbo].[PasswordHistory] (
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    [Password]            NVARCHAR (128)   NOT NULL,
    [PasswordChangedDate] DATETIME         NOT NULL,
    CONSTRAINT [PK_PasswordHistory] PRIMARY KEY CLUSTERED ([UserId] ASC, [Password] ASC, [PasswordChangedDate] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PasswordHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

