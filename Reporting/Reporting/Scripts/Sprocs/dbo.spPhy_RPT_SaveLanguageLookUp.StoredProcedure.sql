SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveLanguageLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Code varchar(50),
	@Active varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_LanguageLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_LanguageLookUp
		Set LookUpType = @LookUpType,
			Name = @Name,
			Code = @Code,
			Active = @Active
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_LanguageLookUp(LookUpType, Name, Code, Active, MongoId) values (@LookUpType, @Name, @Code, @Active, @MongoID)
	End
END
GO
