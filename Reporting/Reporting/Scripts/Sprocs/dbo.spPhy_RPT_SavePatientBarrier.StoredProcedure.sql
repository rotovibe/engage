SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientBarrier] 
	@MongoID varchar(50),
	@PatientGoalMongoId varchar(50),
	@MongoCategoryLookUpId varchar(50),
	@Delete varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TimeToLive datetime,
	@UpdatedBy varchar(50),
	@Version float,
	@StartDate datetime,
	@Name varchar(500),
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@RecordCreatedById INT,
			@UpdatedById INT,
			@CategoryLookUpId INT
			
	Select @PatientGoalId = PatientGoalId from RPT_PatientGoal Where MongoId = @PatientGoalMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId from [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If @MongoCategoryLookUpId != ' '
	Select @CategoryLookUpId = BarrierCategoryId From RPT_BarrierCategoryLookUp Where MongoId = @MongoCategoryLookUpId
	
	If Exists(Select Top 1 1 From RPT_PatientBarrier Where MongoId = @MongoID)
	Begin
		Update RPT_PatientBarrier
		Set [Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @PatientGoalMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TTLDate = @TimeToLive,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			StartDate = @StartDate,
			Name = @Name,
			CategoryLookUpId = @CategoryLookUpId,
			MongoCategoryLookUpId = @MongoCategoryLookUpId,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientBarrier(CategoryLookUpId, MongoCategoryLookUpId, ExtraElements, StartDate, Name, [Delete], LastUpdatedOn, PatientGoalId, MongoPatientGoalId, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Status], StatusDate, TTLDate, MongoUpdatedBy, UpdatedById, [Version], MongoId) values (@CategoryLookUpId, @MongoCategoryLookUpId, @ExtraElements, @StartDate, @Name, @Delete, @LastUpdatedOn, @PatientGoalId, @PatientGoalMongoId, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Status, @StatusDate, @TimeToLive, @UpdatedBy, @UpdatedById, @Version, @MongoID)
	End
END
GO
