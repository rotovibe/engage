ALTER PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE RPT_CareMember
	DELETE RPT_CareMemberTypeLookUp
	DELETE RPT_ContactEmail
	DELETE RPT_ContactPhone
	DELETE RPT_ContactAddress
	DELETE RPT_ContactRecentList
	DELETE RPT_ContactMode
	DELETE RPT_ContactLanguage
	DELETE RPT_ContactWeekDay
	DELETE RPT_ContactTimeOfDay
	-- todos
	DELETE RPT_ToDoProgram
	DELETE RPT_ToDo
	-- patient programs
	DELETE RPT_SpawnElements
	DELETE RPT_SpawnElementTypeCode
	DELETE RPT_PatientProgramAttribute
	DELETE RPT_PatientProgramResponse
	DELETE RPT_PatientProgramStep
	DELETE RPT_PatientProgramAction	
	DELETE RPT_PatientProgramModule
	DELETE RPT_PatientProgram	
	DELETE RPT_PatientNoteProgram
	DELETE RPT_PatientNote
	DELETE RPT_PatientProblem
	DELETE RPT_ObjectiveCategory
	DELETE RPT_ObjectiveLookUp	
	DELETE RPT_PatientObservation	
	DELETE RPT_Observation
	DELETE RPT_PatientTaskAttributeValue
	DELETE RPT_PatientTaskAttribute
	DELETE RPT_PatientTaskBarrier
	DELETE RPT_PatientTask
	-- patient allergies
	DELETE RPT_Allergy
	DELETE RPT_AllergyType
	DELETE RPT_PatientAllergy
	DELETE RPT_PatientAllergyReaction
	-- patient medsupps
	DELETE RPT_PatientMedSuppPhClass
	DELETE RPT_MedPharmClass
	DELETE RPT_PatientMedSuppNDC
	DELETE RPT_PatientMedSupp	
	DELETE RPT_Medication
	DELETE RPT_MedicationMap
	DELETE RPT_PatientMedFrequency
	DELETE RPT_CustomPatientMedFrequency
	
	-- patient goal
	DELETE RPT_PatientGoalProgram
	DELETE RPT_PatientGoalFocusArea
	DELETE RPT_GoalAttributeOption	
	DELETE RPT_PatientGoalAttributeValue
	DELETE RPT_PatientGoalAttribute
	DELETE RPT_GoalAttribute
	DELETE RPT_PatientInterventionBarrier
	DELETE RPT_PatientIntervention	
	DELETE RPT_PatientBarrier	
	DELETE RPT_PatientGoal
	DELETE RPT_PatientUser	
	DELETE RPT_Contact
	DELETE RPT_PatientSystem
	DELETE RPT_Patient
	DELETE RPT_CommTypeCommMode
	DELETE RPT_ToDoCategoryLookUp	
	DELETE RPTMongoCategoryLookUp
	DELETE RPT_SourceLookUp
	DELETE RPT_BarrierCategoryLookUp
	DELETE RPT_InterventionCategoryLookUp
	DELETE RPTMongoTimeZoneLookUp
	DELETE RPT_ProblemLookUp
	DELETE RPT_TimesOfDayLookUp
	DELETE RPT_CommTypeLookUp
	DELETE RPT_CommModeLookUp
	DELETE RPT_StateLookUp
	DELETE RPT_LanguageLookUp
	DELETE RPT_FocusAreaLookUp
	DELETE RPT_CodingSystemLookUp
	DELETE RPT_ObservationTypeLookUp
	DELETE RPT_AllergyTypeLookUp
	DELETE RPT_AllergySourceLookUp
	DELETE RPT_SeverityLookUp
	DELETE RPT_ReactionLookUp
	DELETE RPT_MedSupTypeLookUp
	DELETE RPT_FreqHowOftenLookUp
	DELETE RPT_FreqWhenLookUp
	DELETE RPT_NoteTypeLookUp
	DELETE RPT_UserRecentList
	DELETE [RPT_User]
	
	--DELETE CohortPatientView	
	--DELETE CohortPatientViewSearchField
	
	DBCC CHECKIDENT ('RPT_CareMember', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_CareMemberTypeLookUp', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_ContactLanguage', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactWeekDay', RESEED, 0)  
	DBCC CHECKIDENT ('RPT_ContactTimeOfDay', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactRecentList', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_ContactMode', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactPhone', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactAddress', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactEmail', RESEED, 0)
	
-- reseed program tables
	DBCC CHECKIDENT ('RPT_PatientProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SpawnElements', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramModule', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAction', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramStep', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramResponse', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAttribute', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_PatientNoteProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientNote', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProblem', RESEED, 0)

	-- allergies
	DBCC CHECKIDENT ('RPT_AllergyType', RESEED, 0)	
	DBCC CHECKIDENT ('RPT_Allergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedicationMap', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Medication', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CustomPatientMedFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedPharmClass', RESEED,0)
	
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SeverityLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ReactionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedSupTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqHowOftenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqWhenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteTypeLookUp', RESEED, 0)

	DBCC CHECKIDENT ('RPT_ObjectiveCategory', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObjectiveLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientObservation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Observation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTask', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientGoalProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalFocusArea', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttributeOption', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttribute', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientInterventionBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientIntervention', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoal', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientUser', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Contact', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientSystem', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_Patient', RESEED, 0) 
	
	DBCC CHECKIDENT ('RPT_CommTypeCommMode', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_BarrierCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_InterventionCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoTimeZoneLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ProblemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_TimesOfDayLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommModeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_StateLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_LanguageLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FocusAreaLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CodingSystemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObservationTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_UserRecentList', RESEED, 0)
	DBCC CHECKIDENT ('RPT_User', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_ToDoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDoProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDo', RESEED, 0)
	
	-- patient allergies
	DBCC CHECKIDENT ('RPT_PatientAllergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientAllergyReaction', RESEED, 0)
	
	--DBCC CHECKIDENT ('RPT_CohortPatientView', RESEED, 0)
	--DBCC CHECKIDENT ('RPT_CohortPatientViewSearchField', RESEED, 0)
END
GO

/* RPT_patientSystem */
ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientSystem] 
	@MongoID varchar(50),
	@PatientMongoId varchar(50),
	@Label varchar(50),
	@SystemId varchar(50),
	@SystemName varchar(50),
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ReturnID	INT,
			@PatientId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @PatientMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientSystem Where MongoId = @MongoID)
	Begin
		Update RPT_PatientSystem
		Set [Delete] = @Delete,
			Label = @Label,
			SystemId = @SystemId,
			SystemName = @SystemName,
			LastUpdatedOn = @LastUpdatedOn,
			PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			TTLDate = @TimeToLive,
			ExtraElements = @ExtraElements
		Where MongoId = @MongoID
		
		Select @ReturnID = PatientSystemId From RPT_PatientSystem Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientSystem
			(ExtraElements, 
			[Delete], 
			Label, 
			SystemId, 
			SystemName, 
			LastUpdatedOn, 
			PatientId, 
			MongoPatientId, 
			MongoRecordCreatedBy, 
			RecordCreatedById, 
			RecordCreatedOn, 
			MongoUpdatedBy, 
			UpdatedById, 
			[Version], 
			TTLDate, 
			MongoId) 
		values 
			(@ExtraElements, 
			@Delete, 
			@Label, 
			@SystemId, 
			@SystemName, 
			@LastUpdatedOn, 
			@PatientId, 
			@PatientMongoId, 
			@RecordCreatedBy, 
			@RecordCreatedById, 
			@RecordCreatedOn, 
			@UpdatedBy, 
			@UpdatedById, 
			@Version, 
			@TimeToLive, 
			@MongoID)
		Select @ReturnID = @@IDENTITY
		
	End
	
	Update RPT_Patient
	Set DisplayPatientSystemId = @ReturnID,
		MongoPatientSystemId = @MongoID
	Where MongoId = @PatientMongoId
	
	Select @ReturnID
END
GO

/* [spPhy_RPT_PatientInformation] */
ALTER PROCEDURE [dbo].[spPhy_RPT_PatientInformation]
AS
	DELETE [RPT_PatientInformation]
	INSERT INTO [RPT_PatientInformation]
	(
		 [PatientId]
		, [MongoId]
		, [firstName] 
		, [LastName]
		, [MiddleName]
		, [Suffix]
		, [DateOfBirth]
		, [AGE]
		, [Gender]	
		, [Priority]			
		,[SystemId]
		,[SystemName]
		,[TimeZone]
		,[Phone_1]
		,[Phone_2]
		,[Email_1]
		,[Email_1_Preferred]	
		,[Email_1_Type]
		,[Address_1]
		,[Address_2]
		,[Address_3]
		,[Address_City]
		,[Address_State]
		,[Address_ZIP_Code]
		,[Assigned_PCM]
		,[LSSN]	
	)
	SELECT 
		pt.PatientId
		, pt.MongoId
		, pt.firstName 
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.DateOfBirth
		, (CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END) AS [AGE]
		, pt.Gender	
		, pt.[Priority]			
		, (SELECT TOP 1 ps.SystemId FROM RPT_PatientSystem ps with (nolock) WHERE ps.MongoPatientId = pt.MongoId) as SystemId
		, (SELECT TOP 1 ps.SystemName FROM RPT_PatientSystem ps with (nolock) WHERE ps.MongoPatientId = pt.MongoId) as SystemName
		, (SELECT TOP 1 tz.Name FROM RPTMongoTimeZoneLookUp AS tz with (nolock) WHERE tz.MongoId = c.MongoTimeZone) AS [TimeZone]
		, (SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp with (nolock) WHERE cp.MongoContactId = c.MongoId AND cp.OptOut != 'True' AND cp.[Delete] = 'false') as [Phone_1]
		, (SELECT TOP 1
				CASE WHEN (SELECT COUNT(*) from RPT_ContactPhone cp with (nolock) where cp.MongoContactId = c.MongoId) > 1
				THEN
					 t.Number
				ELSE
					NULL
				END 		 
			FROM ( SELECT TOP 2 d.PhoneId, d.Number, d.contactId FROM (SELECT cp.PhoneId, cp.Number, cp.ContactId, cp.PhonePreferred from RPT_ContactPhone cp with (nolock) where cp.MongoContactId = c.MongoId AND cp.OptOut != 'True' AND cp.[Delete] = 'false') as d ORDER BY d.PhoneId DESC) as t) as [Phone_2]
		,(SELECT  TOP 1   ce.[Text] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId)) as [Email_1]
		,(SELECT  TOP 1   ce.[Preferred] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId) ORDER BY ce.Preferred DESC ) as [Email_1_Preferred]	
		,(SELECT  TOP 1 (SELECT TOP 1 Name FROM dbo.RPT_CommTypeLookUp AS t WITH (nolock) WHERE (t.MongoId = ce.MongoCommTypeId)) AS [Type] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId) ORDER BY ce.Preferred DESC ) as [Email_1_Type]
		,(SELECT TOP 1 Line1 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_1]
		,(SELECT TOP 1 Line2 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_2]
		,(SELECT TOP 1 Line3 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_3]
		,(SELECT TOP 1 City FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_City]
		,(SELECT TOP 1 (SELECT Code FROM dbo.RPT_StateLookUp AS st WITH (nolock) WHERE (st.MongoId = ca.MongoStateId)) AS [State] FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_State]
		,(SELECT TOP 1 PostalCode FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_ZIP_Code]
		,(SELECT     
			  (SELECT (SELECT u.PreferredName
						FROM dbo.RPT_CareMember AS cm WITH (nolock) 
							INNER JOIN dbo.RPT_User AS u ON cm.MongoUserId = u.MongoId
						WHERE     (cm.CareMemberId = c.CareMemberId)) AS [preferred name]
				FROM dbo.RPT_CareMember AS c WITH (nolock)
				WHERE (c.MongoPatientId = ptn.MongoId)) AS [pref_name]
		  FROM dbo.RPT_Patient AS [ptn] WITH (nolock)
		  WHERE (ptn.[Delete] = 'False') AND ptn.MongoId = pt.MongoId) as [Assigned_PCM]
		, pt.LSSN			
	FROM 
		RPT_Patient pt with (nolock) 
		LEFT JOIN RPT_Contact c with (nolock) ON c.MongoPatientId = pt.MongoId
	WHERE 
		pt.[Delete] = 'False'
		AND pt.[TTLDate] IS NULL

GO