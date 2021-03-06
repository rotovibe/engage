SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveAllergySourceLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_AllergySourceLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_AllergySourceLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Active] = @Active,
			[Default] = @Default
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_AllergySourceLookUp
			(LookUpType, 
			Name, 
			[Active], 
			[Default], 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@Active, 
			@Default, 
			@MongoID)
	End
END
GO
