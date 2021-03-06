SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientProgramAttribute] 
	@MongoId						varchar(50),	
	@MongoPlanElementId			varchar(50),	
	@Completed				varchar(50),	
	@DidNotEnrollReason		varchar(50),	
	@Eligibility			varchar(50),	
	@Enrollment				varchar(50),	
	@GraduatedFlag			varchar(50),	
	@InelligibleReason		varchar(50),	
	@Lock					varchar(50),	
	@OptOut					varchar(50),	
	@OverrideReason			varchar(50),	
	@Population				varchar(50),	
	@RemovedReason			varchar(50),	
	@Status					varchar(50),	
	@MongoUpdatedBy				varchar(50),	
	@LastUpdatedOn			datetime,	
	@MongoRecordCreatedBy		varchar(50),	
	@RecordCreatedOn		datetime,	
	@Version				varchar(50),	
	@Delete					varchar(50),
	@DateCompleted			datetime,
	@MongoCompletedBy			varchar(50),
	@TTLDate				datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
		@ReturnID			INT,
		@PlanElementId		INT,
		@RecordCreatedById	INT,
		@UpdatedById		INT,
		@CompletedBy		INT

	-- find uniqueId
	set @PlanElementId = (select PatientProgramId from dbo.RPT_PatientProgram where MongoId = @MongoId);

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
	
	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientProgramAttribute Where MongoId = @MongoId)
		Begin
			Update RPT_PatientProgramAttribute
			Set  
				MongoId						= @MongoId,	
				MongoPlanElementId			= @MongoPlanElementId,	
				PlanElementId			= @PlanElementId,	
				Completed				= @Completed,	
				DidNotEnrollReason		= @DidNotEnrollReason,	
				Eligibility				= @Eligibility,	
				Enrollment				= @Enrollment,	
				GraduatedFlag			= @GraduatedFlag,	
				InelligibleReason		= @InelligibleReason,	
				Lock					= @Lock,	
				OptOut					= @OptOut,	
				OverrideReason			= @OverrideReason,	
				[Population]			= @Population,	
				RemovedReason			= @RemovedReason,	
				[Status]				= @Status,	
				MongoUpdatedBy				= @MongoUpdatedBy,	
				UpdatedBy				= @UpdatedById,	
				LastUpdatedOn			= @LastUpdatedOn,	
				MongoRecordCreatedBy		= @MongoRecordCreatedBy,	
				RecordCreatedBy			= @RecordCreatedById,	
				RecordCreatedOn			= @RecordCreatedOn,	
				[Version]				= @Version,	
				[Delete]				= @Delete,
				DateCompleted			= @DateCompleted,
				CompletedBy				= @CompletedBy,
				MongoCompletedBy			= @MongoCompletedBy,
				TTLDate					= @TTLDate								
			Where 
				MongoId = @MongoId
			
		End
	Else
		Begin
			Insert into RPT_PatientProgramAttribute(
				MongoId,					
				MongoPlanElementId,		
				PlanElementId,		
				Completed,			
				DidNotEnrollReason,	
				Eligibility,			
				Enrollment,			
				GraduatedFlag,		
				InelligibleReason,	
				Lock,				
				OptOut,				
				OverrideReason,		
				[Population],		
				RemovedReason,		
				[Status],			
				MongoUpdatedBy,			
				UpdatedBy,			
				LastUpdatedOn,		
				MongoRecordCreatedBy,	
				RecordCreatedBy,		
				RecordCreatedOn,		
				[Version],			
				[Delete],
				DateCompleted,
				CompletedBy,
				MongoCompletedBy,
				TTLDate			
				)
			values (
				 @MongoId,	
				 @MongoPlanElementId,	
				 @PlanElementId,	
				 @Completed,	
				 @DidNotEnrollReason,	
				 @Eligibility,	
				 @Enrollment,	
				 @GraduatedFlag,	
				 @InelligibleReason,	
				 @Lock,	
				 @OptOut,	
				 @OverrideReason,	
				 @Population,	
				 @RemovedReason,	
				 @Status,	
				 @MongoUpdatedBy,	
				 @UpdatedById,	
				 @LastUpdatedOn,	
				 @MongoRecordCreatedBy,	
				 @RecordCreatedById,	
				 @RecordCreatedOn,	
				 @Version,	
				 @Delete,
				 @DateCompleted,
				 @CompletedBy,
				 @MongoCompletedBy,
				 @TTLDate							
				)
			Select @ReturnID = @@IDENTITY
		End
	
	Select @ReturnID
END
GO
