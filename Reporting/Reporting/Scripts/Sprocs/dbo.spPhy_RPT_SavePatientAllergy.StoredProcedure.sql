SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientAllergy] 
	@MongoId [varchar](50),
	@MongoAllergyId [varchar](50),
	@MongoPatientId [varchar](50),
	@MongoSeverityId [varchar](50),
	@StatusId [varchar](100),
	@SourceId [varchar](50),
	@StartDate [datetime],
	@EndDate [datetime],
	@Notes [varchar](5000),
	@SystemName [varchar](50),
	@MongoUpdatedBy [varchar](50),
	@LastUpdatedOn [datetime],
	@MongoRecordCreatedBy [varchar](50),
	@RecordCreatedOn [datetime],
	@Version [float],
	@TTLDate [datetime],
	@Delete [varchar](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@AllergyId INT,
			@SeverityId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @MongoPatientId
	
	Select @AllergyId = AllergyId From RPT_Allergy Where MongoId = @MongoAllergyId
	
	Select @SeverityId = SeverityId from RPT_SeverityLookUp Where MongoId = @MongoSeverityId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientAllergy Where MongoId = @MongoID)
		Begin
			Update RPT_PatientAllergy
			Set 
				MongoId					= @MongoId,				
				AllergyId				= @AllergyId,
				MongoAllergyId			= @MongoAllergyId,			
				PatientId				= @PatientId,
				MongoPatientId			= @MongoPatientId,			
				SeverityId				= @SeverityId,
				MongoSeverityId			= @MongoSeverityId,		
				StatusId				= @StatusId,				
				SourceId				= @SourceId,				
				StartDate				= @StartDate,				
				EndDate					= @EndDate,				
				Notes					= @Notes,					
				SystemName				= @SystemName,
				UpdatedBy				= @UpdatedById,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				LastUpdatedOn			= @LastUpdatedOn,			
				RecordCreatedBy			= @RecordCreatedById,
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,	
				RecordCreatedOn			= @RecordCreatedOn,		
				[Version]				= @Version,				
				TTLDate					= @TTLDate,				
				[Delete]				= @Delete					
			Where MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_PatientAllergy
			(
				MongoId,				
				AllergyId,				
				MongoAllergyId,			
				PatientId,			
				MongoPatientId,			
				SeverityId,				
				MongoSeverityId,			
				StatusId,				
				SourceId,				
				StartDate,				
				EndDate,					
				Notes,					
				SystemName,				
				UpdatedBy,				
				MongoUpdatedBy,			
				LastUpdatedOn,			
				RecordCreatedBy,		
				MongoRecordCreatedBy,	
				RecordCreatedOn,			
				[Version],				
				TTLDate,					
				[Delete]
			) 
			values 
			(
				@MongoId,				
				@AllergyId,
				@MongoAllergyId,		
				@PatientId,
				@MongoPatientId,		
				@SeverityId,
				@MongoSeverityId,		
				@StatusId,			
				@SourceId,			
				@StartDate,			
				@EndDate,				
				@Notes,				
				@SystemName,
				@UpdatedById,			
				@MongoUpdatedBy,		
				@LastUpdatedOn,		
				@RecordCreatedById,
				@MongoRecordCreatedBy,
				@RecordCreatedOn,		
				@Version,				
				@TTLDate,				
				@Delete
			)
		End
END
GO
