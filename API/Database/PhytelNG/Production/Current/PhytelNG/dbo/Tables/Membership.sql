CREATE TABLE [dbo].[Membership] (
    [ApplicationId]                          UNIQUEIDENTIFIER NOT NULL,
    [UserId]                                 UNIQUEIDENTIFIER NOT NULL,
    [Password]                               NVARCHAR (128)   NOT NULL,
    [PasswordFormat]                         INT              CONSTRAINT [DF__Membershi__Passw__4222D4EF] DEFAULT ((0)) NOT NULL,
    [PasswordSalt]                           NVARCHAR (128)   NOT NULL,
    [MobilePIN]                              NVARCHAR (16)    NULL,
    [Email]                                  NVARCHAR (256)   NULL,
    [LoweredEmail]                           NVARCHAR (256)   NULL,
    [PasswordQuestion]                       NVARCHAR (256)   NULL,
    [PasswordAnswer]                         NVARCHAR (128)   NULL,
    [IsApproved]                             BIT              NOT NULL,
    [IsLockedOut]                            BIT              NOT NULL,
    [CreateDate]                             DATETIME         NOT NULL,
    [LastLoginDate]                          DATETIME         NOT NULL,
    [LastPasswordChangedDate]                DATETIME         NOT NULL,
    [LastLockoutDate]                        DATETIME         NOT NULL,
    [FailedPasswordAttemptCount]             INT              NOT NULL,
    [FailedPasswordAttemptWindowStart]       DATETIME         NOT NULL,
    [FailedPasswordAnswerAttemptCount]       INT              NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] DATETIME         NOT NULL,
    [Comment]                                NTEXT            NULL,
    [PasswordExpiration]                     DATETIME         NULL,
    [Status]                                 VARCHAR (10)     CONSTRAINT [DF_Membership_Status] DEFAULT ('Active') NOT NULL,
    [AdministratorUserId]                    UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Membership] PRIMARY KEY NONCLUSTERED ([UserId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [CK_Membership_Status] CHECK ([Status]='Active' OR [Status]='Inactive' OR [Status]='Deleted' OR [Status]='Locked'),
    CONSTRAINT [FK_Membership_Application_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_Membership_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);


GO
CREATE CLUSTERED INDEX [IDX_Membership_ApplicationID_LoweredEmail]
    ON [dbo].[Membership]([ApplicationId] ASC, [LoweredEmail] ASC) WITH (FILLFACTOR = 80);

