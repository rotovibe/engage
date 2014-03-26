CREATE TABLE [dbo].[User] (
    [ApplicationId]    UNIQUEIDENTIFIER NOT NULL,
    [UserId]           UNIQUEIDENTIFIER CONSTRAINT [DF__User__UserId__03F0984C] DEFAULT (newid()) NOT NULL,
    [UserName]         NVARCHAR (256)   NOT NULL,
    [LoweredUserName]  NVARCHAR (256)   NOT NULL,
    [UserTypeId]       INT              CONSTRAINT [DF__User__UserTypeId__2C538F61] DEFAULT ((2)) NOT NULL,
    [MobileAlias]      NVARCHAR (16)    CONSTRAINT [DF__User__MobileAlia__04E4BC85] DEFAULT (NULL) NULL,
    [IsAnonymous]      BIT              CONSTRAINT [DF__User__IsAnonymou__05D8E0BE] DEFAULT ((0)) NOT NULL,
    [LastActivityDate] DATETIME         NOT NULL,
    [NewUser]          BIT              CONSTRAINT [DF__User__NewUser__54CB950F] DEFAULT ((1)) NOT NULL,
    [DisplayName]      VARCHAR (100)    NULL,
    [FirstName]        VARCHAR (100)    NULL,
    [MiddleName]       VARCHAR (100)    NULL,
    [LastName]         VARCHAR (100)    NULL,
    [Phone]            VARCHAR (10)     CONSTRAINT [DF_User_Phone] DEFAULT ('0000000000') NOT NULL,
    [Ext]              VARCHAR (5)      NULL,
    [SessionTimeout]   SMALLINT         NULL,
    [StatusTypeId]     INT              CONSTRAINT [DF__User__StatusType__4E88ABD4] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_User] PRIMARY KEY NONCLUSTERED ([UserId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_User_Application_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_User_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([UserTypeId])
);


GO
CREATE UNIQUE CLUSTERED INDEX [IDX_User_ApplicationID_LoweredUserName]
    ON [dbo].[User]([ApplicationId] ASC, [LoweredUserName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_User_ApplicationID_LastActivityDate]
    ON [dbo].[User]([ApplicationId] ASC, [LastActivityDate] ASC) WITH (FILLFACTOR = 80);

