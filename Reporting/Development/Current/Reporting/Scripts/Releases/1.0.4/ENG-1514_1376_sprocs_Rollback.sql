IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientNote]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientNote]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientNote]
	@MongoID			varchar(50),
	@PatientMongoId		varchar(50),
	@Delete				varchar(50),
	@LastUpdatedOn		datetime,
	@RecordCreatedBy	varchar(50),
	@RecordCreatedOn	datetime,
	@Text				varchar(MAX),
	@UpdatedBy			varchar(50),
	@Version			float,
	@TTLDate			datetime,
	@ExtraElements		varchar(MAX),
	@MongoMethodId			varchar(50),
	@Type				varchar(50),
	@MongoOutcomeId			varchar(50),
	@MongoWhoId				varchar(50)	,
	@MongoSourceId			varchar(50),
	@MongoDurationId		varchar(50),
	@ContactedOn		datetime,
	@ValidatedIntentity	varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @PatientId			INT,
			@RecordCreatedById	INT,
			@UpdatedById		INT,
			@MethodId			INT,
			@OutcomeId			INT,
			@WhoId				INT,
			@SourceId			INT,
			@DurationId			INT

	Select @PatientId = PatientId from RPT_Patient Where MongoId = @PatientMongoId

	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy

	If @UpdatedBy !=  ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy

	if @MongoMethodId != ' '
		Select @MethodId = NoteMethodId From [RPT_NoteMethodLookUp] Where MongoId = @MongoMethodId

	if @MongoOutcomeId != ' '
		Select @OutcomeId = NoteOutcomeId From [RPT_NoteOutcomeLookUp] Where MongoId = @MongoOutcomeId

	if @MongoWhoId != ' '
		Select @WhoId = NoteWhoId From [RPT_NoteWhoLookUp] Where MongoId = @MongoWhoId

	if @MongoSourceId != ' '
		Select @SourceId = NoteSourceId From [RPT_NoteSourceLookUp] Where MongoId = @MongoSourceId

	if @MongoDurationId != ' '
		Select @DurationId = NoteDurationId From [RPT_NoteDurationLookUp] Where MongoId = @MongoDurationId


	If Exists(Select Top 1 1 From RPT_PatientNote Where MongoId = @MongoID)
	Begin
		Update RPT_PatientNote
		Set [Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Text] = @Text,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements,
			MongoMethodId			= @MongoMethodId,
			MethodId			= @MethodId,
			[Type]				= @Type,
			MongoOutcomeId			= @MongoOutcomeId,
			OutcomeId			= @OutcomeId,
			MongoWhoId				= @MongoWhoId,
			WhoId				= @WhoId,
			MongoSourceId			= @MongoSourceId,
			SourceId			= @SourceId,
			MongoDurationId			= @MongoDurationId,
			DurationId			= @DurationId,
			ContactedOn			= @ContactedOn,
			ValidatedIntentity	= @ValidatedIntentity
		Where MongoId = @MongoID

	End
	Else
	Begin
		Insert Into RPT_PatientNote(
				ExtraElements,
				TTLDate,
				[Delete],
				LastUpdatedOn,
				PatientId,
				MongoPatientId,
				MongoRecordCreatedBy,
				RecordCreatedById,
				RecordCreatedOn,
				[Text],
				MongoUpdatedBy,
				UpdatedById,
				[Version],
				MongoId,
				MongoMethodId,
				MethodId,
				[Type],
				MongoOutcomeId,
				OutcomeId,
				MongoWhoId,
				WhoId,
				MongoSourceId,
				SourceId,
				MongoDurationId,
				DurationId,
				ContactedOn,
				ValidatedIntentity
			) values (
				@ExtraElements,
				@TTLDate,
				@Delete,
				@LastUpdatedOn,
				@PatientId,
				@PatientMongoId,
				@RecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,
				@Text,
				@UpdatedBy,
				@UpdatedById,
				@Version,
				@MongoID,
				@MongoMethodId,
				@MethodId,
				@Type,
				@MongoOutcomeId,
				@OutcomeId,
				@MongoWhoId,
				@WhoId,
				@MongoSourceId,
				@SourceId,
				@MongoDurationId,
				@DurationId,
				@ContactedOn,
				@ValidatedIntentity
			)
	End
END

GO







