IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_TaskStatistics')
	DROP TABLE [dbo].[RPT_Flat_TaskStatistics]
GO

CREATE TABLE [dbo].[RPT_Flat_TaskStatistics](
		MongoTaskId [varchar](50) NOT NULL,
		MongoPatientId [varchar](50) NULL,
		MongoGoalId [varchar](50) NULL,
		CreatedOn DateTime NULL,
		CreatedBy [varchar](100) NULL,
		UpdatedOn DateTime NULL,
		UpdatedBy [varchar](100) NULL,
		ClosedDate [datetime] NULL,
		[Status] [varchar](50) NULL,
		StartDate [datetime] NULL,
		TargetDate [datetime] NULL,
		TargetValue [varchar](50) NULL,
		[Description] [varchar](max) NULL,
		Details [varchar](max) NULL,
		TemplateId [varchar](50) NULL,
		PrimaryCareManagerMongoId [varchar](50) NULL,
		PrimaryCareManagerPreferredName [varchar](100) NULL,
		TaskBarriers [varchar](max) NULL
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_TaskStatistics]') AND name = N'CIDX_RPT_Flat_TaskStatistics_MongoPatientId_MongoGoalId')
DROP INDEX [CIDX_RPT_Flat_TaskStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_TaskStatistics] WITH ( ONLINE = OFF )
GO

CREATE CLUSTERED INDEX [CIDX_RPT_Flat_TaskStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_TaskStatistics] 
(
	MongoPatientId ASC,
	MongoGoalId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_TaskStatistics';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_TaskStatistics', 'false');
GO