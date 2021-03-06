SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactMode]
	@ContactMongoId varchar(50),
	@Preferred varchar(50),
	@OptOut varchar(50),
	@ModeLookUpMongoId varchar(50),
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

	Declare @ModeId	INT,
			@ContactId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ModeId = CommModeId From RPT_CommModeLookUp Where MongoId = @ModeLookUpMongoId
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_ContactMode Where ModeId = @ModeId AND ContactId = @ContactId)
	Begin
		Update RPT_ContactMode
		Set Preferred = @Preferred,
			OptOut = @OptOut,
			MongoContactId = @ContactMongoId,
			MongoCommModeId = @ModeLookUpMongoId,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			TTLDate = @TTLDate
		Where CommModeLookUpId = @ModeId AND ContactId = @ContactId
	End
	Else
	Begin
		Insert Into RPT_ContactMode(ContactId, MongoContactId, Preferred, OptOut, CommModeLookUpId, MongoCommModeId, [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn,
		TTLDate) values (@ContactId, @ContactMongoId, @Preferred, @OptOut, @ModeId, @ModeLookUpMongoId, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @TTLDate)
	End
END
GO
