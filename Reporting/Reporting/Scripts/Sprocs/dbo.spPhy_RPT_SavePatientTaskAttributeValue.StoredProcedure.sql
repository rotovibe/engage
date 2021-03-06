SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientTaskAttributeValue] 
	@GoalAttributeMongoId varchar(50),
	@PatientTaskMongoId varchar(50),
	@Value varchar(50),
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
	
	Declare @PatientTaskAttributeId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
			
	Select @PatientTaskAttributeId = PatientTaskAttributeId From RPT_PatientTaskAttribute Where MongoGoalAttributeId = @GoalAttributeMongoId AND MongoPatientTaskId = @PatientTaskMongoId
	
		-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedById = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedById = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end	
	
	If not Exists(Select Top 1 1 From RPT_PatientTaskAttributeValue Where PatientTaskAttributeId = @PatientTaskAttributeId AND Value = @Value)
	Begin
		Insert Into 
			RPT_PatientTaskAttributeValue
			(
			PatientTaskAttributeId,
			Value,
			MongoRecordCreatedBy,
			RecordCreatedById,
			RecordCreatedOn,
			UpdatedById,
			MongoUpdatedBy,
			LastUpdatedOn,
			[Version]
			) 
			values 
			(
			@PatientTaskAttributeId,
			@Value,
			@MongoRecordCreatedBy,
			@RecordCreatedById,
			@RecordCreatedOn,
			@UpdatedById,
			@MongoUpdatedBy,
			@LastUpdatedOn,
			@Version
			)
	End
END
GO
