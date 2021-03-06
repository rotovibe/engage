SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientInterventionBarrier](
	[PatientInterventionBarrierId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientInterventionId] [varchar](50) NULL,
	[PatientInterventionId] [int] NULL,
	[MongoPatientBarrierId] [varchar](50) NOT NULL,
	[PatientBarrierId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientInterventionBarrier] PRIMARY KEY CLUSTERED 
(
	[PatientInterventionBarrierId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientInterventionBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientInterventionBarrier_PatientBarrier] FOREIGN KEY([PatientBarrierId])
REFERENCES [RPT_PatientBarrier] ([PatientBarrierId])
GO
ALTER TABLE [RPT_PatientInterventionBarrier] CHECK CONSTRAINT [FK_PatientInterventionBarrier_PatientBarrier]
GO
ALTER TABLE [RPT_PatientInterventionBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientInterventionBarrier_PatientIntervention] FOREIGN KEY([PatientInterventionId])
REFERENCES [RPT_PatientIntervention] ([PatientInterventionId])
GO
ALTER TABLE [RPT_PatientInterventionBarrier] CHECK CONSTRAINT [FK_PatientInterventionBarrier_PatientIntervention]
GO
ALTER TABLE [RPT_PatientInterventionBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientInterventionBarrier_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientInterventionBarrier] CHECK CONSTRAINT [FK_PatientInterventionBarrier_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientInterventionBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientInterventionBarrier_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientInterventionBarrier] CHECK CONSTRAINT [FK_PatientInterventionBarrier_UserMongoUpdatedBy]
GO
