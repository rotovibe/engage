SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveToDoCategoryLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_ToDoCategoryLookUp Where MongoId = @MongoID)
		Begin
			Update RPT_ToDoCategoryLookUp
			Set 
				LookUpType = @LookUpType,
				Name = @Name
			Where MongoId = @MongoID
			
		End
	Else
		Begin
			Insert Into RPT_ToDoCategoryLookUp
			(
				LookUpType, 
				Name, 
				MongoId
			) 
			values 
			(
				@LookUpType, 
				@Name, 
				@MongoID
			)
		End
END
GO
