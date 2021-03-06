SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveAllergyType]
	@MongoAllergyId			[varchar](50),
	@MongoTypeId			[varchar](50),	
	@MongoUpdatedBy			[varchar](50),
	@LastUpdatedOn			[datetime],
	@MongoRecordCreatedBy	[varchar](50),
	@RecordCreatedOn		[datetime],
	@Version			 	[float]
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
			@RecordCreatedById INT,
			@UpdatedById INT,
			@AllergyId INT
	
	if @MongoAllergyId != ' '
		Select @AllergyId = AllergyId From [RPT_Allergy] Where MongoId = @MongoAllergyId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	--If Exists(Select Top 1 1 From RPT_AllergyType Where MongoAllergyId = @MongoAllergyId)
	--	Begin
	--		Update RPT_AllergyType
	--		Set 
	--			AllergyId				= @AllergyId,
	--			MongoAllergyId			= @MongoAllergyId,
	--			MongoTypeId				= @MongoTypeId,
	--			MongoUpdatedBy			= @MongoUpdatedBy,
	--			UpdatedById				= @UpdatedById,
	--			LastUpdatedOn			= @LastUpdatedOn,		
	--			MongoRecordCreatedBy	= @MongoRecordCreatedBy,
	--			RecordCreatedById		= @RecordCreatedById,
	--			RecordCreatedOn			= @RecordCreatedOn,	
	--			[Version]				= @Version
	--		Where MongoAllergyId = @MongoAllergyId
	--	End
	--Else
	--	Begin
			Insert Into RPT_AllergyType
			(
				AllergyId,
				MongoAllergyId,					
				MongoTypeId,		
				MongoUpdatedBy,		
				LastUpdatedOn,		
				MongoRecordCreatedBy,
				RecordCreatedOn,	
				[Version]			 												
			) 
			values 
			(
				@AllergyId,
				@MongoAllergyId,					
				@MongoTypeId,		
				@MongoUpdatedBy,		
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedOn,	
				@Version			
			)
		--End
END
GO
