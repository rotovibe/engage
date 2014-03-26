CREATE TABLE [dbo].[APIUserToken] (
    [APIUserTokenID] INT              IDENTITY (1, 1) NOT NULL,
    [UserID]         UNIQUEIDENTIFIER NOT NULL,
    [Token]          VARCHAR (100)    NOT NULL,
    [CreatedOn]      DATETIME         CONSTRAINT [DF_APIUserToken_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_APIUserToken] PRIMARY KEY CLUSTERED ([APIUserTokenID] ASC),
    CONSTRAINT [FK_APIUserToken_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserId])
);

