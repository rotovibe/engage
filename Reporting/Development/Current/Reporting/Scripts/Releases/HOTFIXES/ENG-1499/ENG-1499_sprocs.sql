-- [spPhy_RPT_Flat_Engage]
ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
AS
BEGIN
	DELETE [RPT_Engage_Enrollment_Info]

	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7';
	
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
		acuity_score		
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
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [Practice]			
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
	FROM
		RPT_Patient as pt with (nolock) 	
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
		acuity_score		
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
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [Practice]
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
	FROM
		RPT_Patient as pt with (nolock) 	
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


-- [spPhy_RPT_Flat_CareBridge]
ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_CareBridge]
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
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
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


-- [spPhy_RPT_Flat_TouchPoint_Dim]
ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
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
	[AssignedOn]		
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


