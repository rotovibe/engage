SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientTask] 
	@MongoId varchar(50),
	@MongoPatientGoalId varchar(50),
	@Name varchar(100),
	@Description varchar(5000),
	@StartDate datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TargetDate datetime,
	@TargetValue varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@TimeToLive datetime,
	@Delete varchar(50),
	@ExtraElements varchar(5000),
	@ClosedDate datetime,
	@TemplateId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@UpdatedBy		INT,
			@RecordCreatedBy INT
	
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @MongoPatientGoalId
	
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
	
	If Exists(Select Top 1 1 From RPT_PatientTask Where MongoId = @MongoId)
	Begin
		Update RPT_PatientTask
		Set Name = @Name,
			Description = @Description,
			[Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @MongoPatientGoalId,
			RecordCreatedBy = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			StartDate = @StartDate,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TargetDate = @TargetDate,
			TargetValue = @TargetValue,
			TTLDate = @TimeToLive,
			UpdatedBy = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			[Version] = @Version,
			ExtraElements = @ExtraElements,
			ClosedDate = @ClosedDate,
			TemplateId = @TemplateId
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert Into 
			RPT_PatientTask
			(
			Name, 
			[Description], 
			[Delete], 
			LastUpdatedOn, 
			PatientGoalId, 
			MongoPatientGoalId, 
			RecordCreatedBy,
			MongoRecordCreatedBy, 
			RecordCreatedOn, 
			StartDate, 
			[Status], 
			StatusDate, 
			TargetDate, 
			TargetValue,
			TTLDate, 
			UpdatedBy, 
			MongoUpdatedBy, 
			[Version], 
			ExtraElements,
			MongoId,
			ClosedDate,
			TemplateId
			) 
			values 
			(
			@Name, 
			@Description, 
			@Delete, 
			@LastUpdatedOn, 
			@PatientGoalId, 
			@MongoPatientGoalId, 
			@RecordCreatedBy,
			@MongoRecordCreatedBy, 
			@RecordCreatedOn, 
			@StartDate, 
			@Status, 
			@StatusDate, 
			@TargetDate,
			@TargetValue, 
			@TimeToLive, 
			@UpdatedBy, 
			@MongoUpdatedBy, 
			@Version, 
			@ExtraElements,
			@MongoId,
			@ClosedDate,
			@TemplateId
			)
	End
END
GO
