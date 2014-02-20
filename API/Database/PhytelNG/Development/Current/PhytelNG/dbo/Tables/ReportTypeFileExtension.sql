CREATE TABLE [dbo].[ReportTypeFileExtension] (
    [ReportTypeID]  INT         NOT NULL,
    [FileExtension] VARCHAR (5) NOT NULL,
    CONSTRAINT [PK_ReportFileExtensions] PRIMARY KEY CLUSTERED ([ReportTypeID] ASC, [FileExtension] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ReportFileExtensions_ReportType] FOREIGN KEY ([ReportTypeID]) REFERENCES [dbo].[ReportType] ([ReportTypeID])
);

