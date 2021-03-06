SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientMedSupp](
	[PatientMedSuppId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[PatientId] [int] NOT NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoFamilyId] [varchar](50) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Category] [varchar](200) NOT NULL,
	[MongoTypeId] [varchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Status] [varchar](200) NOT NULL,
	[Dosage] [varchar](500) NULL,
	[Strength] [varchar](200) NULL,
	[Route] [varchar](200) NULL,
	[Form] [varchar](200) NULL,
	[FreqQuantity] [varchar](200) NULL,
	[MongoFreqHowOftenId] [varchar](50) NULL,
	[FreqHowOftenId] [int] NULL,
	[MongoFreqWhenId] [varchar](50) NULL,
	[FreqWhenId] [int] NULL,
	[MongoSourceId] [varchar](50) NOT NULL,
	[SourceId] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Reason] [varchar](5000) NULL,
	[Notes] [varchar](5000) NULL,
	[PrescribedBy] [varchar](500) NULL,
	[SystemName] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientMedSupp] PRIMARY KEY CLUSTERED 
(
	[PatientMedSuppId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
