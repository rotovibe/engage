SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveObservation] 
	@MongoID varchar(50),
	@Code varchar(50),
	@CodingSystemId varchar(50),
	@Delete varchar(50),
	@Description varchar(MAX),
	@ExtraElements varchar(MAX),
	@HighValue INT,
	@LastUpdatedOn datetime,
	@LowValue INT,
	@ObservationTypeMongoId varchar(50),
	@Order INT,
	@Source varchar(50),
	@Standard varchar(50),
	@Status varchar(50),
	@TimeToLive datetime,
	@Units varchar(50),
	@UpdatedBy varchar(50),
	@Version float,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@CommonName varchar(100),
	@GroupId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @ObservationTypeId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ObservationTypeId = ObservationTypeId From RPT_ObservationTypeLookUp Where MongoId = @ObservationTypeMongoId
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId from [RPT_User] Where MongoId = @RecordCreatedBy
	
	
	If (@HighValue = -1)
	Select @HighValue = NULL
	
	If (@LowValue = -1)
	Select @LowValue = NULL
	
	If (@Order = -1)
	Select @Order = NULL
	
	If Exists(Select Top 1 1 From RPT_Observation Where MongoId = @MongoID)
	Begin
		Update RPT_Observation
		Set Code = @Code,
			CodingSystemId = @CodingSystemId,
			[Delete] = @Delete,
			[Description] = @Description,
			ExtraElements = @ExtraElements,
			HighValue = @HighValue,
			LastUpdatedOn = @LastUpdatedOn,
			LowValue = @LowValue,
			ObservationTypeId = @ObservationTypeId,
			MongoObservationLookUpId = @ObservationTypeMongoId,
			[Order] = @Order,
			[Source] = @Source,
			[Standard] = @Standard,
			[Status] = @Status,
			TTLDate = @TimeToLive,
			Units = @Units,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version,
			CommonName = @CommonName,
			GroupId = @GroupId
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_Observation(CommonName, GroupId, Code, CodingSystemId, [Delete], [Description], ExtraElements, HighValue, LastUpdatedOn, LowValue, ObservationTypeId, MongoObservationLookUpId, [Order], [Source], [Standard], [Status], TTLDate, Units, MongoUpdatedBy, UpdatedById, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version], MongoId) values 
		(@CommonName, @GroupId, @Code, @CodingSystemId, @Delete, @Description, @ExtraElements, @HighValue, @LastUpdatedOn, @LowValue, @ObservationTypeId, @ObservationTypeMongoId, @Order, @Source, @Standard, @Status, @TimeToLive, @Units, @UpdatedBy, @UpdatedById, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Version, @MongoID)
	End
END
GO
