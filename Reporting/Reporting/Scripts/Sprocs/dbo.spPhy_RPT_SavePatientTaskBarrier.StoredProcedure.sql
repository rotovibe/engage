SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientTaskBarrier] 
	@PatientBarrierMongoId varchar(50),
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
			@PatientBarrierId INT,
			@RecordCreatedBy INT,
			@UpdatedBy INT
	
	Select @PatientTaskId = PatientTaskId From RPT_PatientTask Where MongoId = @PatientTaskMongoId
	Select @PatientBarrierId = PatientBarrierId From RPT_PatientBarrier Where MongoId = @PatientBarrierMongoId
	
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
	
	If Exists(Select Top 1 1 From RPT_PatientTaskBarrier Where MongoPatientTaskId = @PatientTaskMongoId AND MongoPatientBarrierId = @PatientBarrierMongoId)
	Begin
		Update RPT_PatientTaskBarrier
		Set PatientTaskId = @PatientTaskId,
			PatientBarrierId = @PatientBarrierId,
			RecordCreatedById = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			LastUpdatedOn = @LastUpdatedOn,
			UpdatedById = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			[Version] = @Version
		Where MongoPatientTaskId = @PatientTaskMongoId AND MongoPatientBarrierId = @PatientBarrierMongoId
		
	End
	Else
	Begin
		Insert Into 
			RPT_PatientTaskBarrier
			(
			PatientTaskId,
			MongoPatientTaskId, 
			PatientBarrierId,
			MongoPatientBarrierId,
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
			@PatientBarrierId,
			@PatientBarrierMongoId,
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
