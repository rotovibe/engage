SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientSystem] 
	@MongoID varchar(50),
	@PatientMongoId varchar(50),
	@Label varchar(50),
	@SystemId varchar(50),
	@SystemName varchar(50),
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ReturnID	INT,
			@PatientId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @PatientMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientSystem Where MongoId = @MongoID)
	Begin
		Update RPT_PatientSystem
		Set [Delete] = @Delete,
			Label = @Label,
			SystemId = @SystemId,
			SystemName = @SystemName,
			LastUpdatedOn = @LastUpdatedOn,
			PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			TTLDate = @TimeToLive,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoID
		
		Select @ReturnID = PatientSystemId From RPT_PatientSystem Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientSystem(ExtraElements, [Delete], Label, SystemId, SystemName, LastUpdatedOn, PatientId, MongoPatientId, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, MongoUpdatedBy, UpdatedById, [Version], TTLDate, MongoId) values (@ExtraElements, @Delete, @Label, @SystemId, @SystemName, @LastUpdatedOn, @PatientId, @PatientMongoId, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @UpdatedBy, @UpdatedById, @Version, @TimeToLive, @MongoID)
		Select @ReturnID = @@IDENTITY
		
	End
	
	Update RPT_Patient
	Set DisplayPatientSystemId = @ReturnID,
		MongoPatientSystemId = @MongoID
	Where MongoId = @PatientMongoId
	
	Select @ReturnID
END
GO
