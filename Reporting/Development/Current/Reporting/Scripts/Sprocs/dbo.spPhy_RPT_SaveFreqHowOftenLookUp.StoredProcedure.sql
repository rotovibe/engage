SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveFreqHowOftenLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_FreqHowOftenLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_FreqHowOftenLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_FreqHowOftenLookUp
			(LookUpType, 
			Name, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@MongoID)
	End
END
GO
