SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContact] 
	@MongoID	varchar(50),
	@PatientMongoId varchar(50),
	@ResourceId varchar(50),
	@FirstName	varchar(100),
	@MiddleName varchar(100),
	@LastName	varchar(100),
	@PreferredName varchar(100),
	@Gender varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Delete varchar(50),
	@TimeZone varchar(50),
	@TTLDate datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @TimeZoneId INT,
			@PatientId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	If (@TimeZone is not null)
	Select @TimeZoneId = TimeZoneId From RPTMongoTimeZoneLookUp Where MongoId = @TimeZone
	
	If (@PatientMongoId is not null)
	Select @PatientId = PatientId From RPT_Patient Where MongoId = @PatientMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_Contact Where MongoId = @MongoID)
	Begin
		Update RPT_Contact
		Set PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			ResourceId = @ResourceId,
			FirstName = @FirstName,
			MiddleName = @MiddleName,
			LastName = @LastName,
			PreferredName = @PreferredName,
			Gender = @Gender,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Delete] = @Delete,
			MongoTimeZone = @TimeZone,
			TimeZone = @TimeZoneId,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_Contact(PatientId, MongoPatientId, ResourceId, FirstName, MiddleName, LastName, PreferredName, Gender, [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Delete], MongoTimeZone, TimeZone, TTLDate, ExtraElements, MongoId) values (@PatientId, @PatientMongoId, @ResourceId, @FirstName, @MiddleName, @LastName, @PreferredName, @Gender, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Delete, @TimeZone, @TimeZoneId, @TTLDate, @ExtraElements, @MongoID)
	End
END
GO
