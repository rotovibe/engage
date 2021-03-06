SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_Observation](
	[ObservationId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[ObservationTypeId] [int] NOT NULL,
	[MongoObservationLookUpId] [varchar](50) NULL,
	[Code] [varchar](100) NULL,
	[CodingSystemId] [varchar](50) NULL,
	[CommonName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[HighValue] [int] NULL,
	[LowValue] [int] NULL,
	[Order] [int] NULL,
	[Units] [varchar](50) NULL,
	[Standard] [varchar](50) NULL,
	[Source] [varchar](100) NULL,
	[Status] [varchar](50) NULL,
	[GroupId] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_Observation] PRIMARY KEY CLUSTERED 
(
	[ObservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_Observation]  WITH CHECK ADD  CONSTRAINT [FK_Observation_ObservationTypeLookUp] FOREIGN KEY([ObservationTypeId])
REFERENCES [RPT_ObservationTypeLookUp] ([ObservationTypeId])
GO
ALTER TABLE [RPT_Observation] CHECK CONSTRAINT [FK_Observation_ObservationTypeLookUp]
GO
ALTER TABLE [RPT_Observation]  WITH CHECK ADD  CONSTRAINT [FK_Observation_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_Observation] CHECK CONSTRAINT [FK_Observation_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_Observation]  WITH CHECK ADD  CONSTRAINT [FK_Observation_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_Observation] CHECK CONSTRAINT [FK_Observation_UserMongoUpdatedBy]
GO
