SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_Goals_Percent_Summary](
	[Date] [date] NOT NULL,
	[Met] [decimal](18, 0) NOT NULL,
	[Just_Goals] [decimal](18, 0) NOT NULL,
	[Interventions_Tasks] [decimal](18, 0) NOT NULL,
	[No_Goals] [decimal](18, 0) NOT NULL
) ON [PRIMARY]
GO
