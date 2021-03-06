SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientMedSupp] 
	@MongoId				[varchar](50),
	@MongoPatientId			[varchar](50),
	@MongoFamilyId			[VARCHAR](50),
	@Name					[varchar] (200),
	@Category				[varchar] (200),
	@MongoTypeId			[varchar] (50),
	@Status					[varchar] (200),
	@Dosage					[varchar] (500),
	@Strength				[varchar] (200),
	@Route					[varchar] (200),
	@Form					[varchar] (200),
	@FreqQuantity			[varchar] (200),
	@MongoFreqHowOftenId	[varchar] (50),
	@MongoFreqWhenId		[varchar] (50),
	@MongoSourceId			[varchar](50),	
	@StartDate				[datetime],
	@EndDate				[datetime],
	@Reason					[varchar](5000),
	@Notes					[varchar](5000),	
	@PrescribedBy			[varchar](500),	
	@SystemName				[varchar](50),
	@MongoUpdatedBy			[varchar](50),
	@LastUpdatedOn			[datetime],
	@MongoRecordCreatedBy	[varchar](50),
	@RecordCreatedOn		[datetime],
	@Version				[float],
	@TTLDate				[DATETIME],
	@Delete					[VARCHAR](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@FreqHowOftenId INT,
			@FreqWhenId INT,			
			@RecordCreatedById INT,
			@UpdatedById INT,
			@MedSuppTypeId INT,
			@SourceId INT
	
	select @FreqHowOftenId = FreqId from RPT_FreqHowOftenLookUp where MongoId = @MongoFreqHowOftenId
	select @FreqWhenId = FreqWhenId from RPT_FreqWhenLookUp where MongoId = @MongoFreqWhenId
	select @MedSuppTypeId = mst.MedSupId  from RPT_MedSupTypeLookUp as mst where MongoId = @MongoTypeId
	select @SourceId = AllergySourceId from RPT_AllergySourceLookUp where MongoId = @MongoSourceId
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @MongoPatientId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientMedSupp Where MongoId = @MongoID)
		Begin
			Update RPT_PatientMedSupp
			Set 
				MongoId					= @MongoId,				
				MongoPatientId			= @MongoPatientId,
				MongoFamilyId			= @MongoFamilyId,
				PatientId				= @PatientId,			
				Name					= @Name,					
				Category				= @Category,				
				MongoTypeId				= @MongoTypeId,
				TypeId					= @MedSuppTypeId,			
				[Status]				= @Status,					
				Dosage					= @Dosage,					
				Strength				= @Strength,				
				[Route]					= @Route,					
				Form					= @Form,					
				FreqQuantity			= @FreqQuantity,			
				MongoFreqHowOftenId		= @MongoFreqHowOftenId,	
				FreqHowOftenId			= @FreqHowOftenId,	
				MongoFreqWhenId			= @MongoFreqWhenId,	
				FreqWhenId				= @FreqWhenId,		
				MongoSourceId			= @MongoSourceId,			
				SourceId				= @SourceId,			
				StartDate				= @StartDate,				
				EndDate					= @EndDate,				
				Reason					= @Reason,					
				Notes					= @Notes,					
				PrescribedBy			= @PrescribedBy,			
				SystemName				= @SystemName,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				UpdatedBy				= @UpdatedById,				
				LastUpdatedOn			= @LastUpdatedOn,			
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,
				RecordCreatedBy			= @RecordCreatedById,
				RecordCreatedOn			= @RecordCreatedOn,
				[Version]				= @Version,
				TTLDate					= @TTLDate,
				[Delete]				= @Delete
			Where 
				MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_PatientMedSupp
			(
				MongoId,
				MongoPatientId,
				MongoFamilyId,
				PatientId,
				Name,
				Category,
				MongoTypeId,
				TypeId,
				[Status],
				Dosage,
				Strength,
				[Route],
				Form,
				FreqQuantity,			
				MongoFreqHowOftenId,		
				FreqHowOftenId,			
				MongoFreqWhenId,			
				FreqWhenId,				
				MongoSourceId,			
				SourceId,				
				StartDate,				
				EndDate,					
				Reason,					
				Notes,					
				PrescribedBy,			
				SystemName,				
				MongoUpdatedBy,			
				UpdatedBy,				
				LastUpdatedOn,			
				MongoRecordCreatedBy,	
				RecordCreatedBy,			
				RecordCreatedOn,			
				[Version],				
				TTLDate,					
				[Delete]						
			) 
			values 
			(
				@MongoId,				
				@MongoPatientId,
				@MongoFamilyId,
				@PatientId,			
				@Name,				
				@Category,			
				@MongoTypeId,
				@MedSuppTypeId,		
				@Status,				
				@Dosage,				
				@Strength,			
				@Route,				
				@Form,				
				@FreqQuantity,		
				@MongoFreqHowOftenId,	
				@FreqHowOftenId,	
				@MongoFreqWhenId,	
				@FreqWhenId,		
				@MongoSourceId,		
				@SourceId,			
				@StartDate,			
				@EndDate,				
				@Reason,				
				@Notes,				
				@PrescribedBy,		
				@SystemName,			
				@MongoUpdatedBy,		
				@UpdatedById,			
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,
				@Version,
				@TTLDate,
				@Delete
			)
		End
END
GO
