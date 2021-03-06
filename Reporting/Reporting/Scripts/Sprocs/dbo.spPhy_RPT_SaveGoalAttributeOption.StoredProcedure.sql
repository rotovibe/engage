SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveGoalAttributeOption]
	@GoalAttributeMongoId varchar(50),
	@Key INT,
	@Value varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @GoalAttributeId INT,
			@UpdatedById INT
	
	Select @GoalAttributeId = GoalAttributeID From RPT_GoalAttribute Where MongoId = @GoalAttributeMongoId
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_GoalAttributeOption Where GoalAttributeId = @GoalAttributeId AND [Key] = @Key)
	Begin
		Update RPT_GoalAttributeOption
		Set Value = @Value,
			MongoGoalAttributeId = @GoalAttributeMongoId,
			[Version] = @Version,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			LastUpdatedOn = @LastUpdatedOn
		Where GoalAttributeId = @GoalAttributeId AND [Key] = @Key
	End
	Else
	Begin
		Insert Into RPT_GoalAttributeOption(GoalAttributeId, MongoGoalAttributeId, [Key], Value, [Version], MongoUpdatedBy, UpdatedById, LastUpdatedOn) values (@GoalAttributeId, @GoalAttributeMongoId, @Key, @Value, @Version, @UpdatedBy, @UpdatedById, @LastUpdatedOn)
	End
END
GO
