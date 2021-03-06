SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientGoal]
	@MongoId varchar(50),
	@MongoPatientId varchar(50),
	@Name varchar(500),
	@Description varchar(50),
	@StartDate datetime,
	@EndDate datetime,
	@Source varchar(50),
	@Status varchar(50),
	@StatusDate datetime,
	@TargetDate datetime,
	@TargetValue varchar(500),
	@Type varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(5000),
	@TemplateId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @PatientId INT,
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	Select @PatientId = PatientId From RPT_Patient Where MongoId = @MongoPatientId

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
	If Exists(Select Top 1 1 From RPT_PatientGoal Where MongoId = @MongoId)
	Begin
		Update RPT_PatientGoal
		Set Name = @Name,
			StartDate = @StartDate,
			EndDate = @EndDate,
			[Source] = @Source,
			[Status] = @Status,
			[Description] = @Description,
			StatusDate = @StatusDate,
			TargetDate = @TargetDate,
			TargetValue = @TargetValue,
			[Type] = @Type,
			UpdatedBy = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			RecordCreatedBy = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version,
			[Delete] = @Delete,
			TTLDate = @TimeToLive,
			PatientId = @PatientId,
			MongoPatientId = @MongoPatientId,
			ExtraElements = @ExtraElements,
			TemplateId = @TemplateId
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert into RPT_PatientGoal(
			Name, 
			StartDate, 
			EndDate, 
			[Source], 
			[Status], 
			StatusDate, 
			TargetDate, 
			TargetValue, 
			[Type], 
			UpdatedBy,
			MongoUpdatedBy, 
			LastUpdatedOn, 
			RecordCreatedBy, 
			MongoRecordCreatedBy,
			RecordCreatedOn, 
			[Version], 
			[Delete], 
			TTLDate,
			 MongoId, 
			 PatientId,
			 MongoPatientId,
			 [Description],
			 ExtraElements,
			 TemplateId
			 ) 
		 values (
			 @Name, 
			 @StartDate, 
			 @EndDate, 
			 @Source, 
			 @Status, 
			 @StatusDate,
			 @TargetDate, 
			 @TargetValue, 
			 @Type, 
			 @UpdatedBy,
			 @MongoUpdatedBy, 
			 @LastUpdatedOn, 
			 @RecordCreatedBy,
			 @MongoRecordCreatedBy, 
			 @RecordCreatedOn, 
			 @Version, 
			 @Delete, 
			 @TimeToLive, 
			 @MongoId, 
			 @PatientId,
			 @MongoPatientId,
			 @Description,
			 @ExtraElements,
			 @TemplateId
		 )
	End
END
GO
