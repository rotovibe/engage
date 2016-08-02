/******** ENG-1025 **********/
/****** Object:  Table [dbo].[RPT_BSHSI_HW2_Enrollment_Info]    Script Date: 05/14/2015 13:27:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_BSHSI_HW2_Enrollment_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info]
GO

/****** Object:  Table [dbo].[RPT_BSHSI_HW2_Enrollment_Info]    Script Date: 05/14/2015 13:27:47 ******/
CREATE TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[Priority] [varchar](50) NULL,
	[firstName] [varchar](100) NULL,
	[SystemId] [varchar](50) NULL,
	[LastName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[LSSN] [int] NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[Program_CM] [varchar](100) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Assigned_Date] [datetime] NULL,
	[Last_State_Update_Date] [datetime] NULL,
	[State] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Program_Completed_Date] [date] NULL,
	[Re_enrollment_Date] [date] NULL,
	[Enrolled_Date] [date] NULL,
	[Pending_Enrolled_Date] [date] NULL,
	[Enrollment_Action_Completion_Date] [date] NULL,
	[Market] [varchar](max) NULL,
	[Disenroll_Date] [date] NULL,
	[Disenroll_Reason] [varchar](max) NULL,
	[did_not_enroll_date] [date] NULL,
	[did_not_enroll_reason] [varchar](max) NULL
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]    Script Date: 05/14/2015 13:52:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]    Script Date: 05/14/2015 13:52:12 ******/
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
AS
BEGIN
	DELETE [RPT_BSHSI_HW2_Enrollment_Info]
	INSERT INTO [RPT_BSHSI_HW2_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Pending_Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Market,
		Disenroll_Date,
		Disenroll_Reason,
		did_not_enroll_date,
		did_not_enroll_reason
	) 
	SELECT DISTINCT 	
		pt.PatientId
		,ppt.PatientProgramId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  	 	
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = '541943a6bdd4dfa5d90002da'
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fb39ac80d30e00000067')) as [Enrollment]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57383ac80d31203000033', '53f57309ac80d31200000017' )) as [Program_Completed_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f572caac80d31203000020', '53f571e0ac80d3120300001b')) as [Re_enrollment_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f920ac80d30e00000066') ) as [Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f7ecac80d30e02000072')) as [Pending_Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f920ac80d30e00000066')) as [Enrollment_Action_Completion_Date]
		,( select Market FROM dbo.fn_RPT_Market(pt.PatientId,ppt.PatientProgramId) ) as [Market]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56eb7ac80d31203000001')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56f10ac80d31200000001') ) as [Disenroll_Reason]	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fc71ac80d30e02000074') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f885ac80d30e00000065')) as [did_not_enroll_reason]
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = '541943a6bdd4dfa5d90002da'
		AND ppt.[Delete] = 'False'
END

GO


/******** ENG-1150 **********/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Observations_Dim]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Observations_Dim]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Observations_Dim]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Observations_Dim]
GO


/******** ENG-1215 **********/
/**** RPT_CareBridge_Enrollment_Info ****/
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician' AND OBJECT_ID = OBJECT_ID(N'RPT_CareBridge_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_CareBridge_Enrollment_Info
	ADD 
		[Primary_Physician] VARCHAR(1000) NULL;
END
GO

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician_Practice' AND OBJECT_ID = OBJECT_ID(N'RPT_CareBridge_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_CareBridge_Enrollment_Info 
	ADD
		[Primary_Physician_Practice] VARCHAR(2000) NULL;	
END	
GO

/**** RPT_Engage_Enrollment_Info ****/
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician' AND OBJECT_ID = OBJECT_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_Engage_Enrollment_Info 
	ADD 
		[Primary_Physician] VARCHAR(1000) NULL;
END
GO

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician_Practice' AND OBJECT_ID = OBJECT_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_Engage_Enrollment_Info 
	ADD 
		[Primary_Physician_Practice] VARCHAR(2000) NULL;	
END
GO


/******** ENG-724 **********/
/****** Object:  Table [dbo].[RPT_TouchPoint_Dim]    Script Date: 05/15/2015 16:23:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_TouchPoint_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_TouchPoint_Dim]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TouchPoint_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
GO


/******** ENG-1147 **********/
-- Drop RPT_Patient_PCM_Program_Info table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient_PCM_Program_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient_PCM_Program_Info]
GO

-- Drop spPhy_RPT_SavePatientInfo stored procedure
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
GO


/******** ENG-1216 **********/
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


/******************** patientprogramresponse *******************************/

/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]    Script Date: 05/17/2015 12:23:37 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]    Script Date: 05/17/2015 12:23:37 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramResponse]    Script Date: 05/17/2015 12:23:37 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramResponse]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramResponse]    Script Date: 05/17/2015 12:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramResponse](
	[ResponseId] [int] IDENTITY(1,1) NOT NULL,
	[MongoStepId] [varchar](50) NULL,
	[StepId] [int] NULL,
	[MongoNextStepId] [varchar](50) NULL,
	[NextStepId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoActionId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Text] [varchar](5000) NULL,
	[Value] [varchar](5000) NULL,
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
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_1] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_2')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_2] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_Composite')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_Composite] ON [dbo].[RPT_PatientProgramResponse] 
(
	[StepId] ASC,
	[Selected] ASC
)
INCLUDE ( [Text],
[Delete]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_Delete')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_Delete] ON [dbo].[RPT_PatientProgramResponse] 
(
	[Delete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_State')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_State] ON [dbo].[RPT_PatientProgramResponse] 
(
	[Selected] ASC,
	[Delete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_StepidSelected')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_StepidSelected] ON [dbo].[RPT_PatientProgramResponse] 
(
	[StepId] ASC,
	[Selected] ASC
)
INCLUDE ( [Text],
[Delete]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]    Script Date: 05/17/2015 12:23:37 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep] FOREIGN KEY([NextStepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]    Script Date: 05/17/2015 12:23:37 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep] FOREIGN KEY([StepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO


/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/17/2015 17:52:06 ******/
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]    Script Date: 05/17/2015 17:52:06 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/17/2015 17:52:06 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramStep]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/17/2015 17:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramStep](
	[StepId] [int] IDENTITY(1,1) NOT NULL,
	[MongoActionId] [varchar](50) NOT NULL,
	[ActionId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[AttributeEndDate] [datetime] NULL,
	[AttributeStartDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Header] [varchar](100) NULL,
	[SelectedResponseId] [varchar](50) NULL,
	[ControlType] [varchar](50) NULL,
	[SelectType] [varchar](50) NULL,
	[IncludeTime] [varchar](50) NULL,
	[Question] [varchar](max) NULL,
	[Title] [varchar](2000) NULL,
	[Description] [varchar](max) NULL,
	[Notes] [varchar](max) NULL,
	[Text] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[Response] [varchar](50) NULL,
	[StepTypeId] [varchar](50) NULL,
	[Enabled] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStep] PRIMARY KEY CLUSTERED 
(
	[StepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep] ON [dbo].[RPT_PatientProgramStep] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_1] ON [dbo].[RPT_PatientProgramStep] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_2')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_2] ON [dbo].[RPT_PatientProgramStep] 
(
	[StepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_ActionId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_ActionId] ON [dbo].[RPT_PatientProgramStep] 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_Sourceid')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_Sourceid] ON [dbo].[RPT_PatientProgramStep] 
(
	[SourceId] ASC
)
INCLUDE ( [MongoActionId],
[MongoId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]    Script Date: 05/17/2015 17:52:06 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction] FOREIGN KEY([ActionId])
REFERENCES [dbo].[RPT_PatientProgramAction] ([ActionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] CHECK CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO


/******************* RESTORE RPT_PatientNoteProgram ************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_PatientNote]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))
ALTER TABLE [dbo].[RPT_PatientNoteProgram] DROP CONSTRAINT [FK_PatientNoteProgram_PatientNote]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))
ALTER TABLE [dbo].[RPT_PatientNoteProgram] DROP CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))
ALTER TABLE [dbo].[RPT_PatientNoteProgram] DROP CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy]
GO

/****** Object:  Table [dbo].[RPT_PatientNoteProgram]    Script Date: 05/22/2015 13:10:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientNoteProgram]
GO

/****** Object:  Table [dbo].[RPT_PatientNoteProgram]    Script Date: 05/22/2015 13:10:42 ******/
CREATE TABLE [dbo].[RPT_PatientNoteProgram](
	[PatientNoteProgramId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientNoteId] [varchar](50) NOT NULL,
	[PatientNoteId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientNoteProgram] PRIMARY KEY CLUSTERED 
(
	[PatientNoteProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_PatientNote] FOREIGN KEY([PatientNoteId])
REFERENCES [dbo].[RPT_PatientNote] ([PatientNoteId])
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_PatientNote]
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy]
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy]
GO


/*********************** RPT_ContactAddress ***************************/
ALTER TABLE RPT_ContactAddress
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [TypeId] [int] NOT NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [StateId] [int] NOT NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [StateId] [int] NOT NULL


/***************************** RPT_ContactEmail ***************************************/
ALTER TABLE RPT_ContactEmail
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactEmail
ALTER COLUMN [TypeId] [int] NOT NULL


/***************************** RPT_ContactLanguage ***************************************/
ALTER TABLE RPT_ContactLanguage
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactLanguage
ALTER COLUMN [LanguageLookUpId] [int] NOT NULL

/***************************** RPT_ContactPhone ***************************************/
ALTER TABLE RPT_ContactPhone
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactPhone
ALTER COLUMN [TypeId] [int] NOT NULL

/***************************** RPT_ContactTOD ***************************************/
DELETE [RPT_ContactTimeOfDay]
ALTER TABLE [RPT_ContactTimeOfDay]
ALTER COLUMN [ContactId] [int] NOT NULL
GO

DELETE [RPT_ContactTimeOfDay]
ALTER TABLE [RPT_ContactTimeOfDay]
ALTER COLUMN [TimeOfDayLookUpId] [int] NOT NULL
GO

/***************************** RPT_ContactWeekDay ***************************************/
ALTER TABLE [RPT_ContactWeekDay]
ALTER COLUMN [ContactId] [int] NOT NULL

/***************************** RPT_ContactWeekDay ***************************************/
ALTER TABLE [RPT_ContactRecentList]
ALTER COLUMN [ContactId] [int] NOT NULL


/********************************* [RPT_UserRecentList] ********************************/
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'MongoUserId' AND OBJECT_ID = OBJECT_ID(N'RPT_UserRecentList'))
BEGIN
DELETE [RPT_UserRecentList]
ALTER TABLE [RPT_UserRecentList]
ADD [MongoUserId] VARCHAR(50) NOT NULL
END
GO

ALTER TABLE [RPT_UserRecentList]
ALTER COLUMN [UserId] [int] NOT NULL
GO

/********************************* [RPT_Observation] ********************************/
ALTER TABLE [RPT_Observation]
ALTER COLUMN [HighValue] [int] NULL
GO

ALTER TABLE [RPT_Observation]
ALTER COLUMN [LowValue] [int] NULL
GO

/************** RPT_SaveObservation *********************/
ALTER PROCEDURE [dbo].[spPhy_RPT_SaveObservation] 
	@MongoID varchar(50),
	@Code varchar(50),
	@CodingSystemId varchar(50),
	@Delete varchar(50),
	@Description varchar(MAX),
	@ExtraElements varchar(MAX),
	@HighValue INT,
	@LastUpdatedOn datetime,
	@LowValue INT,
	@ObservationTypeMongoId varchar(50),
	@Order INT,
	@Source varchar(50),
	@Standard varchar(50),
	@Status varchar(50),
	@TimeToLive datetime,
	@Units varchar(50),
	@UpdatedBy varchar(50),
	@Version float,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@CommonName varchar(100),
	@GroupId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @ObservationTypeId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ObservationTypeId = ObservationTypeId From RPT_ObservationTypeLookUp Where MongoId = @ObservationTypeMongoId
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId from [RPT_User] Where MongoId = @RecordCreatedBy
	
	
	If (@HighValue = -1)
	Select @HighValue = NULL
	
	If (@LowValue = -1)
	Select @LowValue = NULL
	
	If (@Order = -1)
	Select @Order = NULL
	
	If Exists(Select Top 1 1 From RPT_Observation Where MongoId = @MongoID)
	Begin
		Update RPT_Observation
		Set Code = @Code,
			CodingSystemId = @CodingSystemId,
			[Delete] = @Delete,
			[Description] = @Description,
			ExtraElements = @ExtraElements,
			HighValue = @HighValue,
			LastUpdatedOn = @LastUpdatedOn,
			LowValue = @LowValue,
			ObservationTypeId = @ObservationTypeId,
			MongoObservationLookUpId = @ObservationTypeMongoId,
			[Order] = @Order,
			[Source] = @Source,
			[Standard] = @Standard,
			[Status] = @Status,
			TTLDate = @TimeToLive,
			Units = @Units,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version,
			CommonName = @CommonName,
			GroupId = @GroupId
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_Observation(CommonName, GroupId, Code, CodingSystemId, [Delete], [Description], ExtraElements, HighValue, LastUpdatedOn, LowValue, ObservationTypeId, MongoObservationLookUpId, [Order], [Source], [Standard], [Status], TTLDate, Units, MongoUpdatedBy, UpdatedById, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version], MongoId) values 
		(@CommonName, @GroupId, @Code, @CodingSystemId, @Delete, @Description, @ExtraElements, @HighValue, @LastUpdatedOn, @LowValue, @ObservationTypeId, @ObservationTypeMongoId, @Order, @Source, @Standard, @Status, @TimeToLive, @Units, @UpdatedBy, @UpdatedById, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Version, @MongoID)
	End
END
GO


/********************* ENG-1212  **************************/
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_PCP_Practice_Val]    Script Date: 05/12/2015 12:48:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_RPT_PCP_Practice_Val]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_RPT_PCP_Practice_Val]    Script Date: 05/12/2015 12:48:08 ******/
CREATE FUNCTION [dbo].[fn_RPT_PCP_Practice_Val] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 135;
	--SET @patientprogramid = 269;
	--SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422deccac80d3356d000002';
	--------------------------------

	DECLARE @CPCTemp VARCHAR(200);
	DECLARE @Result VARCHAR(200);
	
	SET @CPCTemp = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
	SET @CPCTemp = LOWER(RTRIM(LTRIM(@CPCTemp)));
	
	SET @Result =
		CASE
			WHEN @CPCTemp = 'other' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df97ac80d3356d000004') )							
			ELSE
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
		END;

	INSERT INTO @ResultTable (Value) VALUES (@Result);
			
	RETURN
END

GO


/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_PCP_Practice_Val]    Script Date: 05/12/2015 12:48:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_RPT_Engage_PCP_Practice_Val]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_PCP_Practice_Val]    Script Date: 05/14/2015 11:07:21 ******/
CREATE FUNCTION [dbo].[fn_RPT_Engage_PCP_Practice_Val] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	----SET @patientid = 546;
	----SET @patientprogramid = 283;
	--SET @patientid = 217;
	--SET @patientprogramid = 280;	
	--SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7';
	--SET @ActionSourceId = '545cee7a890e9458aa000003';
	--SET @StepSourceId = '545ce6f8890e9458a9000002';
	--------------------------------------


	DECLARE @CPCTemp VARCHAR(200);
	DECLARE @Result VARCHAR(200);
	
	SET @CPCTemp = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
	SET @CPCTemp = LOWER(RTRIM(LTRIM(@CPCTemp)));
	
	-------
	--select @CPCTemp;
	-------
	
	SET @Result =
		CASE
			WHEN @CPCTemp = 'other' THEN
				'other'
			ELSE
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
		END;

	---------
	--select @Result
	---------

	INSERT INTO @ResultTable (Value) VALUES (@Result);
			
	RETURN
END
GO