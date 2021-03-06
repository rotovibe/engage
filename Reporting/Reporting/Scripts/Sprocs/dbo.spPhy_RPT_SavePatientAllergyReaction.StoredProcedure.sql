SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientAllergyReaction]
	@MongoPatientAllergyId [varchar](50),
	@MongoReactionId [varchar](50),	
	@MongoUpdatedBy [varchar](50),
	@LastUpdatedOn [datetime],
	@MongoRecordCreatedBy [varchar](50),
	@RecordCreatedOn [datetime],
	@Version [float]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
			@PatientAllergyId INT,
			@ReactionId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientAllergyId = PatientAllergyId From [RPT_PatientAllergy] Where MongoId = @MongoPatientAllergyId
	
	Select @ReactionId = ReactionId From [RPT_ReactionLookUp] Where MongoId = @MongoReactionId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	--If Exists(Select Top 1 1 From RPT_PatientAllergyReaction Where ReactionId = @ReactionId)
	--	Begin
	--		Update RPT_PatientAllergyReaction
	--		Set 
	--			PatientAllergyId		= @PatientAllergyId,
 --				MongoPatientAllergyId	= @MongoPatientAllergyId,
 --				MongoReactionId			= @MongoReactionId,	
 --				ReactionId				= @ReactionId,	
 --				UpdatedById				= @UpdatedById,
 --				MongoUpdatedBy			= @MongoUpdatedBy,
 --				LastUpdatedOn			= @LastUpdatedOn,
 --				RecordCreatedById		= @RecordCreatedById,
 --				MongoRecordCreatedBy	= @MongoRecordCreatedBy,
 --				RecordCreatedOn			= @RecordCreatedOn,
 --				[Version]				= @Version
	--		Where MongoPatientAllergyId = @MongoPatientAllergyId
	--	End
	--Else
	--	Begin
			Insert Into RPT_PatientAllergyReaction
			(
				PatientAllergyId,
				MongoPatientAllergyId,	
				MongoReactionId,			
				ReactionId,				
				UpdatedById,			
				MongoUpdatedBy,			
				LastUpdatedOn,			
				RecordCreatedById,		
				MongoRecordCreatedBy,	
				RecordCreatedOn,
				[Version]								
			) 
			values 
			(
				@PatientAllergyId,
				@MongoPatientAllergyId,
				@MongoReactionId,	
				@ReactionId,	
				@UpdatedById,
				@MongoUpdatedBy,
				@LastUpdatedOn,
				@RecordCreatedById,
				@MongoRecordCreatedBy,
				@RecordCreatedOn,
				@Version
			)
		--End
END
GO
