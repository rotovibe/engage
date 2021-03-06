SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_CareMember](
	[CareMemberId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[MongoUserId] [varchar](50) NULL,
	[TypeId] [int] NOT NULL,
	[MongoCommTypeLookUpId] [varchar](50) NULL,
	[Primary] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[TTLDate] [datetime] NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_CareMember] PRIMARY KEY CLUSTERED 
(
	[CareMemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_CareMember]  WITH CHECK ADD  CONSTRAINT [FK_CareMember_CareMemberTypeLookUp] FOREIGN KEY([TypeId])
REFERENCES [RPT_CareMemberTypeLookUp] ([CareMemberTypeId])
GO
ALTER TABLE [RPT_CareMember] CHECK CONSTRAINT [FK_CareMember_CareMemberTypeLookUp]
GO
ALTER TABLE [RPT_CareMember]  WITH CHECK ADD  CONSTRAINT [FK_CareMember_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_CareMember] CHECK CONSTRAINT [FK_CareMember_Patient]
GO
ALTER TABLE [RPT_CareMember]  WITH CHECK ADD  CONSTRAINT [FK_CareMember_User] FOREIGN KEY([UserId])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_CareMember] CHECK CONSTRAINT [FK_CareMember_User]
GO
ALTER TABLE [RPT_CareMember]  WITH CHECK ADD  CONSTRAINT [FK_CareMember_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_CareMember] CHECK CONSTRAINT [FK_CareMember_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_CareMember]  WITH CHECK ADD  CONSTRAINT [FK_CareMember_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_CareMember] CHECK CONSTRAINT [FK_CareMember_UserMongoUpdatedBy]
GO
