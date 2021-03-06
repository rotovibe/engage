SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientGoalAttribute] 
	@MongoPatientGoalId varchar(50),
	@MongoGoalAttributeId varchar(50),
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

	Declare @PatientGoalId INT,
			@GoalAttributeId INT,
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	If Exists(Select PatientGoalId From RPT_PatientGoal Where MongoId = @MongoPatientGoalId)
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @MongoPatientGoalId
	Else
	RETURN
	
	Select @GoalAttributeId = GoalAttributeId From RPT_GoalAttribute Where MongoId = @MongoGoalAttributeId
	
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
	
	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientGoalAttribute Where MongoPatientGoalId = @MongoPatientGoalId AND MongoGoalAttributeId = @MongoGoalAttributeId)
	Begin
		Update RPT_PatientGoalAttribute
		Set 
			PatientGoalId = @PatientGoalId,
			GoalAttributeID = @GoalAttributeId,
			UpdatedById = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			RecordCreatedById = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version
		Where 
			MongoPatientGoalId = @MongoPatientGoalId AND MongoGoalAttributeId = @MongoGoalAttributeId
		
	End
	Else
	Begin
		Insert into RPT_PatientGoalAttribute(
			PatientGoalId, 
			GoalAttributeID, 
			MongoPatientGoalId,
			MongoGoalAttributeId, 
			UpdatedById,
			MongoUpdatedBy, 
			LastUpdatedOn, 
			RecordCreatedById, 
			MongoRecordCreatedBy,
			RecordCreatedOn, 
			[Version]) 
		values (
			@PatientGoalId, 
			@GoalAttributeId, 
			@MongoPatientGoalId, 
			@MongoGoalAttributeId, 
			 @UpdatedBy,
			 @MongoUpdatedBy, 
			 @LastUpdatedOn, 
			 @RecordCreatedBy,
			 @MongoRecordCreatedBy, 
			 @RecordCreatedOn, 
			 @Version
			)
	End
END
GO
