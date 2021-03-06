SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactRecentList]
	@MongoID varchar(50),
	@ContactMongoId varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ReturnID	INT,
			@ContactId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_ContactRecentList Where MongoId = @MongoID)
	Begin
		Update RPT_ContactRecentList
		Set ContactId = @ContactId,
			MongoContactId = @ContactMongoId,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn
		Where MongoId = @MongoID
		
		Select @ReturnID = ContactRecentListId From RPT_ContactRecentList Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_ContactRecentList(ContactId, MongoContactId, MongoId, [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn) values (@ContactId, @ContactMongoId, @MongoID, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn)
		Select @ReturnID = @@IDENTITY
	End
	
	Select @ReturnID
END
GO
