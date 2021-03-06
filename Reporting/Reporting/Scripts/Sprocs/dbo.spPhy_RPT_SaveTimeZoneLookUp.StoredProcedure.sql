SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveTimeZoneLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPTMongoTimeZoneLookUp Where MongoId = @MongoID)
	Begin
		Update RPTMongoTimeZoneLookUp
		Set LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPTMongoTimeZoneLookUp(LookUpType, Name, [Default], MongoId) values (@LookUpType, @Name, @Default, @MongoID)
	End
END
GO
