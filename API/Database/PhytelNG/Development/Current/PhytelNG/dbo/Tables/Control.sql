CREATE TABLE [dbo].[Control] (
    [ControlId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (200)  NOT NULL,
    [Path]        VARCHAR (500)  NOT NULL,
    [Description] VARCHAR (1000) NULL,
    [Path_13]     VARCHAR (500)  NULL,
    CONSTRAINT [PK_Control] PRIMARY KEY CLUSTERED ([ControlId] ASC) WITH (FILLFACTOR = 80)
);

