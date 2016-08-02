/******* [RPT_Program_Details_By_Individual_Healthy_Weight] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
(
[PatientId] [int] NOT NULL,
[MongoPatientId] [VARCHAR](50) NOT NULL,
[PatientProgramId] [int] NOT NULL,
[MongoPatientProgramId] [VARCHAR](50) NOT NULL,
[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
[Provider_Name] [varchar](5000) NULL,
[Pre_Survey_Date_Administered] [datetime] NULL,
[Post_Survey_Date_Administered] [datetime] NULL,
[Pending_Enrollment_Referral_Date] [datetime] NULL,
[Enrolled_Date] [datetime] NULL,
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
[Other_Referral_Information_Depression] [varchar](1000) NULL,
[Depression_Screening_General_Comments] [varchar](5000) NULL,
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight];
GO
CREATE PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
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
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e76c347865db8000001')) AS Market
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
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54e8cc34786660a0000e0') ) AS [Other_Referral_Information_Depression]
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
