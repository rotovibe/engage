CREATE TABLE [dbo].[SecurityQuestion] (
    [SecurityQuestionId] INT           IDENTITY (1, 1) NOT NULL,
    [Question]           VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_SecurityQuestion] PRIMARY KEY CLUSTERED ([SecurityQuestionId] ASC)
);

