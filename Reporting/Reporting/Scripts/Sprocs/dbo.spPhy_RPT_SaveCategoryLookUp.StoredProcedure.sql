SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveCategoryLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPTMongoCategoryLookUp Where MongoId = @MongoID)
	Begin
		Update RPTMongoCategoryLookUp
		Set LookUpType = @LookUpType,
			Name = @Name
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPTMongoCategoryLookUp(LookUpType, Name, MongoId) values (@LookUpType, @Name, @MongoID)
	End
END
GO
