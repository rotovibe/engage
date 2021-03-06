SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactTimeOfDay] 
	@ContactMongoId varchar(50),
	@TimeOfDayLookUpMongoId varchar(50),
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

	Declare @TimeOfDayId		INT,
			@ContactId			INT,
			@RecordCreatedById	INT,
			@UpdatedById		INT
	
	Select @TimeOfDayId = TimesOfDayId From RPT_TimesOfDayLookUp Where MongoId = @TimeOfDayLookUpMongoId
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If not Exists(Select Top 1 1 From RPT_ContactTimeOfDay Where TimeOfDayLookUpId = @TimeOfDayId AND ContactId = @ContactId)
	Begin
		Insert Into RPT_ContactTimeOfDay(ContactId, MongoContactId, TimeOfDayLookUpId, MongoTimeOfDayId, [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, TTLDate) values (@ContactId, @ContactMongoId, @TimeOfDayId, @TimeOfDayLookUpMongoId, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @TTLDate)
	End
END
GO
