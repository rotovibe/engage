CREATE TABLE [dbo].[AuditView] (
    [AuditViewId]         INT              IDENTITY (1, 1) NOT NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    [ImpersonatingUserId] UNIQUEIDENTIFIER NULL,
    [DateTimeStamp]       DATETIME         NOT NULL,
    [SourcePage]          VARCHAR (50)     NOT NULL,
    [SourceIp]            VARCHAR (15)     NOT NULL,
    [Browser]             VARCHAR (50)     NOT NULL,
    [SessionId]           VARCHAR (100)    NOT NULL,
    [ContractId]          INT              NULL,
    CONSTRAINT [PK_AuditView] PRIMARY KEY CLUSTERED ([AuditViewId] ASC) WITH (FILLFACTOR = 80)
);

