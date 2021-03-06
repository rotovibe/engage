SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientProgramModuleActionStep]
	@MongoId						VARCHAR(50),							
	@MongoActionId					VARCHAR(50),					
	@ActionId					INT,						
	@StepTypeId 				VARCHAR(50),					
	@Header						VARCHAR(100),						
	@SelectedResponseId 		VARCHAR(50),		
	@ControlType				VARCHAR(50),					
	@SelectType					VARCHAR(50),					
	@IncludeTime				VARCHAR(50),					
	@Question					VARCHAR(MAX),						
	@Title						VARCHAR(100),						
	@Description				VARCHAR(MAX),					
	@Notes						VARCHAR(MAX),							
	@Text						VARCHAR(MAX),							
	@Status 					VARCHAR(50),						
	@AttributeEndDate			datetime,			
	@AttributeStartDate 		datetime,		
	@Completed					VARCHAR(50),					
	@EligibilityStartDate		datetime,		
	@Eligible					VARCHAR(50),						
	@Order						VARCHAR(50),							
	@SourceId					VARCHAR(50),						
	@State						VARCHAR(50),							
	@Enabled					VARCHAR(50),						
	@StateUpdatedOn				datetime,				
	@MongoCompletedBy				VARCHAR(50),					
	@DateCompleted				datetime,				
	@EligibilityRequirements	VARCHAR(50),	
	@EligibilityEndDate			datetime,
	@MongoPrevious					VARCHAR(50),
	@MongoNext						VARCHAR(50),
	@Version					varchar(50),
	@MongoUpdatedBy					varchar(50),
	@LastUpdatedOn				datetime,	
	@MongoRecordCreatedBy 			varchar(50),
	@RecordCreatedOn 			datetime,
	@TTLDate					datetime,				
	@Delete						varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ReturnID	INT,
	@CompletedBy INT,
	@previous INT,
	@Next INT,
	@RecordCreatedById INT,
	@UpdatedById INT
	
	-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedById = (select dbo.[RPT_User].UserId  from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedById = (select dbo.[RPT_User].UserId  from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end		
	
		if @MongoCompletedBy != ''
		begin
			set @CompletedBy = (select dbo.[RPT_User].UserId  from dbo.[RPT_User] where MongoId = @MongoCompletedBy);
		end	
	
	if @MongoPrevious != ''
		begin
			set @Previous = (select dbo.RPT_PatientProgramModule.PatientProgramModuleId  from dbo.RPT_PatientProgramModule where dbo.RPT_PatientProgramModule.MongoId = @MongoPrevious);
		end	

	if @MongoNext != ''
		begin
			set @Next = (select dbo.RPT_PatientProgramModule.PatientProgramModuleId  from dbo.RPT_PatientProgramModule where dbo.RPT_PatientProgramModule.MongoId = @MongoNext);
		end		
	
	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientProgramStep Where MongoId = @MongoId)
	Begin
		Update RPT_PatientProgramStep
		Set 
			MongoId							= @MongoId,						
			MongoActionId					= @MongoActionId,			
			ActionId					= @ActionId,				
			StepTypeId 					= @StepTypeId, 				
			Header						= @Header,					
			SelectedResponseId 			= @SelectedResponseId,
			ControlType					= @ControlType,
			SelectType					= @SelectType,
			IncludeTime					= @IncludeTime,
			Question					= @Question,
			Title						= @Title,
			[Description]				= @Description,
			Notes						= @Notes,						
			[Text]						= @Text,						
			[Status]					= @Status, 					
			AttributeEndDate			= @AttributeEndDate,			
			AttributeStartDate 			= @AttributeStartDate, 		
			Completed					= @Completed,					
			EligibilityStartDate		= @EligibilityStartDate,		
			Eligible					= @Eligible,					
			[Order]						= @Order,						
			SourceId					= @SourceId,					
			[State]						= @State,						
			[Enabled]					= @Enabled,					
			StateUpdatedOn				= @StateUpdatedOn,				
			MongoCompletedBy				= @MongoCompletedBy,
			CompletedBy					= @CompletedBy,				
			DateCompleted				= @DateCompleted,				
			EligibilityRequirements		= @EligibilityRequirements,	
			EligibilityEndDate			= @EligibilityEndDate,
			Previous					= @Previous,
			MongoPrevious					= @MongoPrevious,
			[Next]						= @Next,
			MongoNext						= @MongoNext,
			[Version]					= @Version,
			MongoUpdatedBy					= @MongoUpdatedBy,
			UpdatedBy					= @UpdatedById,			
			LastUpdatedOn				= @LastUpdatedOn,	
			MongoRecordCreatedBy			= @MongoRecordCreatedBy,
			RecordCreatedBy				= @RecordCreatedById,
			RecordCreatedOn				= @RecordCreatedOn,
			TTLDate						= @TTLDate,				
			[Delete]					= @Delete			
		Where MongoId = @MongoId
		
		Select 
			@ReturnID = StepId 
		From RPT_PatientProgramStep 
		Where 
			MongoId = @MongoId
	End
	Else
	Begin
		Insert into RPT_PatientProgramStep 
		(
			MongoId,						
			MongoActionId,			
			ActionId,				
			StepTypeId, 				
			Header,					
			SelectedResponseId,
			ControlType,
			SelectType,
			IncludeTime,
			Question,
			Title,
			[Description],
			Notes,					
			[Text],					
			[Status], 					
			AttributeEndDate,		
			AttributeStartDate, 		
			Completed,				
			EligibilityStartDate,	
			Eligible,				
			[Order],					
			SourceId,				
			[State],					
			[Enabled],					
			StateUpdatedOn,			
			CompletedBy,
			MongoCompletedBy,				
			DateCompleted,			
			EligibilityRequirements,	
			EligibilityEndDate,
			Previous,
			MongoPrevious,
			[Next],	
			MongoNext,
			[Version],
			MongoUpdatedBy,
			UpdatedBy,
			LastUpdatedOn,
			MongoRecordCreatedBy,
			RecordCreatedBy,
			RecordCreatedOn,
			TTLDate,
			[Delete]						
		) 
		values
		(
			@MongoId,						
			@MongoActionId,			
			@ActionId,				
			@StepTypeId, 				
			@Header,					
			@SelectedResponseId,
			@ControlType,
			@SelectType,
			@IncludeTime,
			@Question,
			@Title,
			@Description,
			@Notes,					
			@Text,					
			@Status, 					
			@AttributeEndDate,		
			@AttributeStartDate, 		
			@Completed,				
			@EligibilityStartDate,	
			@Eligible,				
			@Order,					
			@SourceId,				
			@State,					
			@Enabled,					
			@StateUpdatedOn,			
			@CompletedBy,
			@MongoCompletedBy,				
			@DateCompleted,			
			@EligibilityRequirements,	
			@EligibilityEndDate,
			@Previous,
			@MongoPrevious,
			@Next,
			@MongoNext,
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
		Select @ReturnID = @@IDENTITY
	End
	
	Select @ReturnID
END
GO
