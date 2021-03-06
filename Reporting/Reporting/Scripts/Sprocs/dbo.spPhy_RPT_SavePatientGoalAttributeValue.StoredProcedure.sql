SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientGoalAttributeValue] 
	@PatientGoalMongoId varchar(50),
	@GoalAttributeMongoId varchar(50),
	@Value varchar(100),
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

	Declare @PatientGoalAttributeId INT,
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	If Exists (Select PatientGoalAttributeId From RPT_PatientGoalAttribute Where MongoPatientGoalId = @PatientGoalMongoId AND MongoGoalAttributeId = @GoalAttributeMongoId) 
	Select @PatientGoalAttributeId = PatientGoalAttributeId From RPT_PatientGoalAttribute Where MongoPatientGoalId = @PatientGoalMongoId AND MongoGoalAttributeId = @GoalAttributeMongoId
	Else
	RETURN
	
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
	If not Exists(Select Top 1 1 From RPT_PatientGoalAttributeValue Where PatientGoalAttributeId = @PatientGoalAttributeId AND Value = @Value)
	Begin
		Insert into RPT_PatientGoalAttributeValue(PatientGoalAttributeId, MongoGoalAttributeId, Value, MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version]) values (@PatientGoalAttributeId, @GoalAttributeMongoId, @Value, @MongoUpdatedBy, @UpdatedBy, @LastUpdatedOn, @MongoRecordCreatedBy, @RecordCreatedBy, @RecordCreatedOn, @Version)
	End
END
GO
