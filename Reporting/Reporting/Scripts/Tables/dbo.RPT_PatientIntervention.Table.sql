SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientIntervention](
	[PatientInterventionId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientGoalId] [varchar](50) NULL,
	[PatientGoalId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoCategoryLookUpId] [varchar](50) NULL,
	[CategoryLookUpId] [int] NULL,
	[MongoContactUserId] [varchar](50) NULL,
	[AssignedToUserId] [int] NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[StatusDate] [datetime] NULL,
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
	[ClosedDate] [datetime] NULL,
	[TemplateId] [varchar](50) NULL,
 CONSTRAINT [PK_PatientIntervention] PRIMARY KEY CLUSTERED 
(
	[PatientInterventionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientIntervention]  WITH CHECK ADD  CONSTRAINT [FK_PatientIntervention_InterventionCategoryLookUp] FOREIGN KEY([CategoryLookUpId])
REFERENCES [RPT_InterventionCategoryLookUp] ([InterventionCategoryId])
GO
ALTER TABLE [RPT_PatientIntervention] CHECK CONSTRAINT [FK_PatientIntervention_InterventionCategoryLookUp]
GO
ALTER TABLE [RPT_PatientIntervention]  WITH CHECK ADD  CONSTRAINT [FK_PatientIntervention_PatientGoal] FOREIGN KEY([PatientGoalId])
REFERENCES [RPT_PatientGoal] ([PatientGoalId])
GO
ALTER TABLE [RPT_PatientIntervention] CHECK CONSTRAINT [FK_PatientIntervention_PatientGoal]
GO
ALTER TABLE [RPT_PatientIntervention]  WITH CHECK ADD  CONSTRAINT [FK_PatientIntervention_User] FOREIGN KEY([AssignedToUserId])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientIntervention] CHECK CONSTRAINT [FK_PatientIntervention_User]
GO
ALTER TABLE [RPT_PatientIntervention]  WITH CHECK ADD  CONSTRAINT [FK_PatientIntervention_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientIntervention] CHECK CONSTRAINT [FK_PatientIntervention_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientIntervention]  WITH CHECK ADD  CONSTRAINT [FK_PatientIntervention_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientIntervention] CHECK CONSTRAINT [FK_PatientIntervention_UserMongoUpdatedBy]
GO
