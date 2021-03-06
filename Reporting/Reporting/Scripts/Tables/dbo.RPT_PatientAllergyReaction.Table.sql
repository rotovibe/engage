SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientAllergyReaction](
	[AllergyReactionId] [int] IDENTITY(1,1) NOT NULL,
	[PatientAllergyId] [int] NOT NULL,
	[MongoPatientAllergyId] [varchar](50) NULL,
	[MongoReactionId] [varchar](50) NOT NULL,
	[ReactionId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_RPT_PatientAllergyReaction] PRIMARY KEY CLUSTERED 
(
	[AllergyReactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
