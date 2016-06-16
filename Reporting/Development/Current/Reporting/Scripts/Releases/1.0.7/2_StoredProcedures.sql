----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_TruncateTables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_TruncateTables]    ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
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
	DELETE RPT_System
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
	
	DELETE RPT_PatientUtilization
	DELETE RPT_PatientUtilizationProgram

	--Delete the lookup columns in the end due to it's dependencies on base tables.

	-- Notes Lookups
	DELETE RPT_NoteMethodLookUp
	DELETE RPT_NoteOutcomeLookUp
	DELETE RPT_NoteSourceLookUp
	DELETE RPT_NoteWhoLookUp
	DELETE RPT_MaritalStatusLookUp
	DELETE RPT_StatusReasonLookUp
	-- Utilization lookups
	DELETE RPT_VisitTypeLookUp
	DELETE RPT_UtilizationLocationLookUp
	DELETE RPT_DispositionLookUp
	DELETE RPT_UtilizationSourceLookUp
	
	DELETE RPT_CareTeamFrequency
	DELETE RPT_ContactTypeLookUp
	DELETE RPT_CareTeam

	-- Resetting the Identity columns.
	
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
	
	DBCC CHECKIDENT ('RPT_System', RESEED, 0)
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
	
	DBCC CHECKIDENT ('RPT_PatientUtilization', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientUtilizationProgram', RESEED, 0)

	-- lookups
	DBCC CHECKIDENT ('RPT_NoteMethodLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteOutcomeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteSourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteWhoLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MaritalStatusLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_StatusReasonLookUp', RESEED, 0)
	-- utilization lookups
	DBCC CHECKIDENT ('RPT_VisitTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_UtilizationLocationLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_DispositionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_UtilizationSourceLookUp', RESEED, 0)

	--CareTeam 
	DBCC CHECKIDENT ('RPT_CareTeamFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CareTeam', RESEED, 0)

END

GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Assigned_PCM]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Assigned_PCM]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Assigned_PCM]
AS
BEGIN
	TRUNCATE TABLE [RPT_Flat_Assigned_PCM]
	INSERT INTO [RPT_Flat_Assigned_PCM]
	(
		[MongoPatientId],
		[MongoPCMContactId],
		[MongoCareTeamId],
		[FirstName],
		[LastName],
		[PreferredName],
		[PrimaryCareManagerMongoID]
	)
	SELECT 
		(SELECT c.MongoPatientId FROM RPT_Contact c WHERE MongoId = CT.MongoContactIdForPatient) AS MongoPatientId,		
		CT.MongoContactIdForCareMember as MongoPCMContactId,
		CT.MongoCareTeamId as MongoCareTeamId,
		C.FirstName,
		C.LastName,
		Case When C.PreferredName is Null Then C.FirstName Else C.PreferredName End as PreferredName,
		CT.MongoCareMemberId as PrimaryCareManagerMongoID
	FROM RPT_CareTeam CT 
		LEFT JOIN RPT_Contact C
			ON C.MongoId  =  CT.MongoContactIdForCareMember
	WHERE
		 CT.Status = 'Active' AND
		 CT.Core = 1 AND
		 CT.RoleId = '56f169f8078e10eb86038514' AND
		 CT.DeleteFlag = 'False' AND
		 CT.TTLDate IS NULL
END

GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1999
----------------------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BarrierStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BarrierStatistics]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[spPhy_RPT_Flat_BarrierStatistics]
As
Begin
	Truncate Table dbo.[RPT_Flat_BarrierStatistics]

	Insert Into dbo.[RPT_Flat_BarrierStatistics]
	(
		MongoBarrierId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,[Status]
		,Name
		,Details
		,Category
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
	)

	
	Select	   
		pb.MongoId as MongoBarrierId
		,pg.MongoPatientId
		,pb.MongoPatientGoalId as MongoGoalId
		,pb.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pb.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pb.[Status]
		,pb.Name
		,pb.Details
		,bcl.Name as Category
		,f.PrimaryCareManagerMongoID AS 'PrimaryCareManagerMongoId'
		,Case When f.PreferredName is Null Then f.FirstName Else f.PreferredName End AS 'PrimaryCareManagerPreferredName'
		--,u.MongoId AS 'PrimaryCareManagerMongoId'
		--,u.PreferredName AS 'PrimaryCareManagerPreferredName'
	From dbo.RPT_PatientBarrier pb with (nolock)
		LEFT OUTER JOIN dbo.RPT_BarrierCategoryLookUp as bcl with (nolock) on pb.MongoCategoryLookUpId = bcl.MongoId
		INNER JOIN dbo.RPT_PatientGoal pg with (nolock) on pb.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with (nolock) on pg.MongoPatientId = f.MongoPatientId
		
	--	LEFT OUTER JOIN dbo.[RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
	--	LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pb.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pb.MongoUpdatedBy = u3.MongoId	
	Where pb.[Delete] = 'False' and pb.TTLDate IS NULL 	
	and  pg.[Delete] = 'False' and pg.TTLDate IS NULL 	


End

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
AS
BEGIN
	DELETE [RPT_BSHSI_HW2_Enrollment_Info]
	INSERT INTO [RPT_BSHSI_HW2_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Pending_Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Market,
		Disenroll_Date,
		Disenroll_Reason,
		did_not_enroll_date,
		did_not_enroll_reason,
		Risk_Level,
		Acuity_Frequency
	) 
	SELECT DISTINCT 	
		pt.PatientId
		,ppt.PatientProgramId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility	
		,f.PreferredName AS [Assigned_PCM]  		
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = '541943a6bdd4dfa5d90002da'
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fb39ac80d30e00000067')) as [Enrollment]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57383ac80d31203000033', '53f57309ac80d31200000017' )) as [Program_Completed_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f572caac80d31203000020', '53f571e0ac80d3120300001b')) as [Re_enrollment_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f920ac80d30e00000066') ) as [Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f7ecac80d30e02000072')) as [Pending_Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f920ac80d30e00000066')) as [Enrollment_Action_Completion_Date]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			FROM fn_RPT_GetText_SingleSelect(pt.PatientId,ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fb39ac80d30e00000067') ) as [Market]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56eb7ac80d31203000001')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56f10ac80d31200000001') ) as [Disenroll_Reason]	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fc71ac80d30e02000074') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f885ac80d30e00000065')) as [did_not_enroll_reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f6ce5bac80d3138d000022', '53f6cc01ac80d3139000000d') ) as [Risk_Level]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f6ce5bac80d3138d000022', '53f6cc4cac80d3138d000012') )as [Acuity_Frequency]
	FROM
		RPT_Patient as pt with (nolock) 	
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pt.MongoID=f.MongoPatientID
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = '541943a6bdd4dfa5d90002da'
		AND ppt.[Delete] = 'False'
END


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_CareBridge]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_CareBridge]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_CareBridge]
AS
BEGIN
	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	
	DELETE [RPT_CareBridge_Enrollment_Info]
	INSERT INTO [RPT_CareBridge_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other		
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '5453f570bdd4dfcef5000330'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility
		,f.PreferredName as [ASSIGNED_PCM]			
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422dd36ac80d3356d000001') )as [Practice]			
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_CareBridge_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, '5453c6c7bdd4dfc94e000012','54255fa0890e942ba2000001')) as [Enrollment]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '5425620b890e942ba2000005')) as [Program_Completed_Date] --       	       	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '54255ff8890e942ba2000002') ) as [Enrolled_Date]  --      	     	
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '54255fa0890e942ba2000001')) as [Enrollment_Action_Completion_Date] --
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5453c6c7bdd4dfc94e000012', '542561b4890e942ba1000003')) as [Disenroll_Date] --	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5453c6c7bdd4dfc94e000012', '542561ec890e942ba1000004') ) as [Disenroll_Reason] --
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other] --
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5453c6c7bdd4dfc94e000012', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason] --
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5453c6c7bdd4dfc94e000012', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other] --
	FROM
		RPT_Patient as pt with (nolock) 	
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pt.MongoID=f.MongoPatientID
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'
END


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Engage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
AS
BEGIN
	Truncate Table [RPT_Engage_Enrollment_Info]

	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7';

	DECLARE @CoMorbidDisease TABLE
	(
	  Name	varchar(50),
	  ActionSourceId varchar(50),
	  StepSourceId varchar(50)
	)

	--version 1 @ProgramSourceId = '5465e772bdd4dfb6d80004f7'
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('HTN', '5457cd63890e94338f000053', '544f0347ac80d37bc0000283')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Heart Failure', '5457cd63890e94338f000053', '544f0295ac80d37bc0000282')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('COPD', '5457cd63890e94338f000053', '544f0461ac80d37bc20002d8')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Diabetes', '5457cd63890e94338f000053', '544f0545ac80d37bc20002db')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Asthma', '5457cd63890e94338f000053', '544f0483ac80d37bc0000286')

	--version 2 @ProgramSourceId = '54b69910ac80d33c2c000032'
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('HTN', '54aadace890e9480450001d3', '544f0347ac80d37bc0000283')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Heart Failure', '54aadace890e9480450001d3', '548ef479ac80d33c2c000001')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('COPD', '54aadace890e9480450001d3', '548efc22ac80d33c2c000002')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Diabetes', '54aadace890e9480450001d3', '544f0545ac80d37bc20002db')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Asthma', '54aadace890e9480450001d3', '544f0483ac80d37bc0000286')
	
	--version 3 @ProgramSourceId = '55ca3880ac80d35b8e00053e'
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('HTN', '55c248aaac80d31c4a000709', '55ad0692ac80d308d20004b6')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Heart Failure', '55c248aaac80d31c4a000709', '55ad04b8ac80d308d0000604')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('COPD', '55c248aaac80d31c4a000709', '55ad3cd4ac80d308d20004d5')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Diabetes', '55c248aaac80d31c4a000709', '55ad3e0aac80d308d20004d8')
	Insert Into @CoMorbidDisease(Name, ActionSourceId, StepSourceId) Values('Asthma', '55c248aaac80d31c4a000709', '55ad42bcac80d308d00006a2')
	
	INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score,
		HTN,
		Heart_Failure,
		COPD,
		Diabetes,
		Asthma,
		Comorbid_Disease		
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility	
		,f.PreferredName as [ASSIGNED_PCM]		
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545cee7a890e9458aa000003', '544efd6fac80d37bc000027b') )as [Practice]		
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255fa0890e942ba2000001')) as [Enrollment] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f571e0ac80d3120300001b')) as [Re_enrollment_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5450ff07ac80d37bc00002f6', '542561b4890e942ba1000003')) as [Disenroll_Date] 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '542561ec890e942ba1000004') ) as [Disenroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425600e890e942ba2000003') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] 
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5457cd63890e94338f000053', '544f0347ac80d37bc0000283')) as [HTN]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5457cd63890e94338f000053', '544f0295ac80d37bc0000282')) as [Heart_Failure]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5457cd63890e94338f000053', '544f0461ac80d37bc20002d8')) as [COPD]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5457cd63890e94338f000053', '544f0545ac80d37bc20002db')) as [Diabetes]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5457cd63890e94338f000053', '544f0483ac80d37bc0000286')) as [Asthma]
		,STUFF(
				(select ', ' + CASE WHEN Value = 'Yes' THEN cm.Name ELSE NULL END 
					From @CoMorbidDisease cm 
						OUTER APPLY fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, cm.ActionSourceId, cm.StepSourceId)
					Where cm.ActionSourceId = '5457cd63890e94338f000053'
					For Xml Path, Type
				).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as CoMorbid_Disease
	FROM
		RPT_Patient as pt with (nolock) 	
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pt.MongoID=f.MongoPatientID
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'


---------------------------- version 2 ----------------------------------------

SET @ProgramSourceId = '54b69910ac80d33c2c000032'; 
INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score,
		HTN,
		Heart_Failure,
		COPD,
		Diabetes,
		Asthma,
		Comorbid_Disease
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '54b69910ac80d33c2c000032'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility		
		,f.PreferredName as [ASSIGNED_PCM]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'545cee7a890e9458aa000003', '544efd6fac80d37bc000027b') )as [Practice] 
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId,'545c0805ac80d36bd4000089', '54255fa0890e942ba2000001')) as [Enrollment]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '54a3625d890e948042000052', '54942ba8ac80d33c29000019')) as [Re_enrollment_Date] --*  	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'545c0805ac80d36bd4000089', '542561b4890e942ba1000003')) as [Disenroll_Date] --*
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '545c0805ac80d36bd4000089', '542561ec890e942ba1000004') ) as [Disenroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --*
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '545c0805ac80d36bd4000089', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date] --(step, action)
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] --	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'54aadace890e9480450001d3', '544f0347ac80d37bc0000283')) as [HTN]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'54aadace890e9480450001d3', '548ef479ac80d33c2c000001')) as [Heart_Failure]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'54aadace890e9480450001d3', '548efc22ac80d33c2c000002')) as [COPD]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'54aadace890e9480450001d3', '544f0545ac80d37bc20002db')) as [Diabetes]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'54aadace890e9480450001d3', '544f0483ac80d37bc0000286')) as [Asthma]
		,STUFF(
				(select ', ' + CASE WHEN Value = 'Yes' THEN cm.Name ELSE NULL END 
					From @CoMorbidDisease cm
						OUTER APPLY fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, cm.ActionSourceId, cm.StepSourceId)
					Where cm.ActionSourceId = '54aadace890e9480450001d3'
					For Xml Path, Type
				).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as CoMorbid_Disease
	FROM
		RPT_Patient as pt with (nolock) 	
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pt.MongoID=f.MongoPatientID
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'
	
	
---------------------------- version 3 ----------------------------------------
SET @ProgramSourceId = '55ca3880ac80d35b8e00053e';

INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score,
		HTN,
		Heart_Failure,
		COPD,
		Diabetes,
		Asthma,
		Comorbid_Disease		
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '55ca3880ac80d35b8e00053e'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility	
		,f.PreferredName as [ASSIGNED_PCM]				
	,( COALESCE( (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '543d38bbac80d33fda00002a')), 
			(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '550afd42ac80d36b310005a3')))
			)as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '544efd6fac80d37bc000027b' ) )as [Practice]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_V3_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId,'55bfa837ac80d31c4c0001ce', '54255fa0890e942ba2000001')) as [Enrollment]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '54a3625d890e948042000052', '54942ba8ac80d33c29000019')) as [Re_enrollment_Date] --*  	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55bfa837ac80d31c4c0001ce', '542561b4890e942ba1000003')) as [Disenroll_Date] --*
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55bfa837ac80d31c4c0001ce', '542561ec890e942ba1000004') ) as [Disenroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --*
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55bfa837ac80d31c4c0001ce', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date] --(step, action)
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] --	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad0692ac80d308d20004b6')) as [HTN]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad04b8ac80d308d0000604')) as [Heart_Failure]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3cd4ac80d308d20004d5')) as [COPD]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3e0aac80d308d20004d8')) as [Diabetes]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad42bcac80d308d00006a2')) as [Asthma]
		,STUFF(
				(select ', ' + CASE WHEN Value = 'Yes' THEN cm.Name ELSE NULL END 
					From @CoMorbidDisease cm
						OUTER APPLY fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, cm.ActionSourceId, cm.StepSourceId)
					Where cm.ActionSourceId = '55c248aaac80d31c4a000709'
					For Xml Path, Type
				).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as CoMorbid_Disease	
	FROM
		RPT_Patient as pt with (nolock) 	
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pt.MongoID=f.MongoPatientID
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'

END




GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_GoalStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_GoalStatistics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create Procedure [dbo].[spPhy_RPT_Flat_GoalStatistics]
As
Begin
	Truncate Table [RPT_Flat_GoalStatistics]

	Create Table #GoalAttribute (
		MongoPatientGoalId VARCHAR(50),
		GoalAttributeName  VARCHAR(100),
		GoalAttributeValue VARCHAR(50)
	)

	Insert Into #GoalAttribute(
		MongoPatientGoalId, 
		GoalAttributeName, 
		GoalAttributeValue
		)
		Select pga.MongoPatientGoalId, ga.Name, gao.Value
			From RPT_PatientGoalAttribute pga with (nolock)
				LEFT OUTER JOIN RPT_GoalAttribute ga with (nolock) on pga.MongoGoalAttributeId = ga.MongoId
				LEFT OUTER JOIN RPT_PatientGoalAttributeValue pgav with (nolock) on pga.PatientGoalAttributeId = pgav.PatientGoalAttributeId
				LEFT OUTER JOIN RPT_GoalAttributeOption gao with (nolock) on pgav.Value = gao.[Key] and ga.MongoId = gao.MongoGoalAttributeId

	Insert Into [RPT_Flat_GoalStatistics]
	(
		MongoGoalId, 
		MongoPatientId,
		Confidence, 
		Importance, 
		StageofChange, 
		CreatedOn, 
		CreatedBy, 
		UpdatedOn, 
		UpdatedBy, 
		Name, 
		Details, 
		TemplateId, 
		[Source],
		TargetDate, 
		TargetValue, 
		[Status], 
		StartDate, 
		EndDate,
		FocusAreas,
		[Type],
		PrimaryCareManagerMongoId,
		PrimaryCareManagerPreferredName
	)
	Select 
		pg.MongoId as MongoGoalId
		,pg.MongoPatientId
		,gasub1.GoalAttributeValue as Confidence
		,gasub2.GoalAttributeValue as Importance
		,gasub3.GoalAttributeValue as 'StageofChange'
		,pg.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pg.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pg.Name
		,pg.Details
		,pg.TemplateId 
		,pgl.Name as Source
		,pg.TargetDate
		,pg.TargetValue
		,pg.[Status]
		,pg.StartDate
		,pg.EndDate
		, STUFF(
				(Select
					Distinct '| ' + pgfa.Name
				From RPT_PatientGoalFocusArea pgf with (nolock)
					LEFT OUTER JOIN RPT_FocusAreaLookUp pgfa with (nolock) on pgf.MongoFocusAreaId = pgfa.MongoId
				Where pgf.MongoPatientGoalId = pg.MongoId
				Order By '| ' + pgfa.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as FocusAreas
		,pg.[Type] as [Type]
		,f.PrimaryCareManagerMongoID AS 'PrimaryCareManagerMongoId'
		,Case When f.PreferredName is Null Then f.FirstName Else f.PreferredName End AS 'PrimaryCareManagerPreferredName'
	From RPT_PatientGoal as pg with (nolock) 
		LEFT OUTER JOIN RPT_SourceLookUp as pgl with (nolock) on pg.Source = pgl.MongoId
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Confidence'
				) gasub1 ON pg.MongoId = gasub1.MongoPatientGoalId
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Importance'
				) gasub2 ON pg.MongoId = gasub2.MongoPatientGoalId				
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Stage of Change'
				) gasub3 ON pg.MongoId = gasub3.MongoPatientGoalId
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f WITH(NOLOCK) on pg.MongoPatientId = f.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pg.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pg.MongoUpdatedBy = u3.MongoId							
	Where pg.[Delete] = 'False' and pg.TTLDate IS NULL

	Drop Table #GoalAttribute

End



GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_InterventionStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_InterventionStatistics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[spPhy_RPT_Flat_InterventionStatistics]
As
Begin
	Truncate Table dbo.[RPT_Flat_InterventionStatistics]

	Insert Into dbo.[RPT_Flat_InterventionStatistics]
	(
		MongoInterventionId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,ClosedDate
		,[Status]
		,StartDate
		,DueDate
		,[Description]
		,Details
		,CategoryName
		,TemplateId
		,AssignedTo
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
		,InterventionBarriers
	)
	Select	   
		pi.MongoId as MongoInterventionId
		,pg.MongoPatientId
		,pi.MongoPatientGoalId as MongoGoalId
		,pi.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pi.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pi.ClosedDate
		,pi.[Status]
		,pi.StartDate
		,pi.DueDate
		,pi.Description
		,pi.Details
		,icl.Name as CategoryName
		,pi.TemplateId		
		,u1.PreferredName as AssignedTo
		,f.PrimaryCareManagerMongoID AS 'PrimaryCareManagerMongoId'
		,Case When f.PreferredName is Null Then f.FirstName Else f.PreferredName End AS 'PrimaryCareManagerPreferredName'
		,STUFF(
				(Select
					Distinct '| ' + pb.Name
				From RPT_PatientInterventionBarrier pib with (nolock) 
					LEFT OUTER JOIN RPT_PatientBarrier pb with (nolock) on pib.MongoPatientBarrierId = pb.MongoId
				Where pib.MongoPatientInterventionId = pi.MongoId
				Order By '| ' + pb.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as InterventionBarriers
	From dbo.RPT_PatientIntervention pi with (nolock)
		INNER JOIN dbo.RPT_PatientGoal pg with (nolock) on pi.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN dbo.RPT_InterventionCategoryLookUp icl with (nolock) on pi.MongoCategoryLookUpId = icl.MongoId
		LEFT OUTER JOIN dbo.RPT_User u1 with (nolock) on pi.MongoContactUserId = u1.MongoId
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f WITH(NOLOCK) on pg.MongoPatientId = f.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pi.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pi.MongoUpdatedBy = u3.MongoId	
	Where pi.[Delete] = 'False' and pi.TTLDate IS NULL 							
	and  pg.[Delete] = 'False' and pg.TTLDate IS NULL 	

End

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Latest_PatientObservations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Latest_PatientObservations]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[spPhy_RPT_Flat_Latest_PatientObservations]
AS
BEGIN
	TRUNCATE TABLE [RPT_Flat_Latest_PatientObservations]

	INSERT INTO [RPT_Flat_Latest_PatientObservations]
	(
		[MongoPatientId],
		[ObservationType],
		[Code],
		[CommonName],
		[Description],
		[State],
		[StartDate],
		[EndDate],
		[NumericValue],
		[NonNumericValue],
		[PrimaryCareManagerMongoId],
		[PrimaryCareManagerFirstName],
		[PrimaryCareManagerLastName],
		[PrimaryCareManagerPreferredName]
	)
	SELECT 
		obs.MongoPatientId,
		obs.ObservationType,
		obs.Code,
		obs.CommonName,
		obs.[Description],
		obs.[State],
		obs.StartDate,
		obs.EndDate,
		obs.NumericValue,
		obs.NonNumericValue
		,f.PrimaryCareManagerMongoID AS 'PrimaryCareManagerMongoId'
		,f.FirstName AS 'PrimaryCareManagerFirstName'
		,f.LastName AS 'PrimaryCareManagerLastName'
		,Case When f.PreferredName is Null Then f.FirstName Else f.PreferredName End AS 'PrimaryCareManagerPreferredName'
	FROM (SELECT ROW_NUMBER() OVER (PARTITION BY MongoPatientId, ObservationType, [Code], [Description]  ORDER BY StartDate DESC, LastUpdatedOn DESC) AS RowNumber,
			MongoPatientId,
			ObservationType,
			Code,
			[Description],
			CommonName,
			[State],
			StartDate,
			EndDate,
			NumericValue,
			NonNumericValue
		FROM RPT_Patient_ClinicalData pcd WITH(NOLOCK)
		) obs
	INNER JOIN	RPT_Patient p WITH(NOLOCK)	ON obs.MongoPatientId = p.MongoId
	LEFT OUTER JOIN RPT_Flat_Assigned_PCM f WITH(NOLOCK) on obs.MongoPatientId = f.MongoPatientId
	WHERE obs.RowNumber = 1
	and p.[Delete] = 'False' and p.TTLDate IS NULL

END



GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_PatientNotes_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_PatientNotes_Dim]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[spPhy_RPT_Flat_PatientNotes_Dim]
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
		f.PreferredName as [ASSIGNED_PCM],
		 (SELECT TOP 1  		
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
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pn.MongoPatientID=f.MongoPatientID
		left outer join RPT_PatientNoteProgram pnp with (nolock) on pn.MongoId = pnp.MongoPatientNoteId
		INNER JOIN RPT_PATIENT PT with (nolock) ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS with (nolock) ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP with (nolock) ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u with (nolock) ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Delete] = 'False'	
END


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_PatientUtilization_Dim]
	INSERT INTO [RPT_PatientUtilization_Dim]
	(
		[PatientUtilizationId],
		[MongoPatientUtilizationId],
		[NoteType],
		[Reason],
		[VisitType],
		[OtherVisitType],
		[AdmitDate],
		[Admitted],
		[DischargeDate],
		[Length],
		[Location],
		[OtherLocation],
		[Disposition],
		[OtherDisposition],	
		[UtilizationSource],
		[DataSource],
		[ProgramName],
		[PatientId],
		[MongoPatientId],
		[FirstName],
		[MiddleName],
		[LastName],
		[DateOfBirth],
		[Age],
		[Gender],
		[Priority],
		[SystemId],
		[Assigned_PCM],
		[AssignedTo],
		[State],
		[StartDate],
		[EndDate],
		[AssignedOn],
		[RecordCreatedOn],
		[RecordCreated_By],
		[RecordUpdatedOn],
		[RecordUpdatedBy]
	)
	SELECT
		pn.PatientUtilizationId,
		pn.MongoId,
		(SELECT DISTINCT Name FROM RPT_NoteTypeLookUp WHERE MongoId = pn.MongoNoteTypeId) as [NoteType],
		pn.Reason,
		(SELECT DISTINCT Name FROM RPT_VisitTypeLookUp nm WHERE nm.MongoId = pn.MongoVisitTypeId) as [VisitType],
		pn.OtherVisitType,
		pn.AdmitDate,
		(CASE WHEN pn.Admitted = 'True' THEN 'Yes' WHEN pn.Admitted = 'False' THEN 'No' END) as Admitted,
		pn.DischargeDate,
		(CASE WHEN (pn.AdmitDate IS NULL AND  pn.DischargeDate IS NULL) THEN NULL
			  WHEN (pn.AdmitDate IS NULL OR  pn.DischargeDate IS NULL) THEN 1
			  WHEN (pn.AdmitDate IS NOT NULL AND pn.DischargeDate IS NOT NULL)  THEN  DATEDIFF(DAY, pn.AdmitDate, pn.DischargeDate) 
		 END) as Length, 
		(SELECT DISTINCT Name FROM RPT_UtilizationLocationLookUp nw WHERE nw.MongoId = pn.MongoLocationId) as [Location],
		pn.OtherLocation,
		(SELECT DISTINCT Name FROM RPT_DispositionLookUp ns WHERE ns.MongoId= pn.MongoDispositionId) as [Disposition],
		pn.OtherDisposition,
		(SELECT DISTINCT Name FROM RPT_UtilizationSourceLookUp noc WHERE noc.MongoId = pn.MongoUtilizationSourceId) as [UtilizationSource],
		pn.DataSource,
		pp.Name as [ProgramName],
		PT.PATIENTID,
		PT.MongoId,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DateOfBirth],
		CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age,	
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID,
		f.PreferredName as [ASSIGNED_PCM],
	(SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo],	
		pp.[State],
		pp.AttributeStartDate as [StartDate],
		pp.[AttributeEndDate]  as [EndDate],
		pp.[AssignedOn],
		pn.RecordCreatedOn,
		u.PreferredName,
		pn.LastUpdatedOn,
		(SELECT PreferredName FROM RPT_User WHERE MongoId = pn.MongoUpdatedBy)
	FROM 
		RPT_PatientUtilization pn with (nolock)
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pn.MongoPatientID=f.MongoPatientID
		LEFT OUTER JOIN RPT_PatientUtilizationProgram pnp with (nolock) on pn.MongoId = pnp.MongoPatientUtilizationId
		INNER JOIN RPT_PATIENT PT with (nolock) ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS with (nolock) ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP with (nolock) ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u with (nolock) ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Delete] = 'False'	
END


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TaskStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TaskStatistics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[spPhy_RPT_Flat_TaskStatistics]
As
Begin
	Truncate Table dbo.[RPT_Flat_TaskStatistics]

	Insert Into dbo.[RPT_Flat_TaskStatistics]
	(
		MongoTaskId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,ClosedDate
		,[Status]
		,StartDate
		,TargetDate
		,TargetValue
		,[Description]
		,Details
		,TemplateId
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
		,TaskBarriers
	)
	Select	   
		pt.MongoId as MongoTaskId
		,pg.MongoPatientId
		,pt.MongoPatientGoalId as MongoGoalId
		,pt.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pt.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pt.ClosedDate
		,pt.[Status]
		,pt.StartDate
		,pt.TargetDate
		,pt.TargetValue
		,pt.[Description]
		,pt.Details
		,pt.TemplateId
		,f.PrimaryCareManagerMongoID AS 'PrimaryCareManagerMongoId'
		,Case When f.PreferredName is Null Then f.FirstName Else f.PreferredName End AS 'PrimaryCareManagerPreferredName'
		,STUFF(
				(Select
					Distinct '| ' + pb.Name
				From RPT_PatientTaskBarrier ptb with (nolock) 
					LEFT OUTER JOIN RPT_PatientBarrier pb with (nolock) on ptb.MongoPatientBarrierId = pb.MongoId
				Where ptb.MongoPatientTaskId = pt.MongoId
				Order By '| ' + pb.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as TaskBarriers
	
	From dbo.RPT_PatientTask pt with (nolock)
		INNER JOIN dbo.RPT_PatientGoal pg with (nolock) on pt.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with (nolock) on pg.MongoPatientId = f.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pt.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pt.MongoUpdatedBy = u3.MongoId	
	Where pt.[Delete] = 'False' and pt.TTLDate IS NULL 	
	and  pg.[Delete] = 'False' and pg.TTLDate IS NULL 	
					

End

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TouchPoint_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
		f.PreferredName as [ASSIGNED_PCM],
	 (SELECT TOP 1  		
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
		LEFT OUTER JOIN RPT_Flat_Assigned_PCM f with(nolock) on pn.MongoPatientID=f.MongoPatientID
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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_PatientInformation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_PatientInformation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spPhy_RPT_PatientInformation]
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
		, [Background]
		,[ClinicalBackGround]
		,[DataSource]
		,[MaritalStatus]
		,[Protected]
		,[Deceased]
		,[Status]
		,[Reason]
		,[StatusDataSource]
		,[Minor]
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
		,[PrimaryId]
		,[PrimaryIdSystem]
		,[EngageId]
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
		, pt.Background
		, pt.ClinicalBackGround
		, pt.DataSource
		, (SELECT TOP 1 ms.Name FROM RPT_MaritalStatusLookUp ms where pt.MongoMaritalStatusId = ms.MongoId)
		,(CASE WHEN pt.Protected = 'True' THEN 'Yes' WHEN pt.Protected = 'False' THEN 'No' END)
		, pt.Deceased
		, pt.[Status]
		, (SELECT TOP 1 sr.Name FROM RPT_StatusReasonLookUp sr where pt.MongoReasonId = sr.MongoId)
		, pt.StatusDataSource
		, NULL
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
		, f.PreferredName as Assigned_PCM
	, pt.LSSN
		,(select top 1 [value] from rpt_patientsystem with (nolock)  where mongopatientid = pt.MongoId and [primary] = 'True' and [Delete] = 'False' and TTLDate is null ORDER BY RecordCreatedOn DESC) AS [PrimaryId]
		,(select rs.displaylabel from rpt_system rs with (nolock)  where rs.[DeleteFlag] = 'False' and rs.TTLDate is null and rs.MongoId in (select top 1 [SysId] from rpt_patientsystem with (nolock)  where mongopatientid = pt.MongoId and [primary] = 'True' and [Delete] = 'False' ORDER BY RecordCreatedOn DESC)) AS [PrimaryIdSystem]
		,(select top 1 [value] from RPT_PatientSystem with (nolock)  where [Delete] = 'False' and TTLDate is null and  MongoRecordCreatedBy = '5368ff2ad4332316288f3e3e' and SysId = '559d8453d433232ca04b3131' and MongoPatientId = pt.MongoId) AS [EngageId]					
	FROM 
		RPT_Patient pt with (nolock) 
		LEFT JOIN RPT_Contact c with (nolock) ON c.MongoPatientId = pt.MongoId
		LEFT JOIN RPT_Flat_Assigned_PCM f with (nolock) ON pt.MongoId = f.MongoPatientId
	WHERE 
		pt.[Delete] = 'False'
		AND pt.[TTLDate] IS NULL
	-- Update the MINOR column based on the calculated AGE.
	UPDATE RPT_PatientInformation
	SET Minor = (CASE WHEN AGE is null THEN NULL WHEN AGE < 18 THEN 'Yes' WHEN AGE >= 18 THEN 'No' END)



GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
AS
BEGIN
	DELETE [RPT_Patient_PCM_Program_Info]
	INSERT INTO [RPT_Patient_PCM_Program_Info]
	(
		PatientId,
		FirstName,				
		MiddleName,				
		LastName,				
		DateOfBirth,
		Age,
		Gender,
		[Priority],
		SystemId,
		Assigned_PCM,
		PatientProgramId,		
		ProgramName,
		[State],
		Assigned_Date,
		StartDate,
		EndDate,				
		Program_CM,
		[MongoPatientId],
		[MongoPatientProgramId],
		[LastStateUpdateDate],
		[GraduatedFlag],
		[Eligibility],
		[Enrollment]
	) 
	SELECT DISTINCT 	
		pt.PatientId
		, pt.FirstName
		, pt.MiddleName
		, pt.LastName
		, pt.DateOfBirth
		,CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age
		, pt.Gender
		,pt.[Priority]
		, ps.SystemId
		, f.PreferredName as Assigned_PCM
	,ppt.PatientProgramId
		,ppt.Name
		,ppt.[State] as [State] 
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]
		,pt.MongoId as [MongoPatientId]
		,ppt.MongoId as [MongoPatientProgramId]	
		,ppt.StateUpdatedOn as [LastStateUpdateDate]
		,ppa.GraduatedFlag as [GraduatedFlag]
		,ppa.Eligibility
		,ppa.Enrollment			
	FROM
		RPT_Patient as pt with (nolock) 
		LEFT JOIN RPT_Flat_Assigned_PCM f with (nolock) ON pt.MongoId = f.MongoPatientId	
		LEFT JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId and ppt.[Delete] = 'False' and ppt.TTLDate IS NULL
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId		
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.MongoId = ps.MongoPatientId
	WHERE
		pt.[Delete] = 'False' and pt.TTLDate IS NULL
END


GO





