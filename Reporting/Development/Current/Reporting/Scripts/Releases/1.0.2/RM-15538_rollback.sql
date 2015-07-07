/*** ENG-725 ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalMetrics]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientGoalMetrics]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientGoalMetrics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
GO

/*** ENG-1149 ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient_ClinicalData]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient_ClinicalData]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientClinicalData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
GO

/*** ENG-1146 ***/
/*** RPT_Medication ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Medication')
	DROP TABLE [RPT_Medication];
GO

/*** [RPT_MedPharmClass] ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedPharmClass')
	DROP TABLE [RPT_MedPharmClass];
GO

/*** RPT_MedicationMap ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedicationMap')
	DROP TABLE [RPT_MedicationMap];
GO

/**** RPT_PatientMedSupp ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSupp]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_PatientMedSupp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientMedSupp]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp]
GO

/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedFrequency]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_PatientMedFrequency]
GO

/*** ENG-1255 ***/
/*** sproc automation ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_SprocNames')
	DROP TABLE RPT_SprocNames;
GO


/***** spPhy_RPT_Execute_Sproc ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Execute_Sproc')
	DROP PROCEDURE [spPhy_RPT_Execute_Sproc];
GO

/***** [spPhy_RPT_Load_Controller] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Load_Controller')
	DROP PROCEDURE [dbo].[spPhy_RPT_Load_Controller];
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/15/2015 13:34:55 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Initialize_Flat_Tables')
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	DECLARE @StartTime Datetime;
	
	SET @StartTime = GETDATE();
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_ProgramResponse_Flat', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));

	SET @StartTime = GETDATE();
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract],[Time]) VALUES ('spPhy_RPT_Flat_BSHSI_HW2', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));

	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_CareBridge', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_Engage];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_Engage', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));	
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_Observations_Dim];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_Observations_Dim', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));		

	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_TouchPoint_Dim];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_TouchPoint_Dim', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));	
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_SavePatientInfo];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_SavePatientInfo', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));		
END


/*** [RPT_PatientInformation] ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_PatientInformation')
	DROP TABLE [RPT_PatientInformation]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_PatientInformation')
	DROP PROCEDURE [spPhy_RPT_PatientInformation];
GO

/******* [RPT_Program_Details_By_Individual_Healthy_Weight2] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight2')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2];
GO

/******* [RPT_Program_Details_By_Individual_Healthy_Weight] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight];
GO


/**** [spPhy_RPT_Flat_BSHSI_HW2] ****/
ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
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
		,( select Market FROM dbo.fn_RPT_Market(pt.PatientId,ppt.PatientProgramId) ) as [Market]
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

/*** ENG-734 ***/
/***************** TO DO METRICS *******************/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Dim_ToDo]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Dim_ToDo]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_ToDo_Dim]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
GO

/*** RPT_SprocName ***/
DELETE RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_ToDo_Dim'
GO

/*** ENG-1288 ***/
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]    Script Date: 06/22/2015 15:37:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight2]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight2]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Enrollment_General_Comments],
		[Disenrolled_General_Comments],
		[Re_enrollment_Reason],
		[Program_Completed_General_Comments],
		[PHQ2_Total_Point_Score],
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca09ac80d31390000001' ) ) AS [Do_you_currently_have_a_PCP] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca4aac80d31390000002'  ) ) AS [Provider_Name]  --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4ea64ac80d30e00000021' , '53f4c273ac80d30e00000009' )) AS [Pre_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4fd75ac80d30e00000083', '53f4f8a0ac80d30e02000073'  ) )	AS [Enrollment_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57115ac80d31203000014', '53f4f8a0ac80d30e02000073') ) AS [Disenrolled_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f5720bac80d3120300001c') ) AS [Re_enrollment_Reason] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57383ac80d31203000033', '53f4f8a0ac80d30e02000073' ) ) AS [Program_Completed_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '540694b2ac80d330cb000010' ) ) AS [PHQ2_Total_Point_Score] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406956aac80d330c8000014') ) AS [Other_Referral_Information_Depression] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406957eac80d330cb000011') ) AS [Depression_Screening_General_Comments] --*
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'

GO

/*** ENG-1289 ***/
ALTER FUNCTION [dbo].[fn_RPT_PCPOther] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	----------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 582;
	--SET @patientprogramid = 38;
	--SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422df97ac80d3356d000004';
	------------------------------------

		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	------
	--select @CompletedCount;
	--select @SavedCount;
	--select @ActionNotComplete;
	------
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = 'True' THEN
					CASE WHEN pr7.[Delete] = 'False' THEN
						pr7.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				ps7.SourceId= @StepSourceId
				AND pa7.SourceId = @ActionSourceId
				AND pp7.SourceId = @ProgramSourceId
				AND pp7.[Delete] = 'False'
				AND pr7.Selected = 'False'
				AND pa7.[State] IN ('Completed')
				AND pa7.Archived = 'True'
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT 
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							ps8.SourceId = @StepSourceId
							AND pa8.SourceId = @ActionSourceId
							AND pp8.SourceId = @ProgramSourceId
							AND (pp8.[Delete] = 'False')
							AND pa8.[State] IN ('Completed')
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
GO


ALTER FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Delete] = 'False' AND (p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 )THEN
					p.[Value]		
			ELSE 						
				'0'
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
GO

ALTER FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			'0' as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.StepCompleted = 'False'
			--AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO

/*** fn_RPT_GetValue ***/
ALTER FUNCTION [dbo].[fn_RPT_GetValue] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' THEN
						pr13.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				--AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO

/*** ENG-940 ***/