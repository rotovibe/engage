/*** ENG-725 ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalMetrics]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientGoalMetrics]
GO

CREATE TABLE [dbo].[RPT_PatientGoalMetrics](
	[PatientId] [int] NOT NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[Age] [tinyint] NULL,
	[Gender] [varchar](50) NULL,
	[Priority] [varchar](50) NULL,
	[SystemId] [varchar](50) NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[PatientProgramId] [int] NULL,
	[ProgramName] [varchar](100) NULL,
	[ProgramState] [varchar](50) NULL,
	[ProgramAssignedDate] [datetime] NULL,
	[ProgramStartDate] [datetime] NULL,
	[ProgramEndDate] [datetime] NULL,
	[PatientGoalId] [int] NULL,
	[PatientGoalName] [varchar](500) NULL,
	[PatientGoalDesc] [varchar](50) NULL,
	[PatientGoalStartDate] [datetime] NULL,
	[PatientGoalEndDate] [datetime] NULL,
	[PatientGoalSource] [varchar](50) NULL,
	[PatientGoalStatus] [varchar](50) NULL,
	[PatientGoalStatusDate] [datetime] NULL,
	[PatientGoalTargetDate] [datetime] NULL,
	[PatientGoalTargetValue] [varchar](300) NULL,
	[PatientGoalType] [varchar](50) NULL,
	[PatientGoalLastUpdatedOn] [datetime] NULL,
	[PatientGoalCreatedOn] [datetime] NULL,
	[PatientGoalClosedDate] [datetime] NULL,
	[PatientGoalFocusArea] [varchar](100) NULL,
	[PatientGoalProgramName] [varchar](100) NULL,
	[PatientGoalProgramState] [varchar](50) NULL,
	[PatientGoalProgramAssignedDate] [datetime] NULL,
	[PatientGoalProgramStartDate] [datetime] NULL,
	[PatientGoalProgramEndDate] [datetime] NULL,
	[PatientGoalAttributeName] [varchar](100) NULL,
	[PatientGoalAttributeValue] [varchar](50) NULL,
	[PatientBarrierId] [int] NULL,
	[PatientBarrierName] [varchar](500) NULL,
	[PatientBarrierCategory] [varchar](100) NULL,
	[PatientBarrierStatus] [varchar](50) NULL,
	[PatientBarrierStatusDate] [datetime] NULL,
	[PatientBarrierLastUpdated] [datetime] NULL,
	[PatientBarrierCreatedOn] [datetime] NULL,
	[PatientTaskId] [int] NULL,
	[PatientTaskDescription] [varchar](max) NULL,
	[PatientTaskStartDate] [datetime] NULL,
	[PatientTaskStatus] [varchar](50) NULL,
	[PatientTaskStatusDate] [datetime] NULL,
	[PatientTaskTargetDate] [datetime] NULL,
	[PatientTaskTargetValue] [varchar](50) NULL,
	[PatientTaskLastUpdated] [datetime] NULL,
	[PatientTaskCreatedOn] [datetime] NULL,
	[PatientTaskClosedDate] [datetime] NULL,
	[PatientTaskBarrierName] [varchar](500) NULL,
	[PatientTaskAttributeName] [varchar](100) NULL,
	[PatientTaskAttributeValue] [varchar](50) NULL,
	[PatientInterventionId] [int] NULL,
	[PatientInterventionCategoryName] [varchar](100) NULL,
	[PatientInterventionDesc] [varchar](max) NULL,
	[PatientInterventionStartDate] [datetime] NULL,
	[PatientInterventionStatus] [varchar](50) NULL,
	[PatientInterventionStatusDate] [datetime] NULL,
	[PatientInterventionLastUpdated] [datetime] NULL,
	[PatientInterventionCreatedOn] [datetime] NULL,
	[PatientInterventionClosedDate] [datetime] NULL,
	[PatientInterventionAssignedTo] [varchar](100) NULL,
	[PatientInterventionBarrierName] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientGoalMetrics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
AS
BEGIN
	DELETE [RPT_PatientGoalMetrics]
	INSERT INTO [RPT_PatientGoalMetrics]
	(
		 PatientId
		,FirstName				
		,MiddleName
		,LastName				
		,DateOfBirth
		,Age
		,Gender
		,[Priority]
		,SystemId
		,Assigned_PCM
		,PatientProgramId		
		,ProgramName
		,ProgramState
		,ProgramAssignedDate
		,ProgramStartDate
		,ProgramEndDate
		,PatientGoalId
		,PatientGoalName
		,PatientGoalDesc
		,PatientGoalStartDate
		,PatientGoalEndDate
		,PatientGoalSource
		,PatientGoalStatus
		,PatientGoalStatusDate
		,PatientGoalTargetDate
		,PatientGoalTargetValue
		,PatientGoalType
		,PatientGoalLastUpdatedOn
		,PatientGoalCreatedOn
		,PatientGoalClosedDate
		,PatientGoalFocusArea
		,PatientGoalProgramName
		,PatientGoalProgramState
		,PatientGoalProgramAssignedDate
		,PatientGoalProgramStartDate
		,PatientGoalProgramEndDate
		,PatientGoalAttributeName
		,PatientGoalAttributeValue
		,PatientBarrierId
		,PatientBarrierName
		,PatientBarrierCategory
		,PatientBarrierStatus
		,PatientBarrierStatusDate
		,PatientBarrierLastUpdated
		,PatientBarrierCreatedOn
		,PatientTaskId
		,PatientTaskDescription
		,PatientTaskStartDate
		,PatientTaskStatus
		,PatientTaskStatusDate
		,PatientTaskTargetDate
		,PatientTaskTargetValue
		,PatientTaskLastUpdated
		,PatientTaskCreatedOn
		,PatientTaskClosedDate
		,PatientTaskBarrierName
		,PatientTaskAttributeName
		,PatientTaskAttributeValue
		,PatientInterventionId
		,PatientInterventionCategoryName
		,PatientInterventionDesc
		,PatientInterventionStartDate
		,PatientInterventionStatus
		,PatientInterventionStatusDate
		,PatientInterventionLastUpdated
		,PatientInterventionCreatedOn
		,PatientInterventionClosedDate
		,PatientInterventionAssignedTo
		,PatientInterventionBarrierName 	
	) 
SELECT DISTINCT 	
		  p.PatientId
		  ,p.FirstName
		  ,p.MiddleName
		  ,p.LastName
		  ,p.DateOfBirth
		  ,CASE WHEN p.DATEOFBIRTH != '' AND ISDATE(p.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,p.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age
		  ,p.Gender
		  ,p.[Priority]
		  ,ps.SystemId
		  ,u.PreferredName as Assigned_PCM
		  ,ppt.PatientProgramId
		  ,ppt.Name as ProgramName
		  ,ppt.[State] as ProgramState 
		  ,ppt.AssignedOn as ProgramAssignedDate 	
		  ,ppt.AttributeStartDate as ProgramStartDate 	
		  ,ppt.AttributeEndDate as ProgramEndDate
		  ,pg.PatientGoalId
		  ,pg.Name as PatientGoalName
		  ,pg.Description as PatientGoalDesc
		  ,pg.StartDate as PatientGoalStartDate
		  ,pg.EndDate as PatientGoalEndDate
		  ,pgl.Name as PatientGoalSource
		  ,pg.[Status]  as PatientGoalSource
		  ,pg.StatusDate as PatientGoalStatusDate
		  ,pg.TargetDate as PatientGoalTargetDate
		  ,pg.TargetValue  as PatientGoalTargetValue
		  ,pg.[Type] as PatientGoalType
		  ,pg.LastUpdatedOn as PatientGoalLastUpdatedOn
		  ,pg.RecordCreatedOn as PatientGoalCreatedOn
		  ,pg.ClosedDate  as PatientGoalClosedDate
		  ,pgfa.Name as PatientGoalFocusArea
		  ,pp1.Name as PatientGoalProgramName
		  ,pp1.[State] as PatientGoalProgramState
		  ,pp1.AssignedOn  as PatientGoalProgramAssignedDate
		  ,pp1.AttributeStartDate  as PatientGoalProgramStartDate
		  ,pp1.AttributeEndDate  as PatientGoalProgramEndDate
		  ,ga.Name as PatientGoalAttributeName
		  ,gao.Value as PatientGoalAttributeValue
		  ,pb.PatientBarrierId
		  ,pb.Name as PatientBarrierName
		  ,bcl.Name as PatientBarrierCategory
		  ,pb.[Status]  as PatientBarrierStatus
		  ,pb.StatusDate as PatientBarrierStatusDate
		  ,pb.LastUpdatedOn as PatientBarrierLastUpdated
		  ,pb.RecordCreatedOn as PatientBarrierCreatedOn
		  ,pt.PatientTaskId
		  ,pt.Description as PatientTaskDescription
		  ,pt.StartDate as PatientTaskStartDate
		  ,pt.[Status] as PatientTaskStatus
		  ,pt.StatusDate as PatientTaskStatusDate
		  ,pt.TargetDate as PatientTaskTargetDate
		  ,pt.TargetValue as PatientTaskTargetValue
		  ,pt.LastUpdatedOn as PatientTaskLastUpdated
		  ,pt.RecordCreatedOn as PatientTaskCreatedOn
		  ,pt.ClosedDate as PatientTaskClosedDate
		  ,pb1.Name as PatientTaskBarrierName
		  ,ga1.Name as PatientTaskAttributeName
		  ,gao1.Value as PatientTaskAttributeValue
		  ,pi.PatientInterventionId
		  ,icl.Name as PatientInterventionCategoryName
		  ,pi.Description as PatientInterventionDesc
		  ,pi.StartDate as PatientInterventionStartDate
		  ,pi.[Status] as PatientInterventionStatus
		  ,pi.StatusDate as PatientInterventionStatusDate
		  ,pi.LastUpdatedOn as PatientInterventionLastUpdated
		  ,pi.RecordCreatedOn as PatientInterventionCreatedOn
		  ,pi.ClosedDate as PatientInterventionClosedDate
		  ,u1.PreferredName as PatientInterventionAssignedTo
		  ,pb2.Name as PatientInterventionBarrierName 	
	FROM
		  RPT_Patient as p with (nolock) 	
		  LEFT JOIN RPT_PatientProgram as ppt with (nolock) ON p.PatientId = ppt.PatientId and ppt.[Delete] = 'False' and ppt.TTLDate IS NULL
	      LEFT JOIN RPT_PatientSystem as ps with (nolock) ON p.PatientId = ps.PatientId
	      LEFT JOIN RPT_CareMember as c with (nolock) on p.PatientId = c.PatientId
		  LEFT JOIN RPT_User as u with (nolock) on c.UserId = u.UserId 
		  LEFT JOIN RPT_PatientGoal as pg with (nolock) ON p.PatientId = pg.PatientId and pg.[Delete] = 'False' and pg.TTLDate IS NULL 
		  left join RPT_SourceLookUp as pgl with (nolock) on pg.Source = pgl.MongoId
		  left join RPT_PatientGoalFocusArea as pgf with (nolock) on pg.PatientGoalId = pgf.PatientGoalId
		  left join RPT_FocusAreaLookUp as pgfa with (nolock) on pgf.FocusAreaId = pgfa.FocusAreaId
		  left join RPT_PatientGoalProgram as pgp with (nolock) on pg.PatientGoalId = pgp.PatientGoalId
		  left join RPT_PatientProgram as pp1 with (nolock) on pgp.MongoId = pp1.MongoId
		  left join RPT_PatientGoalAttribute as pga with (nolock) on pg.PatientGoalId = pga.PatientGoalId
		  left join RPT_GoalAttribute as ga with (nolock) on pga.GoalAttributeID = ga.GoalAttributeID
		  left join RPT_PatientGoalAttributeValue as pgav with (nolock) on pga.PatientGoalAttributeId = pgav.PatientGoalAttributeId
		  left join RPT_GoalAttributeOption as gao with (nolock) on pgav.Value = gao.[Key] and ga.GoalAttributeID = gao.GoalAttributeId
		  left join RPT_PatientBarrier as pb with (nolock) on pg.PatientGoalId = pb.PatientGoalId  and pb.[Delete] = 'False' and pb.TTLDate IS NULL 
		  left join RPT_BarrierCategoryLookUp as bcl with (nolock) on pb.CategoryLookUpId = bcl.BarrierCategoryId
		  left join RPT_PatientTask as pt with (nolock) on pg.PatientGoalId = pt.PatientGoalId and pt.[Delete] = 'False' and pt.TTLDate IS NULL 
		  left join RPT_PatientTaskBarrier as ptb with (nolock) on pt.PatientTaskId = ptb.PatientTaskId
		  left join RPT_PatientBarrier as pb1 with (nolock) on ptb.PatientBarrierId = pb1.PatientBarrierId
		  left join RPT_PatientTaskAttribute as pta with (nolock) on pt.PatientTaskId = pta.PatientTaskId
		  left join RPT_GoalAttribute as ga1 with (nolock) on pta.GoalAttributeID = ga1.GoalAttributeID
		  left join RPT_PatientTaskAttributeValue as ptav with (nolock)on pta.PatientTaskAttributeId = ptav.PatientTaskAttributeId
		  left join RPT_GoalAttributeOption as gao1 with (nolock) on ptav.Value = gao1.[Key] and ga1.GoalAttributeID = gao1.GoalAttributeId
		  left join RPT_PatientIntervention as pi with (nolock) on pg.PatientGoalId = pi.PatientGoalId and pi.[Delete] = 'False' and pi.TTLDate IS NULL 
		  left join RPT_InterventionCategoryLookUp as icl with (nolock) on pi.CategoryLookUpId = icl.InterventionCategoryId
		  left join RPT_User as u1 with (nolock)on pi.AssignedToUserId = u1.UserId
		  left join RPT_PatientInterventionBarrier as pib with (nolock) on pi.PatientInterventionId = pib.PatientInterventionId
		  left join RPT_PatientBarrier as pb2 with (nolock) on pib.PatientBarrierId = pb2.PatientBarrierId
	WHERE
		p.[Delete] = 'False' and p.TTLDate IS NULL 
END
GO

/*** ENG-734 ***/
/***************** TO DO METRICS *******************/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Dim_ToDo]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Dim_ToDo]
GO

CREATE TABLE [dbo].[RPT_Dim_ToDo](
	[DimToDoId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientId] [varchar](50) NULL,
	[MongoToDoId] [varchar](50) NULL,
	[Source] [varchar](200) NULL,
	[Category] [varchar](100) NULL,
	[Status] [varchar](100) NULL,
	[AssignedTo] [varchar](1000) NULL,
	[DueDate] datetime NULL,
	[Priority] varchar(200) NULL,
	[Related Program] varchar(2000) NULL,
	[Title] varchar(900) NULL,
	[Description] varchar(900) NULL,
	[RecordCreatedOn] datetime NULL,
	[RecordCreatedBy] varchar(200) NULL	,
	[UpdatedBy] varchar(2000) NULL,
	[LastUpdatedOn]	datetime NULL
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_ToDo_Dim]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_Dim_ToDo]
	INSERT INTO [RPT_Dim_ToDo]
	(
		[MongoPatientId],
		[MongoToDoId],
		[Source],
		[Category],
		[Related Program],
		[Status],
		[AssignedTo],		
		[Title],
		[Description],
		[DueDate],
		[Priority],		
		[RecordCreatedOn],
		[RecordCreatedBy],
		[LastUpdatedOn],
		[UpdatedBy]
	)
	SELECT
		(CASE WHEN td.MongoPatientId = '' THEN  NULL ELSE td.MongoPatientId END) as [MongoPatientId],
		td.MongoId as [MongoToDoId],
		(CASE WHEN td.MongoSourceId = '000000000000000000000000' THEN  NULL ELSE td.MongoSourceId END) as [Source],
		(select Name from RPT_ToDoCategoryLookUp where MongoId = td.MongoCategory) as [Category],
		(select Name from RPT_PatientProgram where MongoId = tdp.MongoProgramId) as [Related Program],
		td.[Status],
		(select PreferredName  from RPT_User where MongoId = td.MongoAssignedToId) as [AssignedTo],
		td.[Title],
		(CASE WHEN td.[Description] = '' THEN  NULL ELSE td.[Description] END) as [Description],
		td.DueDate,
		td.[Priority],
		td.RecordCreatedOn,
		(select PreferredName  from RPT_User where MongoId = td.MongoRecordCreatedBy) as [RecordCreatedBy],
		td.LastUpdatedOn,
		(select PreferredName from RPT_User where MongoId = td.MongoUpdatedBy) as [UpdatedBy]
	FROM 
		RPT_ToDo as td
		LEFT JOIN RPT_ToDoProgram tdp ON td.MongoId = tdp.MongoToDoId
	WHERE
		td.[DeleteFlag] = 'False'	
		AND td.TTLDate IS NULL
END
GO

/*** RPT_SprocName ***/
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_ToDo_Dim', 'false');
GO

/*** ENG-1146 ***/
ALTER PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE RPT_CareMember
	DELETE RPT_CareMemberTypeLookUp
	DELETE RPT_ContactEmail
	DELETE RPT_ContactPhone
	DELETE RPT_ContactAddress
	DELETE RPT_ContactRecentList
	DELETE RPT_ContactMode
	DELETE RPT_ContactLanguage
	DELETE RPT_ContactWeekDay
	DELETE RPT_ContactTimeOfDay
	-- todos
	DELETE RPT_ToDoProgram
	DELETE RPT_ToDo
	-- patient programs
	DELETE RPT_SpawnElements
	DELETE RPT_SpawnElementTypeCode
	DELETE RPT_PatientProgramAttribute
	DELETE RPT_PatientProgramResponse
	DELETE RPT_PatientProgramStep
	DELETE RPT_PatientProgramAction	
	DELETE RPT_PatientProgramModule
	DELETE RPT_PatientProgram	
	DELETE RPT_PatientNoteProgram
	DELETE RPT_PatientNote
	DELETE RPT_PatientProblem
	DELETE RPT_ObjectiveCategory
	DELETE RPT_ObjectiveLookUp	
	DELETE RPT_PatientObservation	
	DELETE RPT_Observation
	DELETE RPT_PatientTaskAttributeValue
	DELETE RPT_PatientTaskAttribute
	DELETE RPT_PatientTaskBarrier
	DELETE RPT_PatientTask
	-- patient allergies
	DELETE RPT_Allergy
	DELETE RPT_AllergyType
	DELETE RPT_PatientAllergy
	DELETE RPT_PatientAllergyReaction
	-- patient medsupps
	DELETE RPT_PatientMedSuppPhClass
	DELETE RPT_MedPharmClass
	DELETE RPT_PatientMedSuppNDC
	DELETE RPT_PatientMedSupp	
	DELETE RPT_Medication
	Delete RPT_MedicationMap
	Delete RPT_PatientMedFrequency
	Delete RPT_CustomPatientMedFrequency
	
	-- patient goal
	DELETE RPT_PatientGoalProgram
	DELETE RPT_PatientGoalFocusArea
	DELETE RPT_GoalAttributeOption	
	DELETE RPT_PatientGoalAttributeValue
	DELETE RPT_PatientGoalAttribute
	DELETE RPT_GoalAttribute
	DELETE RPT_PatientInterventionBarrier
	DELETE RPT_PatientIntervention	
	DELETE RPT_PatientBarrier	
	DELETE RPT_PatientGoal
	DELETE RPT_PatientUser	
	DELETE RPT_Contact
	DELETE RPT_PatientSystem
	DELETE RPT_Patient
	DELETE RPT_CommTypeCommMode
	DELETE RPT_ToDoCategoryLookUp	
	DELETE RPTMongoCategoryLookUp
	DELETE RPT_SourceLookUp
	DELETE RPT_BarrierCategoryLookUp
	DELETE RPT_InterventionCategoryLookUp
	DELETE RPTMongoTimeZoneLookUp
	DELETE RPT_ProblemLookUp
	DELETE RPT_TimesOfDayLookUp
	DELETE RPT_CommTypeLookUp
	DELETE RPT_CommModeLookUp
	DELETE RPT_StateLookUp
	DELETE RPT_LanguageLookUp
	DELETE RPT_FocusAreaLookUp
	DELETE RPT_CodingSystemLookUp
	DELETE RPT_ObservationTypeLookUp
	DELETE RPT_AllergyTypeLookUp
	DELETE RPT_AllergySourceLookUp
	DELETE RPT_SeverityLookUp
	DELETE RPT_ReactionLookUp
	DELETE RPT_MedSupTypeLookUp
	DELETE RPT_FreqHowOftenLookUp
	DELETE RPT_FreqWhenLookUp
	DELETE RPT_NoteTypeLookUp
	DELETE RPT_UserRecentList
	DELETE [RPT_User]
	
	--DELETE CohortPatientView	
	--DELETE CohortPatientViewSearchField
	
	DBCC CHECKIDENT ('RPT_CareMember', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_CareMemberTypeLookUp', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_ContactLanguage', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactWeekDay', RESEED, 0)  
	DBCC CHECKIDENT ('RPT_ContactTimeOfDay', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactRecentList', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_ContactMode', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactPhone', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactAddress', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactEmail', RESEED, 0)
	
-- reseed program tables
	DBCC CHECKIDENT ('RPT_PatientProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SpawnElements', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramModule', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAction', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramStep', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramResponse', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAttribute', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_PatientNoteProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientNote', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProblem', RESEED, 0)

	-- allergies
	DBCC CHECKIDENT ('RPT_AllergyType', RESEED, 0)	
	DBCC CHECKIDENT ('RPT_Allergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedicationMap', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Medication', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CustomPatientMedFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedPharmClass', RESEED,0)
	
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SeverityLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ReactionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedSupTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqHowOftenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqWhenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteTypeLookUp', RESEED, 0)

	DBCC CHECKIDENT ('RPT_ObjectiveCategory', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObjectiveLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientObservation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Observation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTask', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientGoalProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalFocusArea', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttributeOption', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttribute', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientInterventionBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientIntervention', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoal', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientUser', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Contact', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientSystem', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_Patient', RESEED, 0) 
	
	DBCC CHECKIDENT ('RPT_CommTypeCommMode', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_BarrierCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_InterventionCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoTimeZoneLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ProblemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_TimesOfDayLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommModeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_StateLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_LanguageLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FocusAreaLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CodingSystemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObservationTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_UserRecentList', RESEED, 0)
	DBCC CHECKIDENT ('RPT_User', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_ToDoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDoProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDo', RESEED, 0)
	
	-- patient allergies
	DBCC CHECKIDENT ('RPT_PatientAllergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientAllergyReaction', RESEED, 0)
	
	--DBCC CHECKIDENT ('RPT_CohortPatientView', RESEED, 0)
	--DBCC CHECKIDENT ('RPT_CohortPatientViewSearchField', RESEED, 0)
END
GO

/*** RPT_Medication ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Medication')
	DROP TABLE [RPT_Medication];
GO

CREATE TABLE [dbo].[RPT_Medication](
[MedId] [int] IDENTITY(1,1) NOT NULL,
[MongoId] [varchar](50) NOT NULL,
[ProductId] [varchar](100) NULL,
[NDC] [varchar](20) NULL,
[FullName] [varchar](300) NULL,
[ProprietaryName] [varchar](300) NULL,
[ProprietaryNameSuffix] [varchar](200) NULL,
[StartDate] [varchar](100) NULL,
[EndDate] [varchar](100) NULL,
[SubstanceName] [varchar](3000) NULL,
[Route] [varchar](200) NULL,
[Form] [varchar](100) NULL,
[FamilyId] [varchar](50) NULL,
[Unit] [varchar](5000) NULL,
[Strength] [varchar](900) NULL,
[Version] [float] NULL,
[DeleteFlag] [varchar](50) NULL,
[TTLDate] [datetime] NULL,
[LastUpdatedOn] [datetime] NULL,
[MongoRecordCreatedBy] [varchar](50) NULL,
[RecordCreatedOn] [datetime] NULL,
[MongoUpdatedBy] [varchar](50) NULL,
	
 CONSTRAINT [PK_Medication] PRIMARY KEY CLUSTERED 
(
	[MedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [RPT_Medication_MongoId] ON [dbo].[RPT_Medication] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [RPT_Medication_NDC] ON [dbo].[RPT_Medication] 
(
	[NDC] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/*** RPT_Medication ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedPharmClass')
	DROP TABLE [RPT_MedPharmClass];
GO

CREATE TABLE [dbo].[RPT_MedPharmClass](
[PhmCId] [int] IDENTITY(1,1) NOT NULL,
[MedMongoId] [varchar](50) NULL,
[PharmClass] [varchar](150) NULL,
	
 CONSTRAINT [PK_MedPharmClass] PRIMARY KEY CLUSTERED 
(
	[PhmCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [Med_Mongo_Id] ON [dbo].[RPT_MedPharmClass] 
(
	[MedMongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PharmClass] ON [dbo].[RPT_MedPharmClass] 
(
	[PharmClass] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/*** RPT_MedicationMap ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedicationMap')
	DROP TABLE [RPT_MedicationMap];
GO

CREATE TABLE [dbo].[RPT_MedicationMap](
[MedMapId] [int] IDENTITY(1,1) NOT NULL,
[MongoId] [varchar](50) NOT NULL,
[FullName] [varchar](300) NULL,
[SubstanceName] [varchar](3000) NULL,
[Route] [varchar](200) NULL,
[Form] [varchar](100) NULL,
[Strength] [varchar](2000) NULL,
[Version] [float] NULL,
[DeleteFlag] [varchar](50) NULL,
[Custom] [varchar](50) NULL,
[Verified] [varchar](50) NULL,
[TTLDate] [datetime] NULL,
[LastUpdatedOn] [datetime] NULL,
[MongoRecordCreatedBy] [varchar](50) NULL,
[RecordCreatedOn] [datetime] NULL,
[MongoUpdatedBy] [varchar](50) NULL,
	
 CONSTRAINT [PK_MedicationMap] PRIMARY KEY CLUSTERED 
(
	[MedMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

/**** RPT_PatientMedSupp ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSupp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientMedSupp]
GO

CREATE TABLE [dbo].[RPT_PatientMedSupp](
	[PatientMedSuppId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoFrequencyId] [varchar](50) NULL,	
	[MongoFamilyId] [varchar](50) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Category] [varchar](200) NOT NULL,
	[MongoTypeId] [varchar](50) NOT NULL,
	[TypeId] [int] NULL,
	[Status] [varchar](200) NOT NULL,
	[Strength] [varchar](200) NULL,
	[Route] [varchar](200) NULL,
	[Form] [varchar](200) NULL,
	[FreqQuantity] [varchar](200) NULL,
	[MongoSourceId] [varchar](50) NOT NULL,
	[SourceId] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Reason] [varchar](5000) NULL,
	[Notes] [varchar](5000) NULL,
	[PrescribedBy] [varchar](500) NULL,
	[SystemName] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientMedSupp] PRIMARY KEY CLUSTERED 
(
	[PatientMedSuppId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_PatientMedSupp_Compound') 
	DROP INDEX IX_RPT_PatientMedSupp_Compound ON RPT_PatientMedSupp; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_PatientMedSupp_Compound 
	ON [RPT_PatientMedSupp] 
	(
		[MongoId]
		,[PatientId]
		,[MongoPatientId]
		,[MongoFrequencyId]
		,[MongoFamilyId]
		,[Name]
		--,[Category]
		--,[MongoTypeId]
		,[TypeId]
		--,[Status]
		--,[Strength]
		,[Route]
		,[Form]
		--,[FreqQuantity]
		--,[MongoSourceId]
		,[SourceId]
	) ON [PRIMARY]; 
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientMedSupp]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp] 
	@MongoId				[varchar](50),
	@MongoPatientId			[varchar](50),
	@MongoFrequencyId		[VARCHAR](50),	
	@MongoFamilyId			[VARCHAR](50),
	@Name					[varchar] (200),
	@Category				[varchar] (200),
	@MongoTypeId			[varchar] (50),
	@Status					[varchar] (200),
	@Strength				[varchar] (200),
	@Route					[varchar] (200),
	@Form					[varchar] (200),
	@FreqQuantity			[varchar] (200),
	@MongoSourceId			[varchar](50),	
	@StartDate				[datetime],
	@EndDate				[datetime],
	@Reason					[varchar](5000),
	@Notes					[varchar](5000),	
	@PrescribedBy			[varchar](500),	
	@SystemName				[varchar](50),
	@MongoUpdatedBy			[varchar](50),
	@LastUpdatedOn			[datetime],
	@MongoRecordCreatedBy	[varchar](50),
	@RecordCreatedOn		[datetime],
	@Version				[float],
	@TTLDate				[DATETIME],
	@Delete					[VARCHAR](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@FreqHowOftenId INT,
			@FreqWhenId INT,			
			@RecordCreatedById INT,
			@UpdatedById INT,
			@MedSuppTypeId INT,
			@SourceId INT
	
	select @MedSuppTypeId = mst.MedSupId  from RPT_MedSupTypeLookUp as mst where MongoId = @MongoTypeId
	select @SourceId = AllergySourceId from RPT_AllergySourceLookUp where MongoId = @MongoSourceId
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @MongoPatientId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientMedSupp Where MongoId = @MongoID)
		Begin
			Update RPT_PatientMedSupp
			Set 
				MongoId					= @MongoId,				
				MongoPatientId			= @MongoPatientId,
				MongoFrequencyId		= @MongoFrequencyId,
				MongoFamilyId			= @MongoFamilyId,
				PatientId				= @PatientId,			
				Name					= @Name,					
				Category				= @Category,				
				MongoTypeId				= @MongoTypeId,
				TypeId					= @MedSuppTypeId,			
				[Status]				= @Status,					
				Strength				= @Strength,				
				[Route]					= @Route,					
				Form					= @Form,					
				FreqQuantity			= @FreqQuantity,			
				MongoSourceId			= @MongoSourceId,			
				SourceId				= @SourceId,			
				StartDate				= @StartDate,				
				EndDate					= @EndDate,				
				Reason					= @Reason,					
				Notes					= @Notes,					
				PrescribedBy			= @PrescribedBy,			
				SystemName				= @SystemName,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				UpdatedBy				= @UpdatedById,				
				LastUpdatedOn			= @LastUpdatedOn,			
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,
				RecordCreatedBy			= @RecordCreatedById,
				RecordCreatedOn			= @RecordCreatedOn,
				[Version]				= @Version,
				TTLDate					= @TTLDate,
				[Delete]				= @Delete
			Where 
				MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_PatientMedSupp
			(
				MongoId,
				MongoPatientId,
				MongoFrequencyId,				
				MongoFamilyId,
				PatientId,
				Name,
				Category,
				MongoTypeId,
				TypeId,
				[Status],
				Strength,
				[Route],
				Form,
				FreqQuantity,					
				MongoSourceId,			
				SourceId,				
				StartDate,				
				EndDate,					
				Reason,					
				Notes,					
				PrescribedBy,			
				SystemName,				
				MongoUpdatedBy,			
				UpdatedBy,				
				LastUpdatedOn,			
				MongoRecordCreatedBy,	
				RecordCreatedBy,			
				RecordCreatedOn,			
				[Version],				
				TTLDate,					
				[Delete]						
			) 
			values 
			(
				@MongoId,				
				@MongoPatientId,
				@MongoFrequencyId,				
				@MongoFamilyId,
				@PatientId,			
				@Name,				
				@Category,			
				@MongoTypeId,
				@MedSuppTypeId,		
				@Status,					
				@Strength,			
				@Route,				
				@Form,				
				@FreqQuantity,		
				@MongoSourceId,		
				@SourceId,			
				@StartDate,			
				@EndDate,				
				@Reason,				
				@Notes,				
				@PrescribedBy,		
				@SystemName,			
				@MongoUpdatedBy,		
				@UpdatedById,			
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,
				@Version,
				@TTLDate,
				@Delete
			)
		End
END
GO

/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedFrequency]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientMedFrequency]
GO

CREATE TABLE [dbo].[RPT_PatientMedFrequency](
	[PMFreqId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[LookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
)
GO

/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_CustomPatientMedFrequency]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_CustomPatientMedFrequency]
GO

CREATE TABLE [dbo].[RPT_CustomPatientMedFrequency](
	[CPMFreqId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[Name] [varchar](300) NULL,
	[MongoPatientId] [VARCHAR](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [VARCHAR](50) NULL,
	[DeleteFlag] [VARCHAR](50) NULL,
	[TTLDate] [DATETIME] NULL,
	[LastUpdatedOn] [DATETIME] NULL,
	[MongoRecordCreatedBy] [VARCHAR](50) NULL,
	[RecordCreatedOn] [DATETIME] NULL
	
)
GO

/*** [add non clustered indexes to medication, map, and phcls] ***/
IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_Medication_FamilyId') 
	DROP INDEX IX_RPT_Medication_FamilyId ON RPT_Medication; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_Medication_FamilyId 
	ON RPT_Medication 
	(
		[FamilyId]
		,[ProductId]
		,[NDC]
		,[ProprietaryName]
		--,[ProprietaryNameSuffix]
		,[StartDate]
		,[EndDate]
		,[Version]
		,[DeleteFlag]
		,[TTLDate]
		,[LastUpdatedOn]
		,[MongoRecordCreatedBy]
		,[RecordCreatedOn]
		,[MongoUpdatedBy]		
		--,[Unit]		
	) ON [PRIMARY]; 
GO

IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_MedicationMap_MongoId') 
	DROP INDEX IX_RPT_MedicationMap_MongoId ON RPT_MedicationMap; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_MedicationMap_MongoId 
	ON RPT_MedicationMap 
	(
		[MongoId]
	--,[FullName]
	--,[SubstanceName]
	,[Route]
	,[Form]
	--,[Strength]
	,[Version]
	,[DeleteFlag]
	,[Custom]
	,[Verified]
	,[TTLDate]
	,[LastUpdatedOn]
	,[MongoRecordCreatedBy]
	,[RecordCreatedOn]
	,[MongoUpdatedBy]		
	) ON [PRIMARY]; 
GO

IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_MedPharmClass_MedMongoId') 
	DROP INDEX IX_RPT_MedPharmClass_MedMongoId ON RPT_MedPharmClass; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_MedPharmClass_MedMongoId
	ON RPT_MedPharmClass ([MedMongoId], [PharmClass]) ON [PRIMARY]; 
GO


/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_MedicationMap_Dim]') AND type in (N'U'))
	DROP TABLE [RPT_Flat_MedicationMap_Dim]
GO

CREATE TABLE [dbo].[RPT_Flat_MedicationMap_Dim](
	[MMId] [int] IDENTITY(1,1) NOT NULL,	
	[MongoId] [varchar](50) NOT NULL,
	[ProductId] [varchar](50) NULL,
	[NDC] [varchar](20) NULL,
	[ProprietaryName] [varchar](300) NULL,
	[ProprietaryNameSuffix] [varchar](200) NULL,
	[StartDate] [varchar](25) NULL,
	[EndDate] [varchar](25) NULL,
	[Unit] [varchar](2000) NULL,
	[FullName] [varchar](300) NULL,
	[SubstanceName] [varchar](3000) NULL,
	[Route] [varchar](200) NULL,
	[Form] [varchar](50) NULL,
	[Strength] [varchar](2000) NULL,
	[Custom] [varchar](10) NULL,
	[Verified] [varchar](10) NULL,
	[PharmClass] [varchar](100) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [VARCHAR](50) NULL,
	[DeleteFlag] [VARCHAR](10) NULL,
	[TTLDate] [DATETIME] NULL,
	[LastUpdatedOn] [DATETIME] NULL,
	[MongoRecordCreatedBy] [VARCHAR](50) NULL,
	[RecordCreatedOn] [DATETIME] NULL,
	[Med_Version] [float] NULL,
	[Med_DeleteFlag] [VARCHAR](10) NULL,
	[Med_TTLDate] [VARCHAR](50) NULL,
	[Med_LastUpdatedOn]  [DATETIME] NULL,
	[Med_MongoRecordCreatedBy]  [VARCHAR](50) NULL,
	[Med_RecordCreatedOn]  [DATETIME] NULL,
	[Med_MongoUpdatedBy] [VARCHAR](50) NULL
)
GO

IF (OBJECT_ID('[spPhy_RPT_Flat_MedicationMap_Dim]') IS NOT NULL)
  DROP PROCEDURE spPhy_RPT_Flat_MedicationMap_Dim
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_MedicationMap_Dim]
AS
BEGIN
	TRUNCATE TABLE RPT_Flat_MedicationMap_Dim
	INSERT INTO RPT_Flat_MedicationMap_Dim
	(
		[MongoId]
		,[ProductId]
		,[NDC]
		,[ProprietaryName]
		,[ProprietaryNameSuffix]
		,[StartDate]
		,[EndDate]
		,[Unit]
		,[FullName]
		,[SubstanceName]
		,[Route]
		,[Form]
		,[Strength]
		,[Version]
		,[DeleteFlag]
		,[Custom]
		,[Verified]
		,[TTLDate]
		,[LastUpdatedOn]
		,[MongoRecordCreatedBy]
		,[RecordCreatedOn]
		,[MongoUpdatedBy]
		,[PharmClass] 
		,[Med_Version]
		,[Med_DeleteFlag]
		,[Med_TTLDate]
		,[Med_LastUpdatedOn]
		,[Med_MongoRecordCreatedBy]
		,[Med_RecordCreatedOn]
		,[Med_MongoUpdatedBy]
	)
	select
		mm.[MongoId]
		,m.[ProductId]
		,m.[NDC]
		,m.[ProprietaryName]
		,m.[ProprietaryNameSuffix]
		,m.[StartDate]
		,m.[EndDate]
		,m.[Unit]
		,mm.[FullName]
		,mm.[SubstanceName]
		,mm.[Route]
		,mm.[Form]
		,mm.[Strength]
		,mm.[Version]
		,mm.[DeleteFlag]
		,mm.[Custom]
		,mm.[Verified]
		,mm.[TTLDate]
		,mm.[LastUpdatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = mm.[MongoRecordCreatedBy]) as [MongoRecordCreatedBy]
		,mm.[RecordCreatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = mm.[MongoUpdatedBy]) as [MongoUpdatedBy]
		,pc.[PharmClass] 
		,m.[Version] AS [Med_Version]
		,m.[DeleteFlag] AS [Med_DeleteFlag]
		,m.[TTLDate] AS [Med_TTLDate]
		,m.[LastUpdatedOn] AS [Med_LastUpdatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = m.[MongoRecordCreatedBy]) as [Med_MongoRecordCreatedBy]
		,m.[RecordCreatedOn] AS [Med_RecordCreatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = m.[MongoUpdatedBy]) as [Med_MongoUpdatedBy]		
	from 
		RPT_MedicationMap as [mm] WITH (NOLOCK) 
		LEFT JOIN RPT_Medication as [m] WITH (NOLOCK) ON mm.MongoId = m.FamilyId
		LEFT OUTER JOIN RPT_MedPharmClass as [pc] WITH (NOLOCK) ON pc.MedMongoId = m.MongoId
	WHERE
		mm.DeleteFlag = 'False'
		AND mm.TTLDate IS NULL
END
GO

/*** [RPT_Flat_PatientMedSup_Dim] ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_PatientMedSup_Dim]') AND type in (N'U'))
	DROP TABLE [RPT_Flat_PatientMedSup_Dim]
GO

CREATE TABLE [RPT_Flat_PatientMedSup_Dim](
 [PMSId] [int] IDENTITY(1,1) NOT NULL	 
,[Name] [VARCHAR](200) NULL
,[MongoId] [VARCHAR](50) NULL
,[MongoPatientId] [VARCHAR](50) NULL
,[Frequency] [VARCHAR](300) NULL
,[MongoFamilyId] [VARCHAR](50) NULL
,[Category] [VARCHAR](200) NULL
,[Type] [VARCHAR](300) NULL
,[Status] [VARCHAR](200) NULL
,[Strength] [VARCHAR](300) NULL
,[Route] [VARCHAR](300) NULL
,[Form] [VARCHAR](300) NULL
,[FreqQuantity] [VARCHAR](200) NULL
,[Source] [VARCHAR](max) NULL
,[StartDate] [VARCHAR](25) NULL
,[EndDate] [VARCHAR](25) NULL
,[Reason] [VARCHAR](5000) NULL
,[Notes] [VARCHAR](5000) NULL
,[PrescribedBy] [VARCHAR](900) NULL
,[SystemName] [VARCHAR](300) NULL
,[UpdatedBy] [VARCHAR](100) NULL
,[LastUpdatedOn] [DATETIME] NULL
,[RecordCreatedBy] [VARCHAR](100) NULL
,[RecordCreatedOn] [DATETIME] NULL
,[Version] [float] NULL
,[TTLDate] [VARCHAR](25) NULL
,[Delete] [VARCHAR](10) NULL
,[PharmClass] [VARCHAR](200) NULL
,[NDC] [VARCHAR](50) NULL
,[Custom] [VARCHAR](50) NULL
,[Verified] [VARCHAR](10) NULL
)
GO

IF (OBJECT_ID('[spPhy_RPT_Flat_PatientMedSup_Dim]') IS NOT NULL)
  DROP PROCEDURE [spPhy_RPT_Flat_PatientMedSup_Dim]
GO

CREATE PROCEDURE [spPhy_RPT_Flat_PatientMedSup_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_Flat_PatientMedSup_Dim]
	INSERT INTO [RPT_Flat_PatientMedSup_Dim]
	(
		[Name]
		,[MongoId]
		,[MongoPatientId]
		,[Frequency]
		,[MongoFamilyId]
		,[Category]
		,[Type]
		,[Status]
		,[Strength]
		,[Route]
		,[Form]
		,[FreqQuantity]
		,[Source]
		,[StartDate]
		,[EndDate]
		,[Reason]
		,[Notes]
		,[PrescribedBy]
		,[SystemName]
		,[UpdatedBy]
		,[LastUpdatedOn]
		,[RecordCreatedBy] 
		,[RecordCreatedOn]
		,[Version]
		,[TTLDate]
		,[Delete]
		,[PharmClass]
		,[NDC]
		,[Custom]
		,[Verified]
	)
	select 
		pm.Name
		,pm.[MongoId]
		,pm.[MongoPatientId]
		, (COALESCE( (select TOP 1 Name from RPT_PatientMedFrequency where MongoId = pm.[MongoFrequencyId]), (select TOP 1  Name from RPT_CustomPatientMedFrequency  where MongoId = pm.[MongoFrequencyId]) )) as [Frequency]
		,pm.[MongoFamilyId]
		,pm.[Category]
		,(select Name from RPT_MedSupTypeLookUp where MongoId = pm.[MongoTypeId]) as [Type]
		,pm.[Status]
		,pm.[Strength]
		,pm.[Route]
		,pm.[Form]
		,pm.[FreqQuantity]
		,(select Name from RPT_AllergySourceLookUp where MongoId = pm.[MongoSourceId]) as [Source]
		,pm.[StartDate]
		,pm.[EndDate]
		,pm.[Reason]
		,pm.[Notes]
		,pm.[PrescribedBy]
		,pm.[SystemName]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = pm.[MongoUpdatedBy])as [UpdatedBy]
		,pm.[LastUpdatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = pm.[MongoRecordCreatedBy]) as [RecordCreatedBy] 
		,pm.[RecordCreatedOn]
		,pm.[Version]
		,pm.[TTLDate]
		,pm.[Delete]
		,pc.PharmClass
		,n.NDC
		,mm.Custom
		, mm.Verified
	from 
		RPT_PatientMedSupp pm
		left outer join RPT_PatientMedSuppNDC n on pm.MongoId = n.MongoPatientMedSuppId
		left outer join RPT_PatientMedSuppPhClass pc on pm.MongoId = pc.MongoPatientMedSuppId
		left outer join RPT_MedicationMap mm on (pm.Name = mm.FullName and pm.Route = mm.Route and pm.Form = mm.Form and pm.Strength = mm.Strength) 
		or (pm.Name = mm.FullName and pm.Route = mm.Route and pm.Form
		 = mm.Form) or (pm.Name = mm.FullName and pm.Route = mm.Route) or (pm.Name = mm.FullName)
	WHERE
		pm.TTLDate IS NULL
		AND pm.[Delete] = 'False'
END
GO

/*** ENG-1149 ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient_ClinicalData]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient_ClinicalData]
GO

CREATE TABLE [dbo].[RPT_Patient_ClinicalData](
	[PatientId] [int] NOT NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[Age] [tinyint] NULL,
	[Gender] [varchar](50) NULL,
	[Priority] [varchar](50) NULL,
	[SystemId] [varchar](50) NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[PatientObservationId] [int] NULL,
	[ObservationType] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[CodingSystem] [varchar](100) NULL,
	[CommonName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Display] [varchar](50) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[NumericValue] [float] NULL,
	[NonNumericValue] [varchar](50) NULL,
	[Source] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Units] [varchar](50) NULL,
	[AdministeredBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](100) NULL,
	[LastUpdatedOn] [datetime2](7) NULL,
	[CreatedBy] [varchar](100) NULL,
	[RecordCreatedOn] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientClinicalData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
AS
BEGIN
	DELETE [RPT_Patient_ClinicalData]
	INSERT INTO [RPT_Patient_ClinicalData]
	(
		 PatientId
		,FirstName				
		,MiddleName				
		,LastName				
		,DateOfBirth
		,Age
		,Gender
		,[Priority]
		,SystemId
		,Assigned_PCM
		,PatientObservationId
        ,ObservationType
		,Code
		,CodingSystem
        ,CommonName
        ,Description
        ,Display 
        ,StartDate
        ,EndDate
        ,NumericValue
        ,NonNumericValue
        ,Source
        ,[State]
        ,[Type]
        ,Units
        ,AdministeredBy
        ,UpdatedBy
        ,LastUpdatedOn 
        ,CreatedBy
        ,RecordCreatedOn
	) 
	SELECT DISTINCT 	
		p.PatientId
		,p.FirstName
		,p.MiddleName
		,p.LastName
		,p.DateOfBirth
		,CASE WHEN p.DATEOFBIRTH != '' AND ISDATE(p.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,p.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age
		,p.Gender
		,p.[Priority]
		,ps.SystemId
		,u.PreferredName as Assigned_PCM
	    ,po.PatientObservationId
        ,otl.Name as ObservationType
		,o.Code
		,csl.Name as CodingSystem
        ,o.CommonName
        ,o.Description
        ,po.Display 
        ,po.StartDate
        ,po.EndDate
        ,po.NumericValue
        ,po.NonNumericValue
        ,po.Source
        ,po.[State]
        ,po.[Type]
        ,po.Units
        ,po.AdministeredBy
        ,u1.PreferredName as UpdatedBy
        ,po.LastUpdatedOn 
        ,u2.PreferredName as CreatedBy
        ,po.RecordCreatedOn
	FROM
		RPT_Patient as p with (nolock) 	
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON p.MongoId = ps.MongoPatientId
		LEFT JOIN RPT_CareMember as c with (nolock) on p.PatientId = c.PatientId
		LEFT JOIN RPT_User as u with (nolock) on c.UserId = u.UserId 
		LEFT JOIN RPT_PatientObservation as po with (nolock) on p.PatientId = po.PatientId and po.[Delete] = 'False' and po.TTLDate IS NULL
		LEFT JOIN RPT_User as u1 with (nolock) on po.UpdatedById = u1.UserId
		LEFT JOIN RPT_User as u2 with (nolock) on po.RecordCreatedById = u2.UserId
		LEFT JOIN RPT_Observation as o with (nolock) on po.ObservationId = o.ObservationId
		LEFT JOIN RPT_ObservationTypeLookUp as otl with (nolock) on o.ObservationTypeId = otl.ObservationTypeId
		LEFT JOIN RPT_CodingSystemLookUp as csl with (nolock) on o.CodingSystemId = csl.MongoId
	WHERE
		p.[Delete] = 'False' and p.TTLDate IS NULL
END
GO	

/*** ENG-1255 ***/
/*** sproc automation ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_SprocNames')
	DROP TABLE RPT_SprocNames;
GO

CREATE TABLE RPT_SprocNames
	(
	id int NOT NULL IDENTITY (1, 1),
	SprocName varchar(100) NOT NULL,
	Prerequire bit NOT NULL,
	Description varchar(2000) NULL
	)  ON [PRIMARY]
GO

INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_ProgramResponse_Flat', 'true');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_PatientInformation', 'true');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_BSHSI_HW2', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_CareBridge', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Engage', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Observations_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_TouchPoint_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_SavePatientInfo', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Program_Details_By_Individual_Healthy_Weight', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2', 'false');
GO

/***** spPhy_RPT_Execute_Sproc ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Execute_Sproc')
	DROP PROCEDURE [spPhy_RPT_Execute_Sproc];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Execute_Sproc]
(	
	@Sproc VARCHAR(2000)
)
AS
BEGIN                    
	DECLARE @StartTime Datetime;
	DECLARE @Sql VARCHAR(2000);
	
	SET @StartTime = GETDATE();
	
	SET @Sql = N'EXECUTE ['+ @Sproc +'];'
	EXEC(@Sql);
	
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES (@Sproc, @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));
END
GO

/***** [spPhy_RPT_Load_Controller] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Load_Controller')
	DROP PROCEDURE [dbo].[spPhy_RPT_Load_Controller];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Load_Controller]
AS
BEGIN                    

	/***** Cursor to run through prereq sproc list  ******/
	DECLARE @Cursor CURSOR;
	DECLARE @SprocName VARCHAR(200);
	BEGIN
		SET @Cursor = CURSOR FOR
		select SprocName from RPT_SprocNames where Prerequire = 'true'      

		OPEN @Cursor 
		FETCH NEXT FROM @Cursor 
		INTO @SprocName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			Print @SprocName
			EXEC [spPhy_RPT_Execute_Sproc] @Sproc = @SprocName;
		  FETCH NEXT FROM @Cursor 
		  INTO @SprocName 
		END; 

		CLOSE @Cursor ;
		DEALLOCATE @Cursor;
	END;	
	
	/***** Cursor to run through sproc list  ******/
	DECLARE @sCursor CURSOR;
	DECLARE @Sproc VARCHAR(200);
	BEGIN
		SET @sCursor = CURSOR FOR
		select SprocName from RPT_SprocNames where Prerequire = 'false'      

		OPEN @sCursor 
		FETCH NEXT FROM @sCursor 
		INTO @Sproc

		WHILE @@FETCH_STATUS = 0
		BEGIN
			Print @Sproc
			EXEC [spPhy_RPT_Execute_Sproc] @Sproc = @Sproc;
			
		  FETCH NEXT FROM @sCursor 
		  INTO @Sproc 
		END; 

		CLOSE @sCursor ;
		DEALLOCATE @sCursor;
	END;	


END
GO


/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/15/2015 13:34:55 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Initialize_Flat_Tables')
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXEC [spPhy_RPT_Load_Controller];
END
GO


/*** [RPT_PatientInformation] ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_PatientInformation')
	DROP TABLE [RPT_PatientInformation]
GO

CREATE TABLE [dbo].[RPT_PatientInformation](
		 [PatientId] [int] NOT NULL
		, [MongoId] [varchar](100) NOT NULL
		, [firstName] [varchar](100) NULL
		, [LastName] [varchar](100) NULL
		, [MiddleName] [varchar](100) NULL
		, [Suffix] [varchar](100) NULL
		, [DateOfBirth] [varchar](100) NULL
		, [AGE] [INT] NULL
		, [Gender]	[varchar](100) NULL
		, [Priority] [varchar](100) NULL
		,[SystemId] [varchar](100) NULL
		,[SystemName] [varchar](100) NULL
		,[TimeZone] [varchar](100) NULL
		,[Phone_1] [varchar](100) NULL
		,[Phone_2] [varchar](100) NULL
		,[Email_1] [varchar](100) NULL
		,[Email_1_Preferred] [varchar](100) NULL
		,[Email_1_Type] [varchar](100) NULL
		,[Address_1] [varchar](100) NULL
		,[Address_2] [varchar](100) NULL
		,[Address_3] [varchar](100) NULL
		,[Address_City] [varchar](100) NULL
		,[Address_State] [varchar](100) NULL
		,[Address_ZIP_Code] [varchar](100) NULL
		,[Assigned_PCM] [varchar](100) NULL
		,[LSSN]	[varchar](100) NULL
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_PatientInformation')
	DROP PROCEDURE [spPhy_RPT_PatientInformation];
GO

CREATE PROCEDURE [spPhy_RPT_PatientInformation]
AS
	DELETE [RPT_PatientInformation]
	INSERT INTO [RPT_PatientInformation]
	(
		 [PatientId]
		, [MongoId]
		, [firstName] 
		, [LastName]
		, [MiddleName]
		, [Suffix]
		, [DateOfBirth]
		, [AGE]
		, [Gender]	
		, [Priority]			
		,[SystemId]
		,[SystemName]
		,[TimeZone]
		,[Phone_1]
		,[Phone_2]
		,[Email_1]
		,[Email_1_Preferred]	
		,[Email_1_Type]
		,[Address_1]
		,[Address_2]
		,[Address_3]
		,[Address_City]
		,[Address_State]
		,[Address_ZIP_Code]
		,[Assigned_PCM]
		,[LSSN]	
	)
	SELECT 
		pt.PatientId
		, pt.MongoId
		, pt.firstName 
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.DateOfBirth
		, (CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END) AS [AGE]
		, pt.Gender	
		, pt.[Priority]			
		, (SELECT TOP 1 ps.SystemId FROM RPT_PatientSystem ps with (nolock) WHERE ps.MongoPatientId = pt.MongoId) as SystemId
		, (SELECT TOP 1 ps.SystemName FROM RPT_PatientSystem ps with (nolock) WHERE ps.MongoPatientId = pt.MongoId) as SystemName
		, (SELECT TOP 1 tz.Name FROM RPTMongoTimeZoneLookUp AS tz with (nolock) WHERE tz.MongoId = c.MongoTimeZone) AS [TimeZone]
		, (SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp with (nolock) WHERE cp.MongoContactId = c.MongoId AND cp.OptOut != 'True' AND cp.[Delete] = 'false') as [Phone_1]
		, (SELECT TOP 1
				CASE WHEN (SELECT COUNT(*) from RPT_ContactPhone cp with (nolock) where cp.MongoContactId = c.MongoId) > 1
				THEN
					 t.Number
				ELSE
					NULL
				END 		 
			FROM ( SELECT TOP 2 d.PhoneId, d.Number, d.contactId FROM (SELECT cp.PhoneId, cp.Number, cp.ContactId, cp.PhonePreferred from RPT_ContactPhone cp with (nolock) where cp.MongoContactId = c.MongoId AND cp.OptOut != 'True' AND cp.[Delete] = 'false') as d ORDER BY d.PhoneId DESC) as t) as [Phone_2]
		,(SELECT  TOP 1   ce.[Text] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId)) as [Email_1]
		,(SELECT  TOP 1   ce.[Preferred] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId) ORDER BY ce.Preferred DESC ) as [Email_1_Preferred]	
		,(SELECT  TOP 1 (SELECT TOP 1 Name FROM dbo.RPT_CommTypeLookUp AS t WITH (nolock) WHERE (t.MongoId = ce.MongoCommTypeId)) AS [Type] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId) ORDER BY ce.Preferred DESC ) as [Email_1_Type]
		,(SELECT TOP 1 Line1 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_1]
		,(SELECT TOP 1 Line2 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_2]
		,(SELECT TOP 1 Line3 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_3]
		,(SELECT TOP 1 City FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_City]
		,(SELECT TOP 1 (SELECT Code FROM dbo.RPT_StateLookUp AS st WITH (nolock) WHERE (st.MongoId = ca.MongoStateId)) AS [State] FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_State]
		,(SELECT TOP 1 PostalCode FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_ZIP_Code]
		,(SELECT     
			  (SELECT (SELECT u.PreferredName
						FROM dbo.RPT_CareMember AS cm WITH (nolock) 
							INNER JOIN dbo.RPT_User AS u ON cm.MongoUserId = u.MongoId
						WHERE     (cm.CareMemberId = c.CareMemberId)) AS [preferred name]
				FROM dbo.RPT_CareMember AS c WITH (nolock)
				WHERE (c.MongoPatientId = ptn.MongoId)) AS [pref_name]
		  FROM dbo.RPT_Patient AS [ptn] WITH (nolock)
		  WHERE (ptn.[Delete] = 'False') AND ptn.MongoId = pt.MongoId) as [Assigned_PCM]
		, pt.LSSN			
	FROM 
		RPT_Patient pt with (nolock) 
		LEFT JOIN RPT_Contact c with (nolock) ON c.MongoPatientId = pt.MongoId
	WHERE 
		pt.[Delete] = 'False'
		AND pt.[TTLDate] IS NULL
GO


/******* [RPT_Program_Details_By_Individual_Healthy_Weight2] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight2')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

CREATE TABLE [RPT_Program_Details_By_Individual_Healthy_Weight2]
(
[PatientId] [int] NOT NULL,
[MongoPatientId] [VARCHAR](50) NOT NULL,
[PatientProgramId] [int] NOT NULL,
[MongoPatientProgramId] [VARCHAR](50) NOT NULL,
[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
[Provider_Name] [varchar](5000) NULL,
[Pre_Survey_Date_Administered] [datetime] NULL,
[Post_Survey_Date_Administered] [datetime] NULL,
[Enrollment_General_Comments] [varchar](5000) NULL,
[Disenrolled_General_Comments] [varchar](5000) NULL,
[Re_enrollment_Reason] [varchar](5000) NULL,
[Program_Completed_General_Comments] [varchar](5000) NULL,
[PHQ2_Total_Point_Score] [varchar](1000) NULL,
[Other_Referral_Information_Depression] [varchar](1000) NULL,
[Depression_Screening_General_Comments] [varchar](5000) NULL,
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight2]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight2]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Enrollment_General_Comments],
		[Disenrolled_General_Comments],
		[Re_enrollment_Reason],
		[Program_Completed_General_Comments],
		[PHQ2_Total_Point_Score],
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca09ac80d31390000001' ) ) AS [Do_you_currently_have_a_PCP] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca4aac80d31390000002'  ) ) AS [Provider_Name]  --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4ea64ac80d30e00000021' , '53f4c273ac80d30e00000009' )) AS [Pre_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4fd75ac80d30e00000083', '53f4f8a0ac80d30e02000073'  ) )	AS [Enrollment_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57115ac80d31203000014', '53f4f8a0ac80d30e02000073') ) AS [Disenrolled_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f5720bac80d3120300001c') ) AS [Re_enrollment_Reason] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57383ac80d31203000033', '53f4f8a0ac80d30e02000073' ) ) AS [Program_Completed_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '540694b2ac80d330cb000010' ) ) AS [PHQ2_Total_Point_Score] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406956aac80d330c8000014') ) AS [Other_Referral_Information_Depression] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406957eac80d330cb000011') ) AS [Depression_Screening_General_Comments] --*
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO


/******* [RPT_Program_Details_By_Individual_Healthy_Weight] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
(
[PatientId] [int] NOT NULL,
[MongoPatientId] [VARCHAR](50) NOT NULL,
[PatientProgramId] [int] NOT NULL,
[MongoPatientProgramId] [VARCHAR](50) NOT NULL,
[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
[Provider_Name] [varchar](5000) NULL,
[Pre_Survey_Date_Administered] [datetime] NULL,
[Post_Survey_Date_Administered] [datetime] NULL,
[Pending_Enrollment_Referral_Date] [datetime] NULL,
[Enrolled_Date] [datetime] NULL,
[Market] [varchar](1000) NULL,
[Date_did_not_enroll] [varchar](1000) NULL,
[Date_did_not_enroll_Reason] [varchar](5000) NULL,
[Enrollment_General_Comments] [varchar](5000) NULL,
[Disenrolled_Date] [datetime] NULL,
[Disenrolled_Reason] [varchar](5000) NULL,
[Disenrolled_General_Comments] [varchar](5000) NULL,
[Re_enrollment_Date] [datetime] NULL,
[Re_enrollment_Reason] [varchar](5000) NULL,
[Program_Completed_Date] [datetime] NULL,
[Program_Completed_General_Comments] [varchar](5000) NULL,
[Risk_Level] [varchar](1000) NULL,
[Acuity_Level] [varchar](1000) NULL,
[PHQ2_Total_Point_Score] [varchar](1000) NULL,
[Referral_Provided_Depression_EAP] [varchar](1000) NULL,
[Referral_Provided_Depression_Community_Resources] [varchar](1000) NULL,
[Referral_Provided_Depression_Participant_Declined] [varchar](1000) NULL,
[Other_Referral_Information_Depression] [varchar](1000) NULL,
[Depression_Screening_General_Comments] [varchar](5000) NULL,
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight];
GO
CREATE PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '5330920da38116ac180009d2';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Pending_Enrollment_Referral_Date],
		[Enrolled_Date],
		[Market],
		[Date_did_not_enroll],
		[Date_did_not_enroll_Reason],
		[Enrollment_General_Comments],
		[Disenrolled_Date],
		[Disenrolled_Reason],
		[Disenrolled_General_Comments],
		[Re_enrollment_Date],
		[Re_enrollment_Reason],
		[Program_Completed_Date],
		[Program_Completed_General_Comments],
		[Risk_Level],
		[Acuity_Level],
		[PHQ2_Total_Point_Score],
		[Referral_Provided_Depression_EAP],
		[Referral_Provided_Depression_Community_Resources] ,
		[Referral_Provided_Depression_Participant_Declined] ,
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '5330920da38116ac180009d2';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fb32c34786626c000029' ) ) AS [Do_you_currently_have_a_PCP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fff2c34786626c00002a'  ) ) AS [Provider_Name] 
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531a304bc347860424000109' , '531a2ec7c347860424000023' )) AS [Pre_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446' , '532c3de1f8efe368860003b2'  )) AS [Pending_Enrollment_Referral_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e1ef8efe368860003b3')) AS [Enrolled_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e76c347865db8000001')) AS [Market]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3f40c347865db8000002')) AS Date_did_not_enroll
	,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c45bff8efe36886000446', '532c3fc2f8efe368860003b5'))	AS [Date_did_not_enroll_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '530f844ef8efe307660001ab'  ) )	AS [Enrollment_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '532c4061f8efe368860003b6')) AS [Disenrolled_Date]
	,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c46b3c347865db8000092', '532c407fc347865db8000003') ) AS [Disenrolled_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '530f844ef8efe307660001ab') ) AS [Disenrolled_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40d0f8efe368860003b7')) AS [Re_enrollment_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40f3f8efe368860003b8') ) AS [Re_enrollment_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '532c411bf8efe368860003b9')) AS [Program_Completed_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '530f844ef8efe307660001ab' ) ) AS [Program_Completed_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50d60c34786662e000001'  ) ) AS [Risk_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50dc3c34786662e000002') ) AS [Acuity_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54de1c34786660a0000de') ) AS [PHQ2_Total_Point_Score]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54e8cc34786660a0000e0') ) AS [Other_Referral_Information_Depression]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '530f844ef8efe307660001ab') ) AS [Depression_Screening_General_Comments]
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO


/****** [spPhy_RPT_Flat_BSHSI_HW2] *******/
ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
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
		did_not_enroll_reason,
		Risk_Level,
		Acuity_Frequency
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
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			FROM fn_RPT_GetText_SingleSelect(pt.PatientId,ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fb39ac80d30e00000067') ) as [Market]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56eb7ac80d31203000001')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56f10ac80d31200000001') ) as [Disenroll_Reason]	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fc71ac80d30e02000074') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f885ac80d30e00000065')) as [did_not_enroll_reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f6ce5bac80d3138d000022', '53f6cc01ac80d3139000000d') ) as [Risk_Level]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f6ce5bac80d3138d000022', '53f6cc4cac80d3138d000012') )as [Acuity_Frequency]
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

/*** ENG-1288 ***/
/**** [fn_RPT_ActionCompleted_Selected] ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Selected]'))
	DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Selected]
GO

CREATE FUNCTION [fn_RPT_ActionCompleted_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(100)	
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			(CASE WHEN p.[Delete] = 'False' THEN	
				CASE 
					WHEN p.[Value] = 'false' THEN
						'No'
					WHEN p.[Value] = '' THEN
						'No'						
					WHEN p.[Value] = 'true' THEN
						'Yes'
					ELSE
						NULL
				END						
			ELSE 						
				'0'
			END) AS [Value]
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
			p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.[Text] = @Text
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid	
	RETURN
END
GO

/**** [fn_RPT_ActionNotCompleted_Selected] ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Selected]'))
	DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Selected]
GO

CREATE FUNCTION [fn_RPT_ActionNotCompleted_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(100)	
)
RETURNS @ActionNotCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionNotCompletedTable
		SELECT 
			'0' as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.StepCompleted = 'False'
			AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.[Text] = @Text
			AND p.patientprogramid = @patientprogramid	
	RETURN
END
GO

/**** [fn_RPT_ActionSaved_Selected] ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Selected]'))
	DROP FUNCTION [fn_RPT_ActionSaved_Selected]
GO

CREATE FUNCTION [fn_RPT_ActionSaved_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(100)	
)
RETURNS @ActionSavedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionSavedTable
		SELECT 
			CASE WHEN p.[Selected] = 'True' THEN
				CASE 
					WHEN p.[Value] = 'false' THEN
						'No'
					WHEN p.[Value] = '' THEN
						'No'								
					WHEN p.[Value] = 'true' THEN
						'Yes'
					ELSE
						NULL
				END						
			ELSE
				'0'
			END
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.[Delete] = 'True'
			
			AND p.[Text] = @Text
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid	
	RETURN
END
GO

/*** [fn_RPT_GetText_Selected] ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText_Selected]'))
	DROP FUNCTION [fn_RPT_GetText_Selected]
GO

CREATE FUNCTION [dbo].[fn_RPT_GetText_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text) where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' THEN
						CASE 
							WHEN pr13.[Value] = 'false' THEN
								'No'
							WHEN pr13.[Value] = '' THEN
								'No'
							WHEN pr13.[Value] = 'true' THEN
								'Yes'
							ELSE
								NULL
						END						
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				--AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pr13.[Text] = @Text
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pr14.[Text] = @Text
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO

/****** Object:  Table [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight]    Script Date: 06/22/2015 14:50:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Program_Details_By_Individual_Healthy_Weight]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight](
	[PatientId] [int] NOT NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoPatientProgramId] [varchar](50) NOT NULL,
	[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
	[Provider_Name] [varchar](5000) NULL,
	[Pre_Survey_Date_Administered] [datetime] NULL,
	[Post_Survey_Date_Administered] [datetime] NULL,
	[Pending_Enrollment_Referral_Date] [datetime] NULL,
	[Enrolled_Date] [datetime] NULL,
	[Final_Call_Date] [datetime] NULL,
	[Market] [varchar](1000) NULL,
	[Date_did_not_enroll] [varchar](1000) NULL,
	[Date_did_not_enroll_Reason] [varchar](5000) NULL,
	[Enrollment_General_Comments] [varchar](5000) NULL,
	[Disenrolled_Date] [datetime] NULL,
	[Disenrolled_Reason] [varchar](5000) NULL,
	[Disenrolled_General_Comments] [varchar](5000) NULL,
	[Re_enrollment_Date] [datetime] NULL,
	[Re_enrollment_Reason] [varchar](5000) NULL,
	[Program_Completed_Date] [datetime] NULL,
	[Program_Completed_General_Comments] [varchar](5000) NULL,
	[Risk_Level] [varchar](1000) NULL,
	[Acuity_Level] [varchar](1000) NULL,
	[PHQ2_Total_Point_Score] [varchar](1000) NULL,
	[Referral_Provided_Depression_EAP] [varchar](1000) NULL,
	[Referral_Provided_Depression_Community_Resources] [varchar](1000) NULL,
	[Referral_Provided_Depression_Participant_Declined] [varchar](1000) NULL,
	[Referral_Provided_Depression_Not_Applicable] [varchar](1000) NULL,
	[Referral_Provided_Depression_Other] [varchar](1000) NULL,		
	[Other_Referral_Information_Depression] [varchar](1000) NULL,
	[Depression_Screening_General_Comments] [varchar](5000) NULL
) ON [PRIMARY]

GO

/*** [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight] ***/
IF (OBJECT_ID('[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]') IS NOT NULL)
  DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '5330920da38116ac180009d2';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Pending_Enrollment_Referral_Date],
		[Enrolled_Date],
		[Final_Call_Date],
		[Market],
		[Date_did_not_enroll],
		[Date_did_not_enroll_Reason],
		[Enrollment_General_Comments],
		[Disenrolled_Date],
		[Disenrolled_Reason],
		[Disenrolled_General_Comments],
		[Re_enrollment_Date],
		[Re_enrollment_Reason],
		[Program_Completed_Date],
		[Program_Completed_General_Comments],
		[Risk_Level],
		[Acuity_Level],
		[PHQ2_Total_Point_Score],
		[Referral_Provided_Depression_EAP],
		[Referral_Provided_Depression_Community_Resources] ,
		[Referral_Provided_Depression_Participant_Declined] ,
		[Referral_Provided_Depression_Not_Applicable],
		[Referral_Provided_Depression_Other],		
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '5330920da38116ac180009d2';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fb32c34786626c000029' ) ) AS [Do_you_currently_have_a_PCP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fff2c34786626c00002a'  ) ) AS [Provider_Name] 
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531a304bc347860424000109' , '531a2ec7c347860424000023' )) AS [Pre_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446' , '532c3de1f8efe368860003b2'  )) AS [Pending_Enrollment_Referral_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e1ef8efe368860003b3')) AS [Enrolled_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
		from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329c5f3a381166015000074', '530f844ef8efe307660001ab')) as [Final_Call_Date]		
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e76c347865db8000001')) AS [Market]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3f40c347865db8000002')) AS Date_did_not_enroll
	,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c45bff8efe36886000446', '532c3fc2f8efe368860003b5'))	AS [Date_did_not_enroll_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '530f844ef8efe307660001ab'  ) )	AS [Enrollment_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '532c4061f8efe368860003b6')) AS [Disenrolled_Date]
	,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c46b3c347865db8000092', '532c407fc347865db8000003') ) AS [Disenrolled_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '530f844ef8efe307660001ab') ) AS [Disenrolled_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40d0f8efe368860003b7')) AS [Re_enrollment_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40f3f8efe368860003b8') ) AS [Re_enrollment_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '532c411bf8efe368860003b9')) AS [Program_Completed_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '530f844ef8efe307660001ab' ) ) AS [Program_Completed_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50d60c34786662e000001'  ) ) AS [Risk_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50dc3c34786662e000002') ) AS [Acuity_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54de1c34786660a0000de') ) AS [PHQ2_Total_Point_Score]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'EAP') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Community Resources') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Participant Declined') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Not Applicable') )	AS [Referral_Provided_Depression_Not_Applicable]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Other')) AS [Referral_Provided_Depression_Other]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54e8cc34786660a0000e0')) AS [Other_Referral_Information_Depression]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '530f844ef8efe307660001ab') ) AS [Depression_Screening_General_Comments]
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO

/****** Object:  Table [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2]    Script Date: 06/22/2015 15:22:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

CREATE TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2](
	[PatientId] [int] NOT NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoPatientProgramId] [varchar](50) NOT NULL,
	[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
	[Provider_Name] [varchar](5000) NULL,
	[Pre_Survey_Date_Administered] [datetime] NULL,
	[Post_Survey_Date_Administered] [datetime] NULL,
	[Enrollment_General_Comments] [varchar](5000) NULL,
	[Final_Call_Date] [datetime] NULL,
	[Disenrolled_General_Comments] [varchar](5000) NULL,
	[Re_enrollment_Reason] [varchar](5000) NULL,
	[Program_Completed_General_Comments] [varchar](5000) NULL,
	[PHQ2_Total_Point_Score] [varchar](1000) NULL,
	[Referral_Provided_Depression_EAP] [varchar](1000) NULL,
	[Referral_Provided_Depression_Community_Resources] [varchar](1000) NULL,
	[Referral_Provided_Depression_Participant_Declined] [varchar](1000) NULL,
	[Referral_Provided_Depression_Not_Applicable] [varchar](1000) NULL,
	[Referral_Provided_Depression_Other] [varchar](1000) NULL,		
	[Other_Referral_Information_Depression] [varchar](1000) NULL,
	[Depression_Screening_General_Comments] [varchar](5000) NULL
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]    Script Date: 06/22/2015 15:32:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight2]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight2]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Enrollment_General_Comments],
		[Final_Call_Date],
		[Disenrolled_General_Comments],
		[Re_enrollment_Reason],
		[Program_Completed_General_Comments],
		[PHQ2_Total_Point_Score],
		[Referral_Provided_Depression_EAP],
		[Referral_Provided_Depression_Community_Resources],
		[Referral_Provided_Depression_Participant_Declined],
		[Referral_Provided_Depression_Not_Applicable],
		[Referral_Provided_Depression_Other],
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca09ac80d31390000001' ) ) AS [Do_you_currently_have_a_PCP] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca4aac80d31390000002'  ) ) AS [Provider_Name]  --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4ea64ac80d30e00000021' , '53f4c273ac80d30e00000009' )) AS [Pre_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4fd75ac80d30e00000083', '53f4f8a0ac80d30e02000073'  ) )	AS [Enrollment_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
		from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b40dac80d330cb0001af', '540698fdac80d330c800001c')) as [Final_Call_Date]				
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57115ac80d31203000014', '53f4f8a0ac80d30e02000073') ) AS [Disenrolled_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f5720bac80d3120300001c') ) AS [Re_enrollment_Reason] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57383ac80d31203000033', '53f4f8a0ac80d30e02000073' ) ) AS [Program_Completed_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '540694b2ac80d330cb000010' ) ) AS [PHQ2_Total_Point_Score] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END	
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', 'EAP') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Community Resources') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Participant Declined') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Not Applicable') )	AS [Referral_Provided_Depression_Not_Applicable]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Other')) AS [Referral_Provided_Depression_Other]		
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406956aac80d330c8000014') ) AS [Other_Referral_Information_Depression] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406957eac80d330cb000011') ) AS [Depression_Screening_General_Comments] --*
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO

/*** ENG-1289 ***/
ALTER FUNCTION [dbo].[fn_RPT_PCPOther] 
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

	----------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 582;
	--SET @patientprogramid = 38;
	--SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422df97ac80d3356d000004';
	------------------------------------

		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	------
	--select @CompletedCount;
	--select @SavedCount;
	--select @ActionNotComplete;
	------
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) --where [Value] != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) --WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = 'True' THEN
					CASE WHEN pr7.[Delete] = 'False' THEN
						pr7.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				ps7.SourceId= @StepSourceId
				AND pa7.SourceId = @ActionSourceId
				AND pp7.SourceId = @ProgramSourceId
				AND pp7.[Delete] = 'False'
				AND pr7.Selected = 'False'
				AND pa7.[State] IN ('Completed')
				AND pa7.Archived = 'True'
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT 
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							ps8.SourceId = @StepSourceId
							AND pa8.SourceId = @ActionSourceId
							AND pp8.SourceId = @ProgramSourceId
							AND (pp8.[Delete] = 'False')
							AND pa8.[State] IN ('Completed')
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
GO

ALTER FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Delete] = 'False' AND (p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 )THEN
					p.[Value]
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
GO

ALTER FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			p.Value as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.StepCompleted = 'False'
			--AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
Go

/*** fn_RPT_GetValue ***/
ALTER FUNCTION [dbo].[fn_RPT_GetValue] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;

		/******/
		--DECLARE @patientid INT;
		--DECLARE @patientprogramid INT;
		--DECLARE @ProgramSourceId VARCHAR(50);
		--DECLARE @ActionSourceId VARCHAR(50);	
		--DECLARE @StepSourceId VARCHAR(50);
		
		--SET @patientid = 59;
		--SET @patientprogramid = 403;
		--SET @ProgramSourceId = '54b69910ac80d33c2c000032';
		--SET @ActionSourceId = '545c0805ac80d36bd4000089';	
		--SET @StepSourceId = '54264df1890e942ba2000006';
		/******/
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		
		/*****/
		--select @CompletedCount;
		--select @SavedCount;
		--select @ActionNotComplete;
		
		--SELECT TOP 1 [Value] FROM 	
		--	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId); --where Value != '0'	
		--SELECT TOP 1 NULL AS [Value] FROM 	
		--	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId); --WHERE [Value] != '0'					
		/*****/
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) --where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) --WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' THEN
						pr13.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				--AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO