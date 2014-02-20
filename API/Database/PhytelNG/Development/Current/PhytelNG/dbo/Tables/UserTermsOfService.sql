CREATE TABLE [dbo].[UserTermsOfService] (
    [TermsOfServiceID] INT              NOT NULL,
    [UserID]           UNIQUEIDENTIFIER NOT NULL,
    [AgreedOn]         DATETIME         NOT NULL,
    CONSTRAINT [PK_UserTermsOfService] PRIMARY KEY CLUSTERED ([TermsOfServiceID] ASC, [UserID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_UserTermsOfService_TermsOfService] FOREIGN KEY ([TermsOfServiceID]) REFERENCES [dbo].[TermsOfService] ([TermsOfServiceID]),
    CONSTRAINT [FK_UserTermsOfService_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserId])
);

