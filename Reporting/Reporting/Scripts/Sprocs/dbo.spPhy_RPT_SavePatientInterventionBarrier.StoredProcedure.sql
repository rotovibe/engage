SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientInterventionBarrier] 
	@PatientBarrierMongoId varchar(50),
	@PatientInterventionMongoId varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@UpdatedBy varchar(50),
	@Version float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientInterventionId INT,
			@PatientBarrierId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientInterventionId = PatientInterventionId From RPT_PatientIntervention Where MongoId = @PatientInterventionMongoId
	Select @PatientBarrierId = PatientBarrierId From RPT_PatientBarrier Where MongoId = @PatientBarrierMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientInterventionBarrier Where MongoPatientInterventionId = @PatientInterventionMongoId AND MongoPatientBarrierId = @PatientBarrierMongoId)
	Begin
		Update RPT_PatientInterventionBarrier
		Set PatientInterventionId = @PatientInterventionId,
			PatientBarrierId = @PatientBarrierId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			LastUpdatedOn = @LastUpdatedOn
		Where MongoPatientInterventionId = @PatientInterventionMongoId AND MongoPatientBarrierId = @PatientBarrierMongoId
		
	End
	Else
	Begin
		Insert Into RPT_PatientInterventionBarrier(PatientInterventionId, MongoPatientInterventionId, PatientBarrierId, MongoPatientBarrierId, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, MongoUpdatedBy, UpdatedById, [Version]) values 
		(@PatientInterventionId, @PatientInterventionMongoId, @PatientBarrierId, @PatientBarrierMongoId, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @UpdatedBy, @UpdatedById, @Version)
	End
END
GO
