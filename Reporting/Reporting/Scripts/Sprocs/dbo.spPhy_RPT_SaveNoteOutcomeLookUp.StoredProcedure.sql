SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveNoteOutcomeLookUp] 
	@MongoId varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_NoteOutcomeLookUp Where MongoId = @MongoId)
	Begin
		Update RPT_NoteOutcomeLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default,
			[Active] = @Active
		Where 
			MongoId = @MongoId		
	End
	Else
	Begin
		Insert Into RPT_NoteOutcomeLookUp(
			LookUpType, 
			Name, 
			[Default], 
			MongoId, 
			Active
		) values (
			@LookUpType, 
			@Name, 
			@Default, 
			@MongoId, 
			@Active
		)
	End
END
GO
