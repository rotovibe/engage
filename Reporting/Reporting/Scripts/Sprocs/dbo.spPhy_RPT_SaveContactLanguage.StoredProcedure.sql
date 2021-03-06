SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactLanguage] 
	@ContactMongoId varchar(50),
	@LanguageLookUpMongoId varchar(50),
	@Preferred varchar(50),
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
			@LanguageId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	Select @LanguageId = LanguageId From RPT_LanguageLookUp Where MongoId = @LanguageLookUpMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_ContactLanguage Where LanguageId = @LanguageId AND ContactId = @ContactId)
	Begin
		Update RPT_ContactLanguage
		Set Preferred = @Preferred,
			MongoContactId = @ContactMongoId, 
			MongoLanguageLookUpId = @LanguageLookUpMongoId,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			TTLDate = @TTLDate
		Where LanguageLookUpId = @LanguageId AND ContactId = @ContactId
	End
	Else
	Begin
		Insert Into RPT_ContactLanguage(ContactId, MongoContactId, Preferred, LanguageLookUpId, MongoLanguageLookUpId, [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn,
		TTLDate) 
		values (@ContactId, @ContactMongoId, @Preferred, @LanguageId, @LanguageLookUpMongoId, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn,
		@TTLDate)
	End
END
GO
