SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientMedSuppPhClass](
	[PhCId] [int] IDENTITY(1,1) NOT NULL,
	[PatientMedSuppId] [int] NOT NULL,
	[MongoPatientMedSuppId] [varchar](50) NOT NULL,
	[PharmClass] [varchar](2000) NOT NULL,
 CONSTRAINT [PK_RPT_PatientMedSuppPhClass] PRIMARY KEY CLUSTERED 
(
	[PhCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
