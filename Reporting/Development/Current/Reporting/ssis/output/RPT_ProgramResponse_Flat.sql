/****** Object:  Table [dbo].[RPT_ProgramResponse_Flat]    Script Date: 05/04/2015 12:02:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_ProgramResponse_Flat]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND type in (N'U'))
BEGIN
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
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_ActionSourceID')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_ActionSourceID] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[ActionSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_Composite')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_Composite] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[ProgramSourceId] ASC,
	[StepSourceId] ASC,
	[ActionSourceId] ASC,
	[ActionArchived] ASC,
	[ActionCompleted] ASC,
	[PatientId] ASC,
	[PatientProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_PatientID')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_PatientID] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_ProgramSourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_ProgramSourceId] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[ProgramSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_StepSourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_StepSourceId] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[StepSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
