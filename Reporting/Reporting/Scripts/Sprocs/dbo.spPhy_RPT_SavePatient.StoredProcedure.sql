SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatient]
	@MongoID	varchar(50),
	@FirstName	varchar(100),
	@MiddleName varchar(100),
	@LastName	varchar(100),
	@PreferredName varchar(100),
	@Suffix varchar(50),
	@DateOfBirth varchar(50),
	@Gender varchar(50),
	@Priority varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Delete varchar(50),
	@BackGround varchar(MAX),
	@FSSN varchar(100),
	@LSSN varchar(50),
	@DisplayPatientSystemMongoId varchar(50),
	@TTLDate datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @DisplayPatientSystemId INT,
			@UpdatedById INT,
			@RecordCreatedById INT
	
	Select @DisplayPatientSystemId = PatientSystemId From RPT_PatientSystem Where MongoId = @DisplayPatientSystemMongoId
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If Exists(Select Top 1 1 From RPT_Patient Where MongoId = @MongoID)
	Begin
		Update RPT_Patient
		Set FirstName = @FirstName,
			MiddleName = @MiddleName,
			LastName = @LastName,
			PreferredName = @PreferredName,
			Suffix = @Suffix,
			DateOfBirth = @DateOfBirth,
			Gender = @Gender,
			Priority = @Priority,
			Version = @Version,
			MongoUpdatedBy = @UpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Delete] = @Delete,
			Background = @BackGround,
			DisplayPatientSystemId = @DisplayPatientSystemId,
			MongoPatientSystemId = @DisplayPatientSystemMongoId,
			UpdatedById = @UpdatedById,
			RecordCreatedById = @RecordCreatedById,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements,
			LSSN = @LSSN,
			FSSN = @FSSN
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_Patient
			(ExtraElements, 
			FirstName, 
			MiddleName, 
			LastName, 
			PreferredName, 
			Suffix, 
			DateOfBirth, 
			Gender, 
			[Priority], 
			[Version], 
			MongoUpdatedBy, 
			UpdatedById, 
			LastUpdatedOn, 
			MongoRecordCreatedBy, 
			RecordCreatedById, 
			RecordCreatedOn, 
			[Delete], 
			Background, 
			TTLDate, 
			MongoId,
			LSSN,
			FSSN)
		values 
			(@ExtraElements, 
			@FirstName, 
			@MiddleName, 
			@LastName, 
			@PreferredName, 
			@Suffix, 
			@DateOfBirth, 
			@Gender, 
			@Priority, 
			@Version, 
			@UpdatedBy, 
			@UpdatedById, 
			@LastUpdatedOn, 
			@RecordCreatedBy, 
			@RecordCreatedById, 
			@RecordCreatedOn, 
			@Delete, 
			@BackGround, 
			@TTLDate, 
			@MongoID,
			@LSSN,
			@FSSN)
	End
END
GO
