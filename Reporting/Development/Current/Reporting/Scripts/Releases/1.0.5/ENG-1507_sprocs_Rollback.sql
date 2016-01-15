DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientIntervention]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientIntervention] 
	@MongoID varchar(50),
	@PatientGoalMongoId varchar(50),
	@MongoCategoryLookUpId varchar(50),
	@AssignedTo varchar(50),
	@Delete varchar(50),
	@Description varchar(5000),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@StartDate datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TimeToLive datetime,
	@UpdatedBy varchar(50),
	@Version float,
	@Name varchar(100),
	@ExtraElements varchar(5000),
	@ClosedDate datetime,
	@TemplateId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@CategoryLookUpId INT,
			@RecordCreatedById INT,
			@UpdatedById INT,
			@UserId INT
	
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId
	
	Select @CategoryLookUpId = InterventionCategoryId From RPT_InterventionCategoryLookUp Where MongoId = @MongoCategoryLookUpId
	Select @UserId = UserId From [RPT_User] Where MongoId = @AssignedTo
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientIntervention Where MongoId = @MongoID)
	Begin
		Update RPT_PatientIntervention
		Set AssignedToUserId = @UserId,
			MongoContactUserId = @AssignedTo,
			CategoryLookUpId = @CategoryLookUpId,
			MongoCategoryLookUpId = @MongoCategoryLookUpId,
			[Delete] = @Delete,
			[Description] = @Description,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @PatientGoalMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			StartDate = @StartDate,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TTLDate = @TimeToLive,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			Name = @Name,
			ExtraElements = @ExtraElements,
			ClosedDate = @ClosedDate,
			TemplateId = @TemplateId
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientIntervention(
		Name, 
		ExtraElements, 
		AssignedToUserId, 
		MongoContactUserId, 
		CategoryLookUpId, 
		MongoCategoryLookUpId, 
		[Delete], 
		[Description], 
		LastUpdatedOn, 
		PatientGoalId, 
		MongoPatientGoalId, 
		MongoRecordCreatedBy, 
		RecordCreatedById, 
		RecordCreatedOn, 
		StartDate, 
		[Status], 
		StatusDate, 
		TTLDate, 
		MongoUpdatedBy, 
		UpdatedById, 
		[Version], 
		MongoId, 
		[ClosedDate], 
		[TemplateId]) 
		values 
		(@Name, 
		@ExtraElements, 
		@UserId, 
		@AssignedTo, 
		@CategoryLookUpId, 
		@MongoCategoryLookUpId, 
		@Delete, 
		@Description, 
		@LastUpdatedOn, 
		@PatientGoalId, 
		@PatientGoalMongoId, 
		@RecordCreatedBy, 
		@RecordCreatedById, 
		@RecordCreatedOn, 
		@StartDate, 
		@Status, 
		@StatusDate, 
		@TimeToLive, 
		@UpdatedBy, 
		@UpdatedById, 
		@Version, 
		@MongoID,
		@ClosedDate,
		@TemplateId)
	End
END

GO

DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
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
