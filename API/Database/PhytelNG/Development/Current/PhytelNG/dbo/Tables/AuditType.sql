CREATE TABLE [dbo].[AuditType] (
    [AuditTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AuditType] PRIMARY KEY CLUSTERED ([AuditTypeId] ASC)
);

