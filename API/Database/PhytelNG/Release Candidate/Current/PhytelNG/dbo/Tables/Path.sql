CREATE TABLE [dbo].[Path] (
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [PathId]        UNIQUEIDENTIFIER CONSTRAINT [DF__Path__PathId__46E78A0C] DEFAULT (newid()) NOT NULL,
    [Path]          NVARCHAR (256)   NOT NULL,
    [LoweredPath]   NVARCHAR (256)   NOT NULL,
    CONSTRAINT [PK_Path] PRIMARY KEY NONCLUSTERED ([PathId] ASC),
    CONSTRAINT [FK_Path_Application_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId])
);


GO
CREATE UNIQUE CLUSTERED INDEX [IDX_Path_ApplicationID_LoweredPath]
    ON [dbo].[Path]([ApplicationId] ASC, [LoweredPath] ASC);

