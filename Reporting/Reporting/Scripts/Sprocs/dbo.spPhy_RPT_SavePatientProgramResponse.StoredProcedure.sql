SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientProgramResponse]
		@MongoId 					VARCHAR(50),				
		@MongoActionId 			VARCHAR(50),		
		@Order 				VARCHAR(50),			
		@Text 				VARCHAR(MAX),			
		@MongoStepId 			VARCHAR(50),		
		@Value 				VARCHAR(50),			
		@Nominal 			VARCHAR(50),		
		@Required 			VARCHAR(50),		
		@MongoNextStepId 		VARCHAR(50),	
		@Selected 			VARCHAR(50),		
		@MongoStepSourceId 		VARCHAR(50),
		@MongoUpdatedBy 		VARCHAR(50),		
		@MongoRecordCreatedBy 	VARCHAR(50),	
		@RecordCreatedOn 	datetime,	
		@TTLDate 			datetime,			
		@Delete 			VARCHAR(50),				
		@LastUpdatedOn 		datetime,
		@Version			FLOAT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare 
		@ReturnID			INT,
		@NextStepId			INT,
		@StepSourceId		INT,
		@RecordCreatedBy	INT,
		@UpdatedBy			INT,
		@StepId				INT

	set @ReturnID = -1;

	-- find next step by Id
	if @MongoNextStepId != ''
		begin
			set @NextStepId = (select dbo.RPT_PatientProgramStep.StepId from dbo.RPT_PatientProgramStep where MongoId = @MongoNextStepId);
		end

	-- find next step by Id
	if @MongoStepSourceId != ''
		begin
			set @StepSourceId = 000000000000000000000000;
			-- need to create stepresponse table 
			--(select dbo.RPT_PatientProgramStep.StepId from dbo.RPT_PatientProgramStep where MongoId = @MongoStepSourceId);
		end

	-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end

	-- stepid
	if @MongoStepId != ''
		begin
			set @StepId = (select dbo.RPT_PatientProgramStep.StepId from dbo.RPT_PatientProgramStep where MongoId = @MongoStepId);
		end

	--if @StepId IS NULL
	--	begin
	--		RETURN;
	--	end

	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientProgramResponse Where MongoId = @MongoId)
		Begin
			Update 
				RPT_PatientProgramResponse
			Set 
				MongoId 				= @MongoId, 				
				MongoActionId 			= @MongoActionId, 			
				[Order] 			= @Order, 					
				[Text] 				= @Text, 				
				MongoStepId 			= @MongoStepId, 			
				StepId 				= @StepId, 			
				Value 				= @Value, 				
				Nominal 			= @Nominal, 			
				[Required] 			= @Required, 			
				MongoNextStepId 		= @MongoNextStepId,
				NextStepId 			= @NextStepId, 		
				Selected 			= @Selected, 			
				MongoStepSourceId 		= @MongoStepSourceId,
				StepSourceId 		= @StepSourceId, 		
				UpdatedBy 			= @UpdatedBy,
				MongoUpdatedBy 			= @MongoUpdatedBy, 			
				RecordCreatedBy 	= @RecordCreatedBy, 	
				MongoRecordCreatedBy 	= @MongoRecordCreatedBy, 	
				RecordCreatedOn 	= @RecordCreatedOn, 	
				TTLDate 			= @TTLDate, 			
				[Delete] 			= @Delete, 			
				LastUpdatedOn 		= @LastUpdatedOn,
				[Version]			= @Version
			Where 
				MongoId = @MongoId
			
			Select @ReturnID = ResponseId From RPT_PatientProgramResponse Where MongoId = @MongoId
		End
	Else
		Begin
			Insert into 
				RPT_PatientProgramResponse 
				(
					MongoId, 				
					MongoActionId, 			
					[Order], 				
					[Text], 				
					MongoStepId, 			
					StepId, 			
					Value, 				
					Nominal, 			
					[Required], 			
					MongoNextStepId,
					NextStepId, 		
					Selected, 			
					MongoStepSourceId,
					StepSourceId, 		
					UpdatedBy,
					MongoUpdatedBy, 			
					RecordCreatedBy, 	
					MongoRecordCreatedBy, 	
					RecordCreatedOn, 	
					TTLDate, 			
					[Delete], 			
					LastUpdatedOn,
					[Version]		
				) values
				(
					@MongoId, 				
					@MongoActionId, 			
					@Order, 				
					@Text, 				
					@MongoStepId, 			
					@StepId, 			
					@Value, 				
					@Nominal, 			
					@Required, 			
					@MongoNextStepId,
					@NextStepId, 		
					@Selected, 			
					@MongoStepSourceId,
					@StepSourceId, 		
					@UpdatedBy,
					@MongoUpdatedBy, 			
					@RecordCreatedBy, 	
					@MongoRecordCreatedBy, 	
					@RecordCreatedOn, 	
					@TTLDate, 			
					@Delete, 			
					@LastUpdatedOn,
					@Version
				)
			Select @ReturnID = @@IDENTITY
		End
	
	Select @ReturnID
END
GO
