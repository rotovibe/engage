SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactAddress] 
	@MongoID varchar(50),
	@ContactMongoId varchar(50),
	@TypeMongoId varchar(50),
	@Line1 varchar(MAX),
	@Line2 varchar(MAX),
	@Line3 varchar(MAX),
	@City varchar(MAX),
	@StateMongoId varchar(50),
	@PostalCode varchar(50),
	@Preferred varchar(50),
	@OptOut varchar(50),
	@Delete varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@TTLDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ContactId INT,
			@TypeId INT,
			@StateId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	Select @TypeId = CommTypeId From RPT_CommTypeLookUp Where MongoId = @TypeMongoId
	Select @StateId = StateId From RPT_StateLookUp Where MongoId = @StateMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_ContactAddress Where MongoId = @MongoID)
	Begin
		Update RPT_ContactAddress
		Set ContactId = @ContactId,
			MongoContactId = @ContactMongoId,
			[Delete] = @Delete,
			Line1 = @Line1,
			Line2 = @Line2,
			Line3 = @Line3,
			City = @City,
			StateId = @StateId,
			MongoStateId = @StateMongoId,
			PostalCode = @PostalCode,
			OptOut = @OptOut,
			Preferred = @Preferred,
			TypeId = @TypeId,
			MongoCommTypeId = @TypeMongoId,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			TTLDate = @TTLDate
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_ContactAddress
		(
			ContactId, 
			MongoContactId, 
			[Delete], 
			Line1, 
			Line2, 
			Line3, 
			City, 
			StateId, 
			MongoStateId, 
			PostalCode, 
			OptOut, 
			Preferred, 
			TypeId, 
			MongoCommTypeId, 
			MongoId, 
			[Version], 
			MongoUpdatedBy, 
			UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, 
			RecordCreatedOn,
			TTLDate) 
		values 
		(
			@ContactId, 
			@ContactMongoId, 
			@Delete, 
			@Line1, 
			@Line2, 
			@Line3, 
			@City, 
			@StateId, 
			@StateMongoId, 
			@PostalCode, 
			@OptOut, 
			@Preferred, 
			@TypeId, 
			@TypeMongoId, 
			@MongoID, 
			@Version, 
			@UpdatedBy, 
			@UpdatedById, 
			@LastUpdatedOn, 
			@RecordCreatedBy, 
			@RecordCreatedById, 
			@RecordCreatedOn, 
			@TTLDate
		)
	End
END
GO
