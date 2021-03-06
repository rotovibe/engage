SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveToDoProgram]
	@MongoToDoId [varchar](50),
	@MongoProgramId [varchar](50)
AS
BEGIN
	SET NOCOUNT ON;
	
	Declare @ProgramId	int,
			@ToDoId		int
	
	Select @ProgramId = pp.PatientProgramId  from RPT_PatientProgram pp Where MongoId = @MongoProgramId;
	Select @ToDoId = td.ToDoId From dbo.RPT_ToDo td Where MongoId = @MongoToDoId;
	
	
	If Exists(Select Top 1 1 From RPT_ToDoProgram Where MongoToDoId = @MongoToDoId)
		Begin
			Update 
				RPT_ToDoProgram
			Set 
				MongoProgramId = @MongoProgramId,
				ProgramId = @ProgramId
			Where 
				MongoToDoId = @MongoToDoId
			
		End
	Else
		Begin
			Insert Into 
				RPT_ToDoProgram
				(
					MongoToDoId,
					ToDoId,
					MongoProgramId,
					ProgramId
				) 
				values 
				(
					@MongoToDoId,
					@ToDoId,
					@MongoProgramId,
					@ProgramId
				)
		End
END
GO
