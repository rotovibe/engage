SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientNote](
	[PatientNoteId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Text] [varchar](max) NULL,
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
	[MongoMethodId] [varchar](50) NULL,
	[MethodId] [int] NULL,
	[Type] [varchar](50) NULL,
	[MongoOutcomeId] [varchar](50) NULL,
	[OutcomeId] [int] NULL,
	[MongoWhoId] [varchar](50) NULL,
	[WhoId] [int] NULL,
	[MongoSourceId] [varchar](50) NULL,
	[SourceId] [int] NULL,
	[MongoDurationId] [varchar](50) NULL,
	[DurationId] [int] NULL,
	[ContactedOn] [datetime] NULL,
	[ValidatedIntentity] [varchar](50) NULL,
 CONSTRAINT [PK_PatientNote] PRIMARY KEY CLUSTERED 
(
	[PatientNoteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientNote]  WITH CHECK ADD  CONSTRAINT [FK_PatientNote_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientNote] CHECK CONSTRAINT [FK_PatientNote_Patient]
GO
ALTER TABLE [RPT_PatientNote]  WITH CHECK ADD  CONSTRAINT [FK_PatientNote_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientNote] CHECK CONSTRAINT [FK_PatientNote_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientNote]  WITH CHECK ADD  CONSTRAINT [FK_PatientNote_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientNote] CHECK CONSTRAINT [FK_PatientNote_UserMongoUpdatedBy]
GO
