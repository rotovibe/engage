SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_CohortPatientView](
	[CohortPatientViewId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
 CONSTRAINT [PK_CohortPatientView] PRIMARY KEY CLUSTERED 
(
	[CohortPatientViewId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_CohortPatientView]  WITH CHECK ADD  CONSTRAINT [FK_CohortPatientView_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_CohortPatientView] CHECK CONSTRAINT [FK_CohortPatientView_Patient]
GO
