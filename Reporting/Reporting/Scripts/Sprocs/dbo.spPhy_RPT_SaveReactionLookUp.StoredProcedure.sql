SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveReactionLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300),
	@CodeSystem varchar(100),
	@Code varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_ReactionLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_ReactionLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			CodeSystem = @CodeSystem,
			Code = @Code
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_ReactionLookUp
			(LookUpType, 
			Name, 
			CodeSystem, 
			Code, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@CodeSystem, 
			@Code, 
			@MongoID)
	End
END
GO
