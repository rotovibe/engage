SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SavePatientNoteProgram] 
	@MongoID varchar(50),
	@PatientNoteMongoId varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@UpdatedBy varchar(50),
	@Version float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientNoteId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientNoteId = PatientNoteId From RPT_PatientNote Where MongoId = @PatientNoteMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy !=  ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientNoteProgram Where MongoId = @MongoID)
	Begin
		Update RPT_PatientNoteProgram
		Set PatientNoteId = @PatientNoteId,
			MongoPatientNoteId = @PatientNoteMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			LastUpdatedOn = @LastUpdatedOn
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientNoteProgram(PatientNoteId, MongoPatientNoteId, MongoId, MongoUpdatedBy, UpdatedById, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version]) values (@PatientNoteId, @PatientNoteMongoId, @MongoID, @UpdatedBy, @UpdatedById, @LastUpdatedOn, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Version)
	End
END
GO
