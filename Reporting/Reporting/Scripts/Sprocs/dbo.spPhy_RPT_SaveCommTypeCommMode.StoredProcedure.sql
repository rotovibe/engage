SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveCommTypeCommMode]
	@CommTypeMongoId varchar(50),
	@CommModeMongoId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @CommTypeId INT
	Declare @CommModeId INT
	
	Select @CommTypeId = CommTypeId From RPT_CommTypeLookUp Where MongoId = @CommTypeMongoId
	Select @CommModeId = CommModeId From RPT_CommModeLookUp Where MongoId = @CommModeMongoId
	
	If not Exists(Select Top 1 1 From RPT_CommTypeCommMode Where CommTypeId = @CommTypeId AND CommModeId = @CommModeId)
		Begin
			Insert Into RPT_CommTypeCommMode(CommModeId, MongoCommModeLookUpId, CommTypeId, MongoCommTypeLookUpId) values (@CommModeId, @CommModeMongoId, @CommTypeId, @CommTypeMongoId)
		End
END
GO
