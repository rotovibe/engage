CREATE TABLE [dbo].[UserSession] (
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [SessionId]        NVARCHAR (88)    NOT NULL,
    [LastActivityDate] DATETIME         NOT NULL,
    [ExpirationDate]   DATETIME         NOT NULL,
    CONSTRAINT [FK_UserSession_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_UserSession_UserId_SessionId]
    ON [dbo].[UserSession]([UserId] ASC, [SessionId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_UserSession_ExpirationDate]
    ON [dbo].[UserSession]([UserId] ASC, [SessionId] ASC, [ExpirationDate] ASC) WITH (FILLFACTOR = 80);

