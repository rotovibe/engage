SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientObservation](
	[PatientObservationId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoObservationId] [varchar](50) NOT NULL,
	[ObservationId] [int] NULL,
	[Display] [varchar](50) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[NumericValue] [float] NULL,
	[NonNumericValue] [varchar](50) NULL,
	[Source] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Units] [varchar](50) NULL,
	[AdministeredBy] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime2](7) NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime2](7) NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime2](7) NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_PatientObservation] PRIMARY KEY CLUSTERED 
(
	[PatientObservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientObservation]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientObservation_Observation] FOREIGN KEY([ObservationId])
REFERENCES [RPT_Observation] ([ObservationId])
GO
ALTER TABLE [RPT_PatientObservation] CHECK CONSTRAINT [FK_PatientObservation_Observation]
GO
ALTER TABLE [RPT_PatientObservation]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientObservation_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientObservation] CHECK CONSTRAINT [FK_PatientObservation_Patient]
GO
ALTER TABLE [RPT_PatientObservation]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientObservation_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientObservation] CHECK CONSTRAINT [FK_PatientObservation_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientObservation]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientObservation_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientObservation] CHECK CONSTRAINT [FK_PatientObservation_UserMongoUpdatedBy]
GO
