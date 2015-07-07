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

