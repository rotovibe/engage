SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientGoalProgram] 
	@PatientGoalMongoId varchar(50),
	@ProgramId varchar(50),
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
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	If Exists (Select PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId)
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId
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
	If not Exists(Select Top 1 1 From RPT_PatientGoalProgram Where MongoId = @ProgramId AND MongoPatientGoalId = @PatientGoalMongoId)
	Begin
		Insert into RPT_PatientGoalProgram(PatientGoalId, MongoPatientGoalId, MongoId, MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version]) values (@PatientGoalId, @PatientGoalMongoId, @ProgramId, @MongoUpdatedBy, @UpdatedBy, @LastUpdatedOn, @MongoRecordCreatedBy, @RecordCreatedBy, @RecordCreatedOn, @Version)
	End
END
GO
