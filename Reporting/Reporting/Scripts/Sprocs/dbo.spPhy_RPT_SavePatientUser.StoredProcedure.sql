SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientUser] 
	@MongoId varchar(50),
	@MongoPatientId varchar(50),
	@MongoContactId varchar(50),
	@Flag varchar(50),
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@Delete varchar(50),
	@TTLDate datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@ContactId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId From RPT_Patient Where MongoId = @MongoPatientId
	
	Select @ContactId = UserId From [RPT_User] Where MongoId = @MongoContactId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientUser Where MongoId = @MongoId)
	Begin
		Update RPT_PatientUser
		Set 
			ContactId = @ContactId,
			MongoContactId = @MongoContactId,
			[Delete] = @Delete,
			Flag = @Flag,
			LastUpdatedOn = @LastUpdatedOn,
			PatientId = @PatientId,
			MongoPatientId = @MongoPatientId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert Into RPT_PatientUser(
			ContactId, 
			MongoContactId, 
			[Delete], 
			Flag, 
			LastUpdatedOn, 
			PatientId, 
			MongoPatientId, 
			MongoRecordCreatedBy, 
			RecordCreatedById,
			RecordCreatedOn, 
			MongoUpdatedBy, 
			UpdatedById,
			[Version], 
			MongoId,
			TTLDate,
			ExtraElements
			) 
		values (
			@ContactId, 
			@MongoContactId, 
			@Delete, 
			@Flag, 
			@LastUpdatedOn, 
			@PatientId, 
			@MongoPatientId, 
			@RecordCreatedBy,
			@RecordCreatedById, 
			@RecordCreatedOn, 
			@UpdatedBy, 
			@UpdatedById,
			@Version, 
			@MongoId,
			@TTLDate,
			@ExtraElements
			)
	End
END
GO
