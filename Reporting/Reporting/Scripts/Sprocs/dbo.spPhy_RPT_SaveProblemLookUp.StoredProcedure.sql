SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveProblemLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Type varchar(100),
	@Active varchar(50),
	@CodeSystem varchar(100),
	@Code varchar(100),
	@Default varchar(50),
	@DefaultLevel varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_ProblemLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_ProblemLookUp
		Set LookUpType = @LookUpType,
			Name = @Name,
			[Type] = @Type,
			Active = @Active,
			CodeSystem = @CodeSystem,
			Code = @Code,
			[Default] = @Default,
			DefaultLevel = @DefaultLevel
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_ProblemLookUp(LookUpType, Name, [Type], Active, CodeSystem, Code, [Default], DefaultLevel, MongoId) values (@LookUpType, @Name, @Type, @Active, @CodeSystem, @Code, @Default, @DefaultLevel, @MongoID)
	End
END
GO
