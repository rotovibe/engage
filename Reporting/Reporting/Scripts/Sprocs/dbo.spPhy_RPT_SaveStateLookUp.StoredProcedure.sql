SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveStateLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Code varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_StateLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_StateLookUp
		Set LookUpType = @LookUpType,
			Name = @Name,
			Code = @Code
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_StateLookUp(LookUpType, Name, Code, MongoId) values (@LookUpType, @Name, @Code, @MongoID)
	End
END
GO
