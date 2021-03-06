SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientProblem] 
	@MongoID varchar(50),
	@Active varchar(50),
	@Delete varchar(50),
	@Featured varchar(50),
	@LastUpdatedOn datetime,
	@Level INT,
	@PatientMongoId varchar(50),
	@ProblemMongoId varchar(50),
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@UpdatedBy varchar(50),
	@Version float,
	@StartDate datetime,
	@EndDate datetime,
	@TTLDate datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@ProblemId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @PatientMongoId
	
	Select @ProblemId = ProblemId From RPT_ProblemLookUp Where MongoId = @ProblemMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientProblem Where MongoId = @MongoID)
	Begin
		Update RPT_PatientProblem
		Set Active = @Active,
			[Delete] = @Delete,
			Featured = @Featured,
			LastUpdatedOn = @LastUpdatedOn,
			[Level] = @Level,
			PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			ProblemId = @ProblemId,
			MongoProblemLookUpId = @ProblemMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			StartDate = @StartDate,
			EndDate = @EndDate,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientProblem(StartDate, EndDate, TTLDate, ExtraElements, Active, [Delete], Featured, LastUpdatedOn, [Level], PatientId, MongoPatientId, ProblemId, MongoProblemLookUpId, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, MongoUpdatedBy, UpdatedById, [Version], MongoId) values (@StartDate, @EndDate, @TTLDate, @ExtraElements, @Active, @Delete, @Featured, @LastUpdatedOn, @Level, @PatientId, @PatientMongoId, @ProblemId, @ProblemMongoId, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @UpdatedBy, @UpdatedById, @Version, @MongoID)
	End
END
GO
