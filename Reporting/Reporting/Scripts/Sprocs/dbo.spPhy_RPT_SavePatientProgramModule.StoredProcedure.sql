SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientProgramModule]
	@MongoId						VARCHAR(50),					
	@MongoProgramId					VARCHAR(50),
	@PatientProgramId			INT,
	@MongoAssignedBy				VARCHAR(50),
	@AssignedOn					datetime,
	@MongoAssignedToId				VARCHAR(50),
	@AttributeEndDate			datetime,
	@AttributeStartDate			datetime,
	@Completed					VARCHAR(50),
	@Description				VARCHAR(MAX),
	@EligibilityStartDate		datetime,
	@Eligible					VARCHAR(50),
	@Name						VARCHAR(100),
	@Order						VARCHAR(50),
	@SourceId					VARCHAR(50),
	@State						VARCHAR(50),
	@Status						VARCHAR(50),
	@Enabled					VARCHAR(50),
	@StateUpdatedOn				datetime,
	@MongoCompletedBy				VARCHAR(50),
	@DateCompleted				datetime,
	@EligibilityRequirements	VARCHAR(50),
	@EligibilityEndDate			datetime,
	@MongoPrevious					VARCHAR(50),
	@MongoNext						VARCHAR(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @ReturnID	INT,
	@AssignedToId INT,
	@AssignedBy INT,
	@CompletedBy INT,
	@Previous	INT,
	@Next		INT

	if @MongoAssignedBy != ''
		begin
			set @AssignedBy = (select dbo.[RPT_User].UserId  from dbo.[RPT_User] where MongoId = @MongoAssignedBy);
		end	
	
	if @MongoAssignedToId != ''
		begin
			set @AssignedToId = (select dbo.[RPT_User].UserId  from dbo.[RPT_User] where MongoId = @MongoAssignedToId);
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

	If Exists(Select Top 1 1 From RPT_PatientProgramModule Where MongoId = @MongoId)
	Begin
		Update RPT_PatientProgramModule
		Set 
			MongoId 						=	@MongoId,										
			MongoProgramId					=	@MongoProgramId,				
			PatientProgramId			=	@PatientProgramId,		
			AssignedBy					=	@AssignedBy,		
			MongoAssignedBy					=	@MongoAssignedBy,		
			AssignedOn					=	@AssignedOn,			
			AssignedTo					=	@AssignedToId,			
			MongoAssignedTo					=	@MongoAssignedToId,			
			AttributeEndDate			=	@AttributeEndDate,		
			AttributeStartDate			=	@AttributeStartDate,		
			Completed					=	@Completed,				
			Description					=	@Description,
			EligibilityStartDate		=	@EligibilityStartDate,	
			Eligible					=	@Eligible,				
			Name						=	@Name,					
			[Order]						=	@Order,					
			SourceId					=	@SourceId,				
			[State]						=	@State,					
			[Status]					=	@Status,					
			[Enabled]					=	@Enabled,					
			StateUpdatedOn				=	@StateUpdatedOn,			
			CompletedBy					=	@CompletedBy,				
			MongoCompletedBy				=	@MongoCompletedBy,
			DateCompleted				=	@DateCompleted,			
			EligibilityRequirements		=	@EligibilityRequirements,	
			EligibilityEndDate			=	@EligibilityEndDate,
			MongoPrevious					=	@MongoPrevious,
			Previous					=	@Previous,			
			MongoNext						=	@MongoNext,
			[Next]						=	@Next		 
		Where MongoId = @MongoId			
		
		Select @ReturnID = PatientProgramModuleId From RPT_PatientProgramModule Where MongoId = @MongoId
	End
	Else
	Begin
		Insert into 
			RPT_PatientProgramModule 
			(
				MongoId, 					
				MongoProgramId,				
				PatientProgramId,		
				AssignedBy,		
				MongoAssignedBy,		
				AssignedOn,				
				AssignedTo,				
				MongoAssignedTo,				
				AttributeEndDate,		
				AttributeStartDate,		
				Completed,				
				[Description],				
				EligibilityStartDate,	
				Eligible,				
				[Name],					
				[Order],					
				SourceId,				
				[State],					
				[Status],				
				[Enabled],				
				StateUpdatedOn,			
				CompletedBy,
				MongoCompletedBy,				
				DateCompleted,			
				EligibilityRequirements,	
				EligibilityEndDate,
				MongoPrevious,
				Previous,
				MongoNext,
				[Next]
			) 
			values
			(
				@MongoId,					
				@MongoProgramId,			
				@PatientProgramId,		
				@AssignedBy,		
				@MongoAssignedBy,		
				@AssignedOn,			
				@AssignedToId,			
				@MongoAssignedToId,			
				@AttributeEndDate,		
				@AttributeStartDate,	
				@Completed,				
				@Description,
				@EligibilityStartDate,	
				@Eligible,				
				@Name,					
				@Order,					
				@SourceId,				
				@State,					
				@Status,				
				@Enabled,				
				@StateUpdatedOn,		
				@CompletedBy,
				@MongoCompletedBy,			
				@DateCompleted,			
				@EligibilityRequirements,
				@EligibilityEndDate,
				@MongoPrevious,
				@Previous,
				@MongoNext,
				@Next
			)
		Select @ReturnID = @@IDENTITY
	End
	
	Select @ReturnID
END
GO
