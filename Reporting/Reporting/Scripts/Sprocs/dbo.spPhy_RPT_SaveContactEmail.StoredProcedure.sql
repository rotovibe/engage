SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactEmail] 
	@MongoID varchar(50),
	@ContactMongoId varchar(50),
	@TypeMongoId varchar(50),
	@Text varchar(MAX),
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
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	Select @TypeId = CommTypeId From RPT_CommTypeLookUp Where MongoId = @TypeMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_ContactEmail Where MongoId = @MongoID)
	Begin
		Update RPT_ContactEmail
		Set ContactId = @ContactId,
			MongoContactId = @ContactMongoId,
			[Delete] = @Delete,
			Text = @Text,
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
		Insert Into RPT_ContactEmail
		(
			ContactId, 
			MongoContactId, 
			[Delete], 
			[Text], 
			OptOut, 
			Preferred, 
			TypeId, 
			MongoCommTypeId, 
			MongoId, 
			[Version], 
			MongoUpdatedBy, 
			UpdatedById, 
			LastUpdatedOn, 
			MongoRecordCreatedBy, 
			RecordCreatedById, 
			RecordCreatedOn,
			TTLDate
		) values 
		(
			@ContactId, 
			@ContactMongoId, 
			@Delete, 
			@Text, 
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
