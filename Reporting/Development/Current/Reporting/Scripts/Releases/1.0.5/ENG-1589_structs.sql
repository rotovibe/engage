IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_GoalStatistics')
	DROP TABLE [dbo].[RPT_Flat_GoalStatistics]
GO

CREATE TABLE [dbo].[RPT_Flat_GoalStatistics](
	MongoGoalId [varchar](50) NOT NULL,
	MongoPatientId [varchar](50) NULL,
	Confidence [varchar](50) NULL,
	Importance [varchar](50) NULL,
	StageofChange [varchar](50) NULL,
	CreatedOn DateTime NULL,
	CreatedBy [varchar](100) NULL,
	UpdatedOn DateTime NULL,
	UpdatedBy [varchar](100) NULL,
	Name [varchar](500) NULL,
	Details [varchar](max) NULL,
	TemplateId [varchar](50) NULL,
	[Source] [varchar](max) NULL,
	TargetDate DateTime NULL,
	TargetValue [varchar](300) NULL,
	[Status] [varchar](50) NULL,
	StartDate DateTime NULL,
	EndDate DateTime NULL,
	FocusAreas [varchar](max) NULL,
	[Type] [varchar](50) NULL,
	PrimaryCareManagerMongoId [varchar](50) NULL,
	PrimaryCareManagerPreferredName [varchar](100) NULL,
  CONSTRAINT [PK_MongoId] PRIMARY KEY CLUSTERED 
	(
		[MongoGoalId] ASC
	)
) ON [PRIMARY]
GO

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_GoalStatistics';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_GoalStatistics', 'false');
GO