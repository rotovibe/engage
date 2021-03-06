SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveAllergy] 
	@MongoId [varchar](50),
	@Name [varchar](50),
	@CodingSystem [varchar](50),
	@CodingSystemCode [varchar](50),
	@Version [float],	
	@MongoUpdatedBy [varchar](50),
	@LastUpdatedOn [datetime],
	@MongoRecordCreatedBy [varchar](50),
	@RecordCreatedOn [datetime],
	@TTLDate [datetime],
	@Delete [varchar](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
			@RecordCreatedById INT,
			@UpdatedById INT
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_Allergy Where MongoId = @MongoID)
		Begin
			Update RPT_Allergy
			Set 
				Name					= @Name,					
				CodingSystem			= @CodingSystem,			
				CodingSystemCode		= @CodingSystemCode,		
				[Version]				= @Version,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				UpdatedById				= @UpdatedById,
				LastUpdatedOn			= @LastUpdatedOn,			
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,	
				RecordCreatedById       = @RecordCreatedById,
				RecordCreatedOn			= @RecordCreatedOn,		
				TTLDate					= @TTLDate,				
				[Delete]				= @Delete					
			Where MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_Allergy
			(
				MongoId,
				Name,
				CodingSystem,			
				CodingSystemCode,		
				[Version],				
				MongoUpdatedBy,			
				UpdatedById,				
				LastUpdatedOn,			
				MongoRecordCreatedBy,	
				RecordCreatedById,       
				RecordCreatedOn,			
				TTLDate,					
				[Delete]								
			) 
			values 
			(
				@MongoId,
				@Name,				
				@CodingSystem,		
				@CodingSystemCode,	
				@Version,				
				@MongoUpdatedBy,		
				@UpdatedById,
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,		
				@TTLDate,				
				@Delete								
			)
		End
END
GO
