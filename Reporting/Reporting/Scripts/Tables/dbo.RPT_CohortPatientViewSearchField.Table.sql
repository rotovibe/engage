SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_CohortPatientViewSearchField](
	[SearchFieldId] [int] IDENTITY(1,1) NOT NULL,
	[CohortPatientViewId] [int] NOT NULL,
	[FieldName] [varchar](50) NULL,
	[Value] [varchar](max) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_CohortPatientViewSearchField] PRIMARY KEY CLUSTERED 
(
	[SearchFieldId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_CohortPatientViewSearchField]  WITH CHECK ADD  CONSTRAINT [FK_CohortPatientViewSearchField_CohortPatientView] FOREIGN KEY([CohortPatientViewId])
REFERENCES [RPT_CohortPatientView] ([CohortPatientViewId])
GO
ALTER TABLE [RPT_CohortPatientViewSearchField] CHECK CONSTRAINT [FK_CohortPatientViewSearchField_CohortPatientView]
GO
