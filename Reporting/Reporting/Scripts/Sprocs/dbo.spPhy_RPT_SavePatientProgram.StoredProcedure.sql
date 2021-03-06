SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientProgram] 
	@MongoID					varchar(50),
	@MongoPatientId				varchar(50),
	@MongoAssignedBy				varchar(50),
	@AssignedOn					datetime,
	@MongoAssignedToId				varchar(50),
	@AttributeEndDate			datetime,
	@AttributeStartDate 		datetime,
	@Completed					varchar(50),
	@ContractProgramId			varchar(50),
	@Description				varchar(MAX),
	@EligibilityReason			varchar(50),
	@EligibilityStartDate		datetime,
	@Eligible					varchar(50),
	@EndDate					datetime,
	@Name						varchar(100),
	@Order						varchar(50),
	@ShortName					varchar(100),
	@SourceId					varchar(50),
	@StartDate					datetime,
	@State						varchar(50),
	@Status						varchar(50),
	@Enabled					varchar(50),
	@StateUpdatedOn 			varchar(50),
	@MongoCompletedBy				varchar(50),
	@DateCompleted				datetime,
	@EligibilityRequirements	varchar(50),
	@EligibilityEndDate			datetime,
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
	@patientId varchar(50),
	@RecordCreatedById INT,
	@UpdatedById INT,
	@AssignedBy INT,
	@AssignedToId INT,
	@CompletedBy INT

	-- find uniqueId
	set @patientId = (select PatientId from dbo.RPT_Patient where MongoId = @MongoPatientId);

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

	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientProgram Where MongoId = @MongoID)
		Begin
			Update RPT_PatientProgram
			Set  
				MongoPatientId			=	@MongoPatientId,
				AssignedBy				=	@AssignedBy,
				MongoAssignedBy				=	@MongoAssignedBy,			
				AssignedOn				=	@AssignedOn,					
				AssignedToId			=	@AssignedToId,				
				MongoAssignedToId			=	@MongoAssignedToId,				
				AttributeEndDate		=	@AttributeEndDate,			
				AttributeStartDate		=	@AttributeStartDate, 		
				Completed				=	@Completed,					
				ContractProgramId		=	@ContractProgramId,			
				[Delete]				=	@Delete,						
				[Description]			=	@Description,				
				EligibilityReason		=	@EligibilityReason,			
				EligibilityStartDate	=	@EligibilityStartDate,		
				Eligible				=	@Eligible,					
				EndDate					=	@EndDate,					
				LastUpdatedOn			=	@LastUpdatedOn,				
				Name					=	@Name,						
				[Order]					=	@Order,						
				RecordCreatedBy			=	@RecordCreatedById,
				MongoRecordCreatedBy		=	@MongoRecordCreatedBy, 			
				RecordCreatedOn			=	@RecordCreatedOn, 			
				ShortName				=	@ShortName,					
				SourceId				=	@SourceId,					
				StartDate				=	@StartDate,					
				[State]					=	@State,						
				[Status]				=	@Status,						
				MongoUpdatedBy				=	@MongoUpdatedBy,
				UpdatedBy				=	@UpdatedById,					
				[Version]				=	@Version,					
				[Enabled]				=	@Enabled,					
				StateUpdatedOn			=	@StateUpdatedOn, 			
				CompletedBy				=	@CompletedBy,
				MongoCompletedBy				=	@MongoCompletedBy,				
				DateCompleted			=	@DateCompleted,				
				EligibilityRequirements =	@EligibilityRequirements,	
				EligibilityEndDate		=	@EligibilityEndDate,
				TTLDate					=	@TTLDate	
			Where 
				MongoId = @MongoID
			Select 
				@ReturnID = PatientID 
			From RPT_Patient 
			Where 
				MongoId = @MongoID
		End
	Else
		Begin
			Insert into RPT_PatientProgram(
				MongoPatientId,		
				AssignedBy,	
				MongoAssignedBy,				
				AssignedOn,				
				AssignedToId,	
				MongoAssignedToId,			
				AttributeEndDate,		
				AttributeStartDate,		
				Completed,
				ContractProgramId,		
				[Delete],				
				[Description],			
				EligibilityReason,		
				EligibilityStartDate,	
				Eligible,				
				EndDate,					
				LastUpdatedOn,			
				Name,					
				[Order],					
				RecordCreatedBy,
				MongoRecordCreatedBy,		
				RecordCreatedOn,			
				ShortName,				
				SourceId,				
				StartDate,				
				[State],					
				[Status],				
				UpdatedBy,
				MongoUpdatedBy,				
				[Version],				
				[Enabled],				
				StateUpdatedOn,			
				CompletedBy,
				MongoCompletedBy,				
				DateCompleted,			
				EligibilityRequirements, 
				EligibilityEndDate,
				MongoId,
				PatientId,
				TTLDate)
			values (
				@MongoPatientId,
				@AssignedBy,
				@MongoAssignedBy,			
				@AssignedOn,			
				@AssignedToId,
				@MongoAssignedToId,
				@AttributeEndDate,		
				@AttributeStartDate, 	
				@Completed,				
				@ContractProgramId,		
				@Delete,				
				@Description,			
				@EligibilityReason,		
				@EligibilityStartDate,	
				@Eligible,				
				@EndDate,				
				@LastUpdatedOn,			
				@Name,					
				@Order,
				@RecordCreatedById,		
				@MongoRecordCreatedBy, 		
				@RecordCreatedOn, 		
				@ShortName,				
				@SourceId,				
				@StartDate,				
				@State,					
				@Status,
				@UpdatedById,				
				@MongoUpdatedBy,				
				@Version,				
				@Enabled,				
				@StateUpdatedOn, 		
				@CompletedBy,
				@MongoCompletedBy,			
				@DateCompleted,			
				@EligibilityRequirements,
				@EligibilityEndDate, 
				@MongoID,
				@patientId,
				@TTLDate)
			Select @ReturnID = @@IDENTITY
		End
	
	Select @ReturnID
END
GO
