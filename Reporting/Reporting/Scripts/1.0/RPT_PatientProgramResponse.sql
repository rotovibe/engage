ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
DROP TABLE [dbo].[RPT_PatientProgramResponse]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RPT_PatientProgramResponse](
	[ResponseId] [int] IDENTITY(1,1) NOT NULL,
	[MongoStepId] [varchar](50) NULL,
	[StepId] [int] NULL,
	[MongoNextStepId] [varchar](50) NULL,
	[NextStepId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoActionId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Text] [varchar](max) NULL,
	[Value] [varchar](max) NULL,
	[Nominal] [varchar](50) NULL,
	[Required] [varchar](50) NULL,
	[Selected] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime2](7) NULL,
	[Delete] [varchar](50) NULL,
	[MongoStepSourceId] [varchar](50) NULL,
	[StepSourceId] [int] NULL,
	[ActionId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime2](7) NULL,
	[TTLDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStepResponse] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_1] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_2] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_State] ON [dbo].[RPT_PatientProgramResponse] 
(
	[Selected] ASC,
	[Delete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep] FOREIGN KEY([NextStepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep] FOREIGN KEY([StepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
