SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveCareMember]
	@MongoID	varchar(50),
	@PatientMongoId	varchar(50),
	@ContactMongoId varchar(50),
	@TypeMongoId varchar(50),
	@Primary varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT
	Select @PatientId = PatientId From RPT_Patient Where MongoId = @PatientMongoId	

	
	Declare @ContactId INT
	If Exists (Select UserId from [RPT_User] Where MongoId = @ContactMongoId)
	Select @ContactId = UserId from [RPT_User] Where MongoId = @ContactMongoId
	Else
	RETURN

	Declare @TypeId INT
	Select @TypeId = CareMemberTypeId From RPT_CareMemberTypeLookUp Where MongoId = @TypeMongoId	
	
	Declare @RecordCreatedById INT
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy	
	
	Declare @UpdatedById INT
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy	

	If Exists(Select Top 1 1 From RPT_CareMember Where MongoId = @MongoID)
	Begin
		Update RPT_CareMember
		Set PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			UserId = @ContactId,
			MongoUserId = @ContactMongoId,
			TypeId = @TypeId,
			[Primary] = @Primary,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Delete] = @Delete,
			TTLDate = @TimeToLive,
			ExtraElements = @ExtraElements,
			MongoCommTypeLookUpId = @TypeMongoId
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_CareMember(PatientId, MongoPatientId, UserId, MongoUserId, TypeId, MongoCommTypeLookUpId, [Primary], [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Delete], TTLDate, ExtraElements, MongoId) values (@PatientId, @PatientMongoId, @ContactId, @ContactMongoId, @TypeId, @TypeMongoId, @Primary, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Delete, @TimeToLive, @ExtraElements, @MongoID)
	End
END
GO
