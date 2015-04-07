SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ProgramResponse_Flat](
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
	[DateCompleted] [datetime] NULL
) ON [PRIMARY]
GO
