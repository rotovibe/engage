CREATE TABLE [dbo].[StatusType] (
    [StatusTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (100) NOT NULL,
    [Description]  VARCHAR (500) NULL,
    CONSTRAINT [PK_StatusType] PRIMARY KEY CLUSTERED ([StatusTypeId] ASC)
);

