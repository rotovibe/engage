/****** Object:  Table [dbo].[RPT_ProgramResponse_Flat]    Script Date: 05/08/2015 14:17:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_ProgramResponse_Flat]
GO

/****** Object:  Table [dbo].[RPT_ProgramResponse_Flat]    Script Date: 05/08/2015 14:17:38 ******/
CREATE TABLE [dbo].[RPT_ProgramResponse_Flat](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[ProgramSourceId] [varchar](50) NULL,
	[ActionSourceId] [varchar](50) NULL,
	[ActionArchived] [varchar](50) NULL,
	[ActionArchivedDate] [datetime] NULL,
	[StepSourceId] [varchar](50) NULL,
	[Text] [varchar](max) NULL,
	[Value] [varchar](max) NULL,
	[Selected] [varchar](50) NULL,
	[Delete] [varchar](50) NULL,
	[ActionCompleted] [varchar](50) NULL,
	[StepCompleted] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[ActionName] [varchar](200) NULL,
	[Question] [varchar](max) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


