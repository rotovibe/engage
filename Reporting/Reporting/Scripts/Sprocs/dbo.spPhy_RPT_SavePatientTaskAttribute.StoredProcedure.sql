SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientTaskAttribute] 
	@GoalAttributeMongoId varchar(50),
	@PatientTaskMongoId varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientTaskId INT,
			@GoalAttributeId INT,
			@RecordCreatedBy INT,
			@UpdatedBy INT
	
	Select @PatientTaskId = PatientTaskId From RPT_PatientTask Where MongoId = @PatientTaskMongoId
	Select @GoalAttributeId = GoalAttributeId From RPT_GoalAttribute Where MongoId = @GoalAttributeMongoId
	
		-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end	
	
	If Exists(Select Top 1 1 From RPT_PatientTaskAttribute Where MongoPatientTaskId = @PatientTaskMongoId AND MongoGoalAttributeId = @GoalAttributeMongoId)
	Begin
		Update RPT_PatientTaskAttribute
		Set PatientTaskId = @PatientTaskId,
			GoalAttributeId = @GoalAttributeId,
			RecordCreatedById = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			LastUpdatedOn = @LastUpdatedOn,
			UpdatedById = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			[Version] = @Version
		Where MongoPatientTaskId = @PatientTaskMongoId AND MongoGoalAttributeId = @GoalAttributeMongoId
		
	End
	Else
	Begin
		Insert Into 
			RPT_PatientTaskAttribute
			(
			PatientTaskId,
			MongoPatientTaskId, 
			GoalAttributeId,
			MongoGoalAttributeId,
			MongoRecordCreatedBy,
			RecordCreatedById,
			RecordCreatedOn,
			LastUpdatedOn,
			UpdatedById,
			MongoUpdatedBy,
			[Version]
			) 
			values 
			(
			@PatientTaskId,
			@PatientTaskMongoId,
			@GoalAttributeId,
			@GoalAttributeMongoId,
			@MongoRecordCreatedBy,
			@RecordCreatedBy,
			@RecordCreatedOn,
			@LastUpdatedOn,
			@UpdatedBy,
			@MongoUpdatedBy,
			@Version
			)
	End
END
GO
