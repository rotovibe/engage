CREATE TABLE [dbo].[Application] (
    [ApplicationName]        NVARCHAR (256)   NOT NULL,
    [LoweredApplicationName] NVARCHAR (256)   NOT NULL,
    [ApplicationId]          UNIQUEIDENTIFIER CONSTRAINT [DF__Applicati__Appli__5070F446] DEFAULT (newid()) NOT NULL,
    [Description]            NVARCHAR (256)   NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY NONCLUSTERED ([ApplicationId] ASC),
    CONSTRAINT [UDX_Application_ApplicationName] UNIQUE NONCLUSTERED ([ApplicationName] ASC),
    CONSTRAINT [UDX_Application_LoweredApplicationName] UNIQUE NONCLUSTERED ([LoweredApplicationName] ASC)
);


GO
CREATE CLUSTERED INDEX [IDX_Application_LoweredApplicationName]
    ON [dbo].[Application]([LoweredApplicationName] ASC);

