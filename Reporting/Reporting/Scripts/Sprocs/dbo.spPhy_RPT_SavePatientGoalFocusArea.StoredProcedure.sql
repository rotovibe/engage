SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientGoalFocusArea] 
	@PatientGoalMongoId varchar(50),
	@FocusAreaMongoId varchar(50),
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
			@FocusAreaId INT,
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	If Exists (Select PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId)
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId
	Else
	RETURN
	
	Select @FocusAreaId = FocusAreaId From RPT_FocusAreaLookUp Where MongoId = @FocusAreaMongoId
	
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
	If Exists(Select Top 1 1 From RPT_PatientGoalFocusArea Where MongoPatientGoalId = @PatientGoalMongoId AND MongoFocusAreaId = @FocusAreaMongoId)
	Begin
		Update RPT_PatientGoalFocusArea
		Set PatientGoalId = @PatientGoalId,
			FocusAreaId = @FocusAreaId,
			UpdatedById = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			RecordCreatedById = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version
		Where MongoPatientGoalId = @PatientGoalMongoId AND MongoFocusAreaId = @FocusAreaMongoId
		
	End
	Else
	Begin
		Insert into RPT_PatientGoalFocusArea(PatientGoalId, FocusAreaId, MongoPatientGoalId, MongoFocusAreaId, MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version]) values (@PatientGoalId, @FocusAreaId, @PatientGoalMongoId, @FocusAreaMongoId, @MongoUpdatedBy, @UpdatedBy, @LastUpdatedOn, @MongoRecordCreatedBy, @RecordCreatedBy, @RecordCreatedOn, @Version)
	End
END
GO
