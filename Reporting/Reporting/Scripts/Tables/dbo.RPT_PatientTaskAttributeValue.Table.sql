SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientTaskAttributeValue](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[PatientTaskAttributeId] [int] NOT NULL,
	[Value] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientTaskAttributeValue] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_PatientTaskAttribute] FOREIGN KEY([PatientTaskAttributeId])
REFERENCES [RPT_PatientTaskAttribute] ([PatientTaskAttributeId])
GO
ALTER TABLE [RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_PatientTaskAttribute]
GO
ALTER TABLE [RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoUpdatedBy]
GO
