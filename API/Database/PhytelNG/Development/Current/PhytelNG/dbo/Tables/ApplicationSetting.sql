CREATE TABLE [dbo].[ApplicationSetting] (
    [Key]   VARCHAR (50)  NOT NULL,
    [Value] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ApplicationSetting] PRIMARY KEY CLUSTERED ([Key] ASC) WITH (FILLFACTOR = 80)
);

