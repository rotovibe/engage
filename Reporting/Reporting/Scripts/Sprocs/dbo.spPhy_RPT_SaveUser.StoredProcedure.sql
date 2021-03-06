SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveUser] 
	@MongoID			varchar(50),
	@ResourceId			varchar(50),
	@FirstName			varchar(100),
	@LastName			varchar(100),
	@PreferredName		varchar(100),
	@Version			float,
	@UpdatedBy			varchar(50),
	@LastUpdatedOn		datetime,
	@RecordCreatedBy	varchar(50),
	@RecordCreatedOn	datetime,
	@TTLDate			datetime,
	@Delete				varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From [RPT_User] Where MongoId = @MongoID)
	Begin
		Update [RPT_User]
		Set ResourceId = @ResourceId,
			FirstName = @FirstName,
			LastName = @LastName,
			PreferredName = @PreferredName,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			TTLDate = @TTLDate,
			[Delete] = @Delete
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into [RPT_User](ResourceId, FirstName, LastName, PreferredName, [Version], MongoUpdatedBy, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedOn, TTLDate, [Delete], MongoId) values (@ResourceId, @FirstName, @LastName, @PreferredName, @Version, @UpdatedBy, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedOn, @TTLDate, @Delete, @MongoID)
	End
END
GO
