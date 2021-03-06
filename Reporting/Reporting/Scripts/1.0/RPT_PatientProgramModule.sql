ALTER TABLE [dbo].[RPT_PatientProgramModule] DROP CONSTRAINT [FK_PatientProgramModule_PatientProgram]
GO
ALTER TABLE [dbo].[RPT_PatientProgramModule] DROP CONSTRAINT [FK_PatientProgramModule_PatientProgram]
GO
DROP TABLE [dbo].[RPT_PatientProgramModule]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RPT_PatientProgramModule](
	[PatientProgramModuleId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoProgramId] [varchar](50) NOT NULL,
	[AttributeStartDate] [datetime] NULL,
	[AttributeEndDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[AssignedOn] [datetime] NULL,
	[MongoAssignedBy] [varchar](50) NULL,
	[AssignedBy] [int] NULL,
	[MongoAssignedTo] [varchar](50) NULL,
	[AssignedTo] [int] NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[Enabled] [varchar](50) NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[LastupdatedOn] [datetime] NULL,
	[RecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModule] PRIMARY KEY CLUSTERED 
(
	[PatientProgramModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramModule_SourceId] ON [dbo].[RPT_PatientProgramModule] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RPT_PatientProgramModule]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModule_PatientProgram] FOREIGN KEY([PatientProgramId])
REFERENCES [dbo].[RPT_PatientProgram] ([PatientProgramId])
GO
ALTER TABLE [dbo].[RPT_PatientProgramModule] CHECK CONSTRAINT [FK_PatientProgramModule_PatientProgram]
GO
