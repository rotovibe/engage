SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveUserRecentList] 
	@MongoID varchar(50),
	@UserMongoId varchar(50),
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

	Declare @UserId INT
	
	Select @UserId = UserId From [RPT_User] Where MongoId = @UserMongoId
	
	If Exists(Select Top 1 1 From RPT_UserRecentList Where MongoId = @MongoID)
	Begin
		Update RPT_UserRecentList
		Set UserId = @UserId,
			MongoUpdatedBy = @UpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_UserRecentList(UserId, MongoId, MongoRecordCreatedBy, RecordCreatedOn, MongoUpdatedBy, LastUpdatedOn, [Version]) values (@UserId, @MongoID, @RecordCreatedBy, @RecordCreatedOn, @UpdatedBy, @LastUpdatedOn, @Version)
	End
END
GO
