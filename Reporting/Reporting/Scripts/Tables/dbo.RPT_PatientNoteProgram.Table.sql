SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientNoteProgram](
	[PatientNoteProgramId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientNoteId] [varchar](50) NOT NULL,
	[PatientNoteId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientNoteProgram] PRIMARY KEY CLUSTERED 
(
	[PatientNoteProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_PatientNote] FOREIGN KEY([PatientNoteId])
REFERENCES [RPT_PatientNote] ([PatientNoteId])
GO
ALTER TABLE [RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_PatientNote]
GO
ALTER TABLE [RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy]
GO
