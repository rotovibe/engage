SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveGoalAttribute] 
	@MongoID	varchar(50),
	@Name varchar(100),
	@Type varchar(50),
	@ControlType varchar(50),
	@Order varchar(50),
	@Required varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @UpdatedById INT
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_GoalAttribute Where MongoId = @MongoID)
	Begin
		Update RPT_GoalAttribute
		Set Name = @Name,
			[Type] = @Type,
			ControlType = @ControlType,
			[Order] = @Order,
			[Required] = @Required,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn,
			[Delete] = @Delete,
			TTLDate = @TimeToLive,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_GoalAttribute(Name, [Type], ControlType, [Order], [Required], [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn, [Delete], TTLDate, ExtraElements, MongoId) values (@Name, @Type, @ControlType, @Order, @Required, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @Delete, @TimeToLive, @ExtraElements, @MongoID)
	End
END
GO
