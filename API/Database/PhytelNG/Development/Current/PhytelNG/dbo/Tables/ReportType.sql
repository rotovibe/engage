CREATE TABLE [dbo].[ReportType] (
    [ReportTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [ReportTypeName] VARCHAR (50)  NOT NULL,
    [Description]    VARCHAR (100) NULL,
    [DeleteFlag]     BIT           CONSTRAINT [DF_ReportType_DeleteFlag] DEFAULT ((0)) NOT NULL,
    [CreatedBy]      INT           NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_ReportType_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      INT           NULL,
    [UpdateDate]     DATETIME      NULL,
    CONSTRAINT [PK_ReportType] PRIMARY KEY CLUSTERED ([ReportTypeID] ASC) WITH (FILLFACTOR = 90)
);

