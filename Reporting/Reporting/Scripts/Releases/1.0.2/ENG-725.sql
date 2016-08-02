IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalMetrics]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientGoalMetrics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RPT_PatientGoalMetrics](
	[MongoPatientId] [varchar](50) NULL,
	[PatientGoalId] [int] NOT NULL,
	[MongoId] [varchar](50) NULL,
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
	[PatientGoalTemplateId] [varchar](50) NULL,
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
	[PatientTaskTemplateId] [varchar](50) NULL,
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
	[PatientInterventionTemplateId] [varchar](50) NULL,
	[PatientInterventionAssignedTo] [varchar](100) NULL,
	[PatientInterventionBarrierName] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientGoalMetrics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
AS
BEGIN
	DELETE [RPT_PatientGoalMetrics]
	INSERT INTO [RPT_PatientGoalMetrics]
	(
		 MongoPatientId
		,PatientGoalId
		,MongoId
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
		,PatientGoalTemplateId
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
		,PatientTaskTemplateId
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
		,PatientInterventionTemplateId
		,PatientInterventionAssignedTo
		,PatientInterventionBarrierName 	
	) 
SELECT DISTINCT 	
		   pg.MongoPatientId
		  ,pg.PatientGoalId
		  ,pg.MongoId
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
		  ,pg.TemplateId as PatientGoalTemplateId
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
		  ,pt.TemplateId as PatientTaskTemplateId
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
		  ,pi.TemplateId as PatientInterventionTemplateId
		  ,u1.PreferredName as PatientInterventionAssignedTo
		  ,pb2.Name as PatientInterventionBarrierName 	
	FROM
		  RPT_PatientGoal as pg with (nolock) 	
		  left join RPT_SourceLookUp as pgl with (nolock) on pg.Source = pgl.MongoId
		  left join RPT_PatientGoalFocusArea as pgf with (nolock) on pg.MongoId = pgf.MongoPatientGoalId
		  left join RPT_FocusAreaLookUp as pgfa with (nolock) on pgf.MongoFocusAreaId = pgfa.MongoId
		  left join RPT_PatientGoalProgram as pgp with (nolock) on pg.MongoId = pgp.MongoPatientGoalId
		  left join RPT_PatientProgram as pp1 with (nolock) on pgp.MongoId = pp1.MongoId
		  left join RPT_PatientGoalAttribute as pga with (nolock) on pg.MongoId = pga.MongoPatientGoalId
		  left join RPT_GoalAttribute as ga with (nolock) on pga.MongoGoalAttributeId = ga.MongoId
		  left join RPT_PatientGoalAttributeValue as pgav with (nolock) on pga.PatientGoalAttributeId = pgav.PatientGoalAttributeId
		  left join RPT_GoalAttributeOption as gao with (nolock) on pgav.Value = gao.[Key] and ga.MongoId = gao.MongoGoalAttributeId
		  left join RPT_PatientBarrier as pb with (nolock) on pg.MongoId = pb.MongoPatientGoalId  and pb.[Delete] = 'False' and pb.TTLDate IS NULL 
		  left join RPT_BarrierCategoryLookUp as bcl with (nolock) on pb.MongoCategoryLookUpId = bcl.MongoId
		  left join RPT_PatientTask as pt with (nolock) on pg.MongoId = pt.MongoPatientGoalId and pt.[Delete] = 'False' and pt.TTLDate IS NULL 
		  left join RPT_PatientTaskBarrier as ptb with (nolock) on pt.MongoId = ptb.MongoPatientTaskId
		  left join RPT_PatientBarrier as pb1 with (nolock) on ptb.MongoPatientBarrierId = pb1.MongoId
		  left join RPT_PatientTaskAttribute as pta with (nolock) on pt.MongoId = pta.MongoPatientTaskId
		  left join RPT_GoalAttribute as ga1 with (nolock) on pta.MongoGoalAttributeId = ga1.MongoId
		  left join RPT_PatientTaskAttributeValue as ptav with (nolock)on pta.PatientTaskAttributeId = ptav.PatientTaskAttributeId
		  left join RPT_GoalAttributeOption as gao1 with (nolock) on ptav.Value = gao1.[Key] and ga1.MongoId = gao1.MongoGoalAttributeId
		  left join RPT_PatientIntervention as pi with (nolock) on pg.MongoId = pi.MongoPatientGoalId and pi.[Delete] = 'False' and pi.TTLDate IS NULL 
		  left join RPT_InterventionCategoryLookUp as icl with (nolock) on pi.MongoCategoryLookUpId = icl.MongoId
		  left join RPT_User as u1 with (nolock)on pi.MongoContactUserId = u1.MongoId
		  left join RPT_PatientInterventionBarrier as pib with (nolock) on pi.MongoId = pib.MongoPatientInterventionId
		  left join RPT_PatientBarrier as pb2 with (nolock) on pib.MongoPatientBarrierId = pb2.MongoId
	WHERE
		pg.[Delete] = 'False' and pg.TTLDate IS NULL 
END
GO




