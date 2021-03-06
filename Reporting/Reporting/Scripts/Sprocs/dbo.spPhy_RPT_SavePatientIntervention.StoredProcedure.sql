SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientIntervention] 
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
