SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientTaskBarrier](
	[PatientTaskBarrierId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientTaskId] [varchar](50) NULL,
	[PatientTaskId] [int] NOT NULL,
	[MongoPatientBarrierId] [varchar](50) NOT NULL,
	[PatientBarrierId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientTaskBarrier] PRIMARY KEY CLUSTERED 
(
	[PatientTaskBarrierId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientTaskBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskBarrier_PatientBarrier] FOREIGN KEY([PatientBarrierId])
REFERENCES [RPT_PatientBarrier] ([PatientBarrierId])
GO
ALTER TABLE [RPT_PatientTaskBarrier] CHECK CONSTRAINT [FK_PatientTaskBarrier_PatientBarrier]
GO
ALTER TABLE [RPT_PatientTaskBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskBarrier_PatientTask] FOREIGN KEY([PatientTaskId])
REFERENCES [RPT_PatientTask] ([PatientTaskId])
GO
ALTER TABLE [RPT_PatientTaskBarrier] CHECK CONSTRAINT [FK_PatientTaskBarrier_PatientTask]
GO
ALTER TABLE [RPT_PatientTaskBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskBarrier_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTaskBarrier] CHECK CONSTRAINT [FK_PatientTaskBarrier_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientTaskBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskBarrier_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTaskBarrier] CHECK CONSTRAINT [FK_PatientTaskBarrier_UserMongoUpdatedBy]
GO
