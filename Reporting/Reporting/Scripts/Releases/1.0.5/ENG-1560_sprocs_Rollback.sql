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
	@ValidatedIntentity	varchar(50),
	@DataSource varchar (50),
	@Duration int
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
			ValidatedIntentity	= @ValidatedIntentity,
			DataSource = @DataSource,
			Duration = @Duration
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
				ValidatedIntentity,
				DataSource,
				Duration			
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
				@ValidatedIntentity,
				@DataSource,
				@Duration
			)
	End
END
GO

DROP PROCEDURE [dbo].[spPhy_RPT_Flat_PatientNotes_Dim]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_PatientNotes_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_PatientNotes_Dim]
	INSERT INTO [RPT_PatientNotes_Dim]
	(
	[PatientNoteId],
	[MongoPatientId],
	[Type],
	[Method],
	[Who],
	[Source],
	[Outcome],
	[ContactedOn],
	[ProgramName],
	[Duration],
	[ValidatedIntentity],
	[Text],
	[RecordCreatedOn],
	[Record_Created_By],
	[PATIENTID],
	[FIRSTNAME],
	[MIDDLENAME],
	[LASTNAME],
	[DATEOFBIRTH],
	[AGE],
	[GENDER],
	[PRIORITY],
	[SYSTEMID],
	[ASSIGNED_PCM],
	[ASSIGNEDTO],
	[State],
	[StartDate],
	[EndDate],
	[AssignedOn],
	[RecordUpdatedOn],
	[RecordUpdatedBy],
	[DataSource],
	[DurationInt]		
	)
	SELECT
		pn.PatientNoteId,
		PT.MongoId,
		(SELECT DISTINCT Name FROM RPT_NoteTypeLookUp WHERE MongoId = pn.[Type]) as [Type],
		(SELECT DISTINCT Name FROM RPT_NoteMethodLookUp nm WHERE nm.MongoId = pn.MongoMethodId) as [Method],
		(SELECT DISTINCT Name FROM RPT_NoteWhoLookUp nw WHERE nw.MongoId = pn.MongoWhoId) as [Who],
		(SELECT DISTINCT Name FROM RPT_NoteSourceLookUp ns WHERE ns.MongoId= pn.MongoSourceId) as [Source],
		(SELECT DISTINCT Name FROM RPT_NoteOutcomeLookUp noc WHERE noc.MongoId = pn.MongoOutcomeId) as [Outcome],
		pn.ContactedOn,
		pp.Name as [ProgramName],
		(SELECT DISTINCT Name FROM RPT_NoteDurationLookUp nd WHERE nd.MongoId = pn.MongoDurationId) as [Duration],
		pn.ValidatedIntentity,
		pn.Text,
		pn.RecordCreatedOn,
		u.PreferredName as [Record_Created_By],
		PT.PATIENTID,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DATEOFBIRTH],
		CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age,	
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID,
		(SELECT TOP 1	
			U.PREFERREDNAME 	  
		  FROM
			RPT_PATIENT AS P,
			RPT_USER AS U,
			RPT_CAREMEMBER AS C 	 		 	  
		  WHERE
			P.MONGOID = C.MONGOPATIENTID
			AND C.MONGOUSERID = U.MONGOID
			AND P.MongoId = PT.MongoId 	) AS [ASSIGNED_PCM]
		, (SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo]
		, pp.[State]
		, pp.AttributeStartDate as [StartDate]
		, pp.[AttributeEndDate]  as [EndDate]
		, pp.[AssignedOn]
		, pn.LastUpdatedOn
		, (SELECT PreferredName FROM RPT_User WHERE MongoId = pn.MongoUpdatedBy)
		, pn.DataSource
		, pn.Duration
	FROM 
		RPT_PatientNote pn with (nolock)
		left outer join RPT_PatientNoteProgram pnp with (nolock) on pn.MongoId = pnp.MongoPatientNoteId
		INNER JOIN RPT_PATIENT PT with (nolock) ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS with (nolock) ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP with (nolock) ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u with (nolock) ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Delete] = 'False'	
END
GO

DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_TouchPoint_Dim]
	INSERT INTO [RPT_TouchPoint_Dim]
	(
		[MongoPatientId],
		[PatientNoteId],
		[Method],
		[Who],
		[Source],
		[Outcome],
		[ContactedOn],
		[ProgramName],
		[Duration],
		[ValidatedIntentity],
		[Text],
		[RecordCreatedOn],
		[Record_Created_By],
		[PATIENTID],
		[FIRSTNAME],
		[MIDDLENAME],
		[LASTNAME],
		[DATEOFBIRTH],
		[AGE],
		[GENDER],
		[PRIORITY],
		[SYSTEMID],
		[ASSIGNED_PCM],
		[ASSIGNEDTO],
		[State],
		[StartDate],
		[EndDate],
		[AssignedOn],
		[RecordUpdatedOn],
		[RecordUpdatedBy],
		[DataSource],
		[DurationInt]	
	)
	SELECT
		PT.MongoId,
		pn.PatientNoteId,
		(SELECT DISTINCT Name FROM RPT_NoteMethodLookUp nm WHERE nm.MongoId = pn.MongoMethodId) as [Method],
		(SELECT DISTINCT Name FROM RPT_NoteWhoLookUp nw WHERE nw.MongoId = pn.MongoWhoId) as [Who],
		(SELECT DISTINCT Name FROM RPT_NoteSourceLookUp ns WHERE ns.MongoId= pn.MongoSourceId) as [Source],
		(SELECT DISTINCT Name FROM RPT_NoteOutcomeLookUp noc WHERE noc.MongoId = pn.MongoOutcomeId) as [Outcome],
		pn.ContactedOn,
		pp.Name as [ProgramName],
		(SELECT DISTINCT Name FROM RPT_NoteDurationLookUp nd WHERE nd.MongoId = pn.MongoDurationId) as [Duration],
		pn.ValidatedIntentity,
		pn.Text,
		pn.RecordCreatedOn,
		u.PreferredName as [Record_Created_By],
		PT.PATIENTID,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DATEOFBIRTH],
		CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age,	
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID,
		(SELECT TOP 1	
			U.PREFERREDNAME 	  
		  FROM
			RPT_PATIENT AS P,
			RPT_USER AS U,
			RPT_CAREMEMBER AS C 	 		 	  
		  WHERE
			P.MONGOID = C.MONGOPATIENTID
			AND C.MONGOUSERID = U.MONGOID
			AND P.MongoId = PT.MongoId 	) AS [ASSIGNED_PCM]
		, (SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo]
		, pp.[State]
		, pp.AttributeStartDate as [StartDate]
		, pp.[AttributeEndDate]  as [EndDate]
		, pp.[AssignedOn]
		, pn.LastUpdatedOn
		, (SELECT PreferredName FROM RPT_User WHERE MongoId = pn.MongoUpdatedBy)
		, pn.DataSource
		, pn.Duration
	FROM 
		RPT_PatientNote pn
		left outer join RPT_PatientNoteProgram pnp on pn.MongoId = pnp.MongoPatientNoteId
		INNER JOIN RPT_PATIENT PT ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Type] = '54909997d43323251c0a1dfe'
		and pn.[Delete] = 'False'
END
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveNoteDurationLookUp] 
	@MongoId varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_NoteDurationLookUp Where MongoId = @MongoId)
	Begin
		Update RPT_NoteDurationLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default,
			[Active] = @Active
		Where 
			MongoId = @MongoId		
	End
	Else
	Begin
		Insert Into RPT_NoteDurationLookUp(
			LookUpType, 
			Name, 
			[Default], 
			MongoId, 
			Active
		) values (
			@LookUpType, 
			@Name, 
			@Default, 
			@MongoId, 
			@Active
		)
	End
END
GO




