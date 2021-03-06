SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProblem](
	[PatientProblemId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoProblemLookUpId] [varchar](50) NOT NULL,
	[ProblemId] [int] NOT NULL,
	[Active] [varchar](50) NULL,
	[Featured] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Level] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_PatientProblem] PRIMARY KEY CLUSTERED 
(
	[PatientProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProblem]  WITH CHECK ADD  CONSTRAINT [FK_PatientProblem_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientProblem] CHECK CONSTRAINT [FK_PatientProblem_Patient]
GO
ALTER TABLE [RPT_PatientProblem]  WITH CHECK ADD  CONSTRAINT [FK_PatientProblem_ProblemLookUp] FOREIGN KEY([ProblemId])
REFERENCES [RPT_ProblemLookUp] ([ProblemId])
GO
ALTER TABLE [RPT_PatientProblem] CHECK CONSTRAINT [FK_PatientProblem_ProblemLookUp]
GO
ALTER TABLE [RPT_PatientProblem]  WITH CHECK ADD  CONSTRAINT [FK_PatientProblem_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientProblem] CHECK CONSTRAINT [FK_PatientProblem_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientProblem]  WITH CHECK ADD  CONSTRAINT [FK_PatientProblem_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientProblem] CHECK CONSTRAINT [FK_PatientProblem_UserMongoUpdatedBy]
GO
