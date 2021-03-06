SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveSpawnElement] 
	@MongoPlanElementId		VARCHAR(50),
	@PlanElementId		INT,	
	@MongoSpawnId			VARCHAR(50),			
	@Tag				VARCHAR(50),					
	@Type				INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
		@ReturnID		INT
	
	-- Insert statements for procedure here
	If Not Exists(Select Top 1 1 From RPT_SpawnElements 
					Where PlanElementId = @PlanElementId AND 
						MongoPlanElementId = @MongoPlanElementId AND 
						MongoSpawnId = @MongoSpawnId AND 
						Tag = @Tag AND 
						[TypeId] = @Type)
		Begin
			Insert into 
			RPT_SpawnElements
			(
				MongoPlanElementId,
				PlanElementId,
				MongoSpawnId,	
				Tag,		
				[TypeId]			
			)
			values 
			(
				@MongoPlanElementId,
				@PlanElementId,	
				@MongoSpawnId,		
				@Tag,			
				@Type						
			)
			Select @ReturnID = @@IDENTITY
		End
	
	Select @ReturnID
END
GO
