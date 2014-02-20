CREATE TABLE [dbo].[AuditError] (
    [AuditErrorId]        INT              IDENTITY (1, 1) NOT NULL,
    [AuditTypeId]         INT              NOT NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    [ImpersonatingUserId] UNIQUEIDENTIFIER NULL,
    [DateTimeStamp]       DATETIME         NOT NULL,
    [SourcePage]          VARCHAR (50)     NOT NULL,
    [SourceIp]            VARCHAR (15)     NOT NULL,
    [Browser]             VARCHAR (50)     NOT NULL,
    [SessionId]           VARCHAR (100)    NOT NULL,
    [ContractId]          INT              NULL,
    [ErrorId]             VARCHAR (20)     NULL,
    [ErrorText]           VARCHAR (MAX)    NOT NULL,
    [HelpLink]            VARCHAR (50)     NULL,
    [Source]              VARCHAR (50)     NULL,
    [StackTrace]          VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_AuditError] PRIMARY KEY CLUSTERED ([AuditErrorId] ASC),
    CONSTRAINT [FK_AuditError_AuditTypeId] FOREIGN KEY ([AuditTypeId]) REFERENCES [dbo].[AuditType] ([AuditTypeId])
);

