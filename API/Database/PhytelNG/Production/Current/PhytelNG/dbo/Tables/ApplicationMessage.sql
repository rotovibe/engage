CREATE TABLE [dbo].[ApplicationMessage] (
    [Code]  VARCHAR (10)  NOT NULL,
    [Title] VARCHAR (200) NOT NULL,
    [Text]  VARCHAR (MAX) NOT NULL,
    [Audit] BIT           CONSTRAINT [DF_ApplicationMessage_Audit] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ApplicationMessage] PRIMARY KEY CLUSTERED ([Code] ASC) WITH (FILLFACTOR = 80)
);

