CREATE TABLE [dbo].[ApplicationMessage_BKUp] (
    [Code]  VARCHAR (10)  NOT NULL,
    [Title] VARCHAR (200) NOT NULL,
    [Text]  VARCHAR (MAX) NOT NULL,
    [Audit] BIT           NOT NULL
);

