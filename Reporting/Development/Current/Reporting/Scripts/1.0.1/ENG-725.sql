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




