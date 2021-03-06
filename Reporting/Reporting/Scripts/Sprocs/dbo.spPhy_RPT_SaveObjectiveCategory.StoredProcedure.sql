SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveObjectiveCategory] 
	@ObjectiveMongoId varchar(50),
	@CategoryMongoId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @ObjectiveId INT
	Declare @CategoryId INT
	
	Select @ObjectiveId = ObjectiveId From RPT_ObjectiveLookUp Where MongoId = @ObjectiveMongoId
	Select @CategoryId = CategoryId From RPTMongoCategoryLookUp Where MongoId = @CategoryMongoId
	
	Begin
	If not Exists(Select Top 1 1 From RPT_ObjectiveCategory Where ObjectiveId = @ObjectiveId AND CategoryId = @CategoryId)
		Begin
			Insert Into RPT_ObjectiveCategory(ObjectiveId, MongoObjectiveLookUpId, CategoryId, MongoCategoryLookUpId) values (@ObjectiveId, @ObjectiveMongoId, @CategoryId, @CategoryMongoId)
		End
	End
END
GO
