/****** Object:  Table [dbo].[RPT_ProcessAudit]    Script Date: 05/12/2015 09:56:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RPT_ProcessAudit]') AND type in (N'U'))
	DROP TABLE [RPT_ProcessAudit]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/11/2015 10:11:29 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P'))
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	EXECUTE [spPhy_RPT_Flat_Engage];
END

/**** ADDING RECOMMENDED NON-CLUSTERED INDEXES *****/
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = 'IX_RPT_PatientProgramResponse_StepidSelected')
	DROP INDEX IX_RPT_PatientProgramResponse_StepidSelected ON RPT_PatientProgramResponse;
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]') AND name = 'IX_RPT_PatientProgramModule_MongoId')
	DROP INDEX IX_RPT_PatientProgramModule_MongoId ON RPT_PatientProgramModule;
GO


IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = 'IX_RPT_ProgramResponse_Flat_PatientId_PatientProgramId_ActionSourceId')
	DROP INDEX [IX_RPT_ProgramResponse_Flat_PatientId_PatientProgramId_ActionSourceId] ON [RPT_ProgramResponse_Flat];
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = 'IX_RPT_PatientProgramStep_SourceId')
	DROP INDEX [IX_RPT_PatientProgramStep_SourceId] ON [RPT_PatientProgramStep];
GO