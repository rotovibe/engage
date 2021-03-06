SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveContactWeekDay] 
	@ContactMongoId varchar(50),
	@WeekDay INT,
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

	Declare @ContactId			INT,
			@RecordCreatedById	INT,
			@UpdatedById		INT
	
	Select @ContactId = ContactId From RPT_Contact Where MongoId = @ContactMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If not Exists(Select Top 1 1 From RPT_ContactWeekDay Where ContactId = @ContactId AND WeekDay = @WeekDay)
	Begin
		Insert Into RPT_ContactWeekDay
		(
			ContactId, 
			MongoContactId, 
			[WeekDay], 
			[Version], 
			MongoUpdatedBy, 
			UpdatedById, 
			LastUpdatedOn, 
			MongoRecordCreatedBy, 
			RecordCreatedById, 
			RecordCreatedOn,
			TTLDate
		) 
		values 
		(
			@ContactId, 
			@ContactMongoId, 
			@WeekDay, 
			@Version, 
			@UpdatedBy, 
			@UpdatedById, 
			@LastUpdatedOn, 
			@RecordCreatedBy, 
			@RecordCreatedById, 
			@RecordCreatedOn,
			@TTLDate)
	End
END
GO
