CREATE TABLE [dbo].[AuditAction] (
    [AuditActionId]       INT              IDENTITY (1, 1) NOT NULL,
    [AuditTypeId]         INT              NOT NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    [ImpersonatingUserId] UNIQUEIDENTIFIER NULL,
    [DateTimeStamp]       DATETIME         NOT NULL,
    [SourcePage]          VARCHAR (50)     NOT NULL,
    [SourceIp]            VARCHAR (15)     NOT NULL,
    [Browser]             VARCHAR (50)     NOT NULL,
    [SessionId]           VARCHAR (100)    NOT NULL,
    [ContractId]          INT              NULL,
    [EditedUserId]        UNIQUEIDENTIFIER NULL,
    [EnteredUserName]     VARCHAR (50)     NULL,
    [SearchText]          VARCHAR (100)    NULL,
    [LandingPage]         VARCHAR (50)     NULL,
    [TOSVersion]          INT              NULL,
    [NotificationTotal]   INT              NULL,
    [DownloadedReport]    VARCHAR (100)    NULL,
    CONSTRAINT [PK_AuditAction] PRIMARY KEY CLUSTERED ([AuditActionId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_AuditAction_AuditTypeId] FOREIGN KEY ([AuditTypeId]) REFERENCES [dbo].[AuditType] ([AuditTypeId])
);

