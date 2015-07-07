/*** ENG-1288 ***/
/*** ENG-1284 ***/
/**** [fn_RPT_ActionCompleted_Selected] ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Selected]'))
	DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Selected]
GO

CREATE FUNCTION [fn_RPT_ActionCompleted_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(100)	
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			(CASE WHEN p.[Delete] = 'False' THEN	
				CASE 
					WHEN p.[Value] = 'false' THEN
						'No'
					WHEN p.[Value] = '' THEN
						'No'						
					WHEN p.[Value] = 'true' THEN
						'Yes'
					ELSE
						NULL
				END						
			ELSE 						
				'0'
			END) AS [Value]
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
			p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.[Text] = @Text
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid	
	RETURN
END
GO

/**** [fn_RPT_ActionNotCompleted_Selected] ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Selected]'))
	DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Selected]
GO

CREATE FUNCTION [fn_RPT_ActionNotCompleted_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(100)	
)
RETURNS @ActionNotCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionNotCompletedTable
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
			AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.[Text] = @Text
			AND p.patientprogramid = @patientprogramid	
	RETURN
END
GO

/**** [fn_RPT_ActionSaved_Selected] ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Selected]'))
	DROP FUNCTION [fn_RPT_ActionSaved_Selected]
GO

CREATE FUNCTION [fn_RPT_ActionSaved_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(100)	
)
RETURNS @ActionSavedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionSavedTable
		SELECT 
			CASE WHEN p.[Selected] = 'True' THEN
				CASE 
					WHEN p.[Value] = 'false' THEN
						'No'
					WHEN p.[Value] = '' THEN
						'No'								
					WHEN p.[Value] = 'true' THEN
						'Yes'
					ELSE
						NULL
				END						
			ELSE
				'0'
			END
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.[Delete] = 'True'
			
			AND p.[Text] = @Text
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid	
	RETURN
END
GO

/*** [fn_RPT_GetText_Selected] ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText_Selected]'))
	DROP FUNCTION [fn_RPT_GetText_Selected]
GO

CREATE FUNCTION [dbo].[fn_RPT_GetText_Selected] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50),
	@Text VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text) where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Selected(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId, @Text) WHERE [Value] != '0'
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
						CASE 
							WHEN pr13.[Value] = 'false' THEN
								'No'
							WHEN pr13.[Value] = '' THEN
								'No'
							WHEN pr13.[Value] = 'true' THEN
								'Yes'
							ELSE
								NULL
						END						
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
				AND pr13.[Text] = @Text
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
						AND pr14.[Text] = @Text
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO

/****** Object:  Table [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight]    Script Date: 06/22/2015 14:50:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Program_Details_By_Individual_Healthy_Weight]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight](
	[PatientId] [int] NOT NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoPatientProgramId] [varchar](50) NOT NULL,
	[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
	[Provider_Name] [varchar](5000) NULL,
	[Pre_Survey_Date_Administered] [datetime] NULL,
	[Post_Survey_Date_Administered] [datetime] NULL,
	[Pending_Enrollment_Referral_Date] [datetime] NULL,
	[Enrolled_Date] [datetime] NULL,
	[Final_Call_Date] [datetime] NULL,
	[Market] [varchar](1000) NULL,
	[Date_did_not_enroll] [varchar](1000) NULL,
	[Date_did_not_enroll_Reason] [varchar](5000) NULL,
	[Enrollment_General_Comments] [varchar](5000) NULL,
	[Disenrolled_Date] [datetime] NULL,
	[Disenrolled_Reason] [varchar](5000) NULL,
	[Disenrolled_General_Comments] [varchar](5000) NULL,
	[Re_enrollment_Date] [datetime] NULL,
	[Re_enrollment_Reason] [varchar](5000) NULL,
	[Program_Completed_Date] [datetime] NULL,
	[Program_Completed_General_Comments] [varchar](5000) NULL,
	[Risk_Level] [varchar](1000) NULL,
	[Acuity_Level] [varchar](1000) NULL,
	[PHQ2_Total_Point_Score] [varchar](1000) NULL,
	[Referral_Provided_Depression_EAP] [varchar](1000) NULL,
	[Referral_Provided_Depression_Community_Resources] [varchar](1000) NULL,
	[Referral_Provided_Depression_Participant_Declined] [varchar](1000) NULL,
	[Referral_Provided_Depression_Not_Applicable] [varchar](1000) NULL,
	[Referral_Provided_Depression_Other] [varchar](1000) NULL,		
	[Other_Referral_Information_Depression] [varchar](1000) NULL,
	[Depression_Screening_General_Comments] [varchar](5000) NULL
) ON [PRIMARY]

GO

/*** [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight] ***/
IF (OBJECT_ID('[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]') IS NOT NULL)
  DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '5330920da38116ac180009d2';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Pending_Enrollment_Referral_Date],
		[Enrolled_Date],
		[Final_Call_Date],
		[Market],
		[Date_did_not_enroll],
		[Date_did_not_enroll_Reason],
		[Enrollment_General_Comments],
		[Disenrolled_Date],
		[Disenrolled_Reason],
		[Disenrolled_General_Comments],
		[Re_enrollment_Date],
		[Re_enrollment_Reason],
		[Program_Completed_Date],
		[Program_Completed_General_Comments],
		[Risk_Level],
		[Acuity_Level],
		[PHQ2_Total_Point_Score],
		[Referral_Provided_Depression_EAP],
		[Referral_Provided_Depression_Community_Resources] ,
		[Referral_Provided_Depression_Participant_Declined] ,
		[Referral_Provided_Depression_Not_Applicable],
		[Referral_Provided_Depression_Other],		
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '5330920da38116ac180009d2';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fb32c34786626c000029' ) ) AS [Do_you_currently_have_a_PCP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fff2c34786626c00002a'  ) ) AS [Provider_Name] 
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531a304bc347860424000109' , '531a2ec7c347860424000023' )) AS [Pre_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446' , '532c3de1f8efe368860003b2'  )) AS [Pending_Enrollment_Referral_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e1ef8efe368860003b3')) AS [Enrolled_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
		from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329c5f3a381166015000074', '530f844ef8efe307660001ab')) as [Final_Call_Date]		
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e76c347865db8000001')) AS [Market]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3f40c347865db8000002')) AS Date_did_not_enroll
	,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c45bff8efe36886000446', '532c3fc2f8efe368860003b5'))	AS [Date_did_not_enroll_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '530f844ef8efe307660001ab'  ) )	AS [Enrollment_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '532c4061f8efe368860003b6')) AS [Disenrolled_Date]
	,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c46b3c347865db8000092', '532c407fc347865db8000003') ) AS [Disenrolled_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '530f844ef8efe307660001ab') ) AS [Disenrolled_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40d0f8efe368860003b7')) AS [Re_enrollment_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40f3f8efe368860003b8') ) AS [Re_enrollment_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '532c411bf8efe368860003b9')) AS [Program_Completed_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '530f844ef8efe307660001ab' ) ) AS [Program_Completed_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50d60c34786662e000001'  ) ) AS [Risk_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50dc3c34786662e000002') ) AS [Acuity_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54de1c34786660a0000de') ) AS [PHQ2_Total_Point_Score]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'EAP') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Community Resources') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Participant Declined') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Not Applicable') )	AS [Referral_Provided_Depression_Not_Applicable]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009', 'Other')) AS [Referral_Provided_Depression_Other]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54e8cc34786660a0000e0')) AS [Other_Referral_Information_Depression]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '530f844ef8efe307660001ab') ) AS [Depression_Screening_General_Comments]
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO

/****** Object:  Table [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2]    Script Date: 06/22/2015 15:22:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

CREATE TABLE [dbo].[RPT_Program_Details_By_Individual_Healthy_Weight2](
	[PatientId] [int] NOT NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoPatientProgramId] [varchar](50) NOT NULL,
	[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
	[Provider_Name] [varchar](5000) NULL,
	[Pre_Survey_Date_Administered] [datetime] NULL,
	[Post_Survey_Date_Administered] [datetime] NULL,
	[Enrollment_General_Comments] [varchar](5000) NULL,
	[Final_Call_Date] [datetime] NULL,
	[Disenrolled_General_Comments] [varchar](5000) NULL,
	[Re_enrollment_Reason] [varchar](5000) NULL,
	[Program_Completed_General_Comments] [varchar](5000) NULL,
	[PHQ2_Total_Point_Score] [varchar](1000) NULL,
	[Referral_Provided_Depression_EAP] [varchar](1000) NULL,
	[Referral_Provided_Depression_Community_Resources] [varchar](1000) NULL,
	[Referral_Provided_Depression_Participant_Declined] [varchar](1000) NULL,
	[Referral_Provided_Depression_Not_Applicable] [varchar](1000) NULL,
	[Referral_Provided_Depression_Other] [varchar](1000) NULL,		
	[Other_Referral_Information_Depression] [varchar](1000) NULL,
	[Depression_Screening_General_Comments] [varchar](5000) NULL
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]    Script Date: 06/22/2015 15:32:37 ******/
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
		[Final_Call_Date],
		[Disenrolled_General_Comments],
		[Re_enrollment_Reason],
		[Program_Completed_General_Comments],
		[PHQ2_Total_Point_Score],
		[Referral_Provided_Depression_EAP],
		[Referral_Provided_Depression_Community_Resources],
		[Referral_Provided_Depression_Participant_Declined],
		[Referral_Provided_Depression_Not_Applicable],
		[Referral_Provided_Depression_Other],
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
	,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
		from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b40dac80d330cb0001af', '540698fdac80d330c800001c')) as [Final_Call_Date]				
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57115ac80d31203000014', '53f4f8a0ac80d30e02000073') ) AS [Disenrolled_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f5720bac80d3120300001c') ) AS [Re_enrollment_Reason] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57383ac80d31203000033', '53f4f8a0ac80d30e02000073' ) ) AS [Program_Completed_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '540694b2ac80d330cb000010' ) ) AS [PHQ2_Total_Point_Score] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END	
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', 'EAP') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Community Resources') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Participant Declined') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Not Applicable') )	AS [Referral_Provided_Depression_Not_Applicable]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_Selected(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406954dac80d330c8000013', ' Other')) AS [Referral_Provided_Depression_Other]		
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