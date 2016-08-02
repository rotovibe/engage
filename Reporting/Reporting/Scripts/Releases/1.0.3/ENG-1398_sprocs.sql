IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]
AS
BEGIN
	DELETE [RPT_Flat_Program_Enrollment_Beacon]

	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '552578ae890e946c5200004e';
	
	INSERT INTO [RPT_Flat_Program_Enrollment_Beacon]
	(
		[PatientId]
		,[PatientProgramId]
		,[ProgramName]
		,[Practice]
		,[Referring_Physician]		
		,[PCP]
		,[Program_Completed_Date]
		,[Re_enrollment_Date]
		,[Enrolled_Date]
		,[Enrollment_Action_Completion_Date]
		,[Disenroll_Date]
		,[Disenroll_Reason]
		,[Other_Disenroll_Reason]
		,[did_not_enroll_date]
		,[Did_Not_Enroll_Reason]
		,[Did_Not_Enroll_Reason_Other]
		,[Did_not_enroll_exclusion_criteria]
		,[Engage_Program_Transferred]
		,[Disenrolled_exclusion_criteria]
		,[Completion_By_Death]
		,[Consistancy_With_Advance_directives]
		,[Hospice_Care_Till_Death]
		,[Wish_To_Die_At_Home_Honored]
		,[Referral_Date]
		,[Referral_Source]
		,[Reason_For_Referral]
		,[Other_Referral_Source]
	)
	--DECLARE @ProgramSourceId varchar(50);
	--SET @ProgramSourceId = '552578ae890e946c5200004e';	
	SELECT DISTINCT 	
		pt.MongoId as [PatientId]
		,ppt.MongoId as [PatientProgramId] 
		,ppt.Name as [ProgramName]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetPractice(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '551315b6ac80d3236a00048d', '544efd6fac80d37bc000027b') )as [Practice]			
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55267b57ac80d32867000124', '544efaecac80d37bc20002d1') )as [Referring_Physician]		
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetPCP_Beacon(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a0ccac80d3236a000150', '5425620b890e942ba2000005')) as [Program_Completed_Date]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a12fac80d3236a000195', '54942ba8ac80d33c29000019')) as [Re_enrollment_Date]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a0ccac80d3236a000150', '54255ff8890e942ba2000002') ) as [Enrolled_Date]
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a0ccac80d3236a000150', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5511a0ccac80d3236a000150', '542561b4890e942ba1000003')) as [Disenroll_Date] 	
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '542561ec890e942ba1000004') ) as [Disenroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a0ccac80d3236a000150', '54264df1890e942ba2000006') )as [Other_Disenroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a0ccac80d3236a000150', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --*
		,(select CASE WHEN LEN(Reason)> 0 THEN Reason ELSE NULL END FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5511a0ccac80d3236a000150', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other]
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '543d38bbac80d33fda00002a') ) as [Did_not_enroll_exclusion_criteria]
		,(select CASE WHEN LEN(Reason)> 0 THEN Reason ELSE NULL END FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '550afb89ac80d36b310005a2')) as [Engage_Program_Transferred]		
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '550afd42ac80d36b310005a3') ) as [Disenrolled_exclusion_criteria]
		,(select CASE WHEN LEN(Reason)> 0 THEN Reason ELSE NULL END FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '550b2952ac80d36b310005b1')) as [Completion_By_Death]
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '550b2c6aac80d36a7300030d') ) as [Consistancy_With_Advance_directives]		
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '550b2c8fac80d36a7300030e') ) as [Hospice_Care_Till_Death]
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5511a0ccac80d3236a000150', '550c2ecfac80d3236700000a') ) as [Wish_To_Die_At_Home_Honored]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55267b57ac80d32867000124', '541a7eebac80d3319e000001')) as [Referral_Date]			
		,(select CASE WHEN LEN(Market)> 0 THEN Market ELSE NULL END  FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55267b57ac80d32867000124', '544efa5dac80d37bc0000277') ) as [Referral_Source]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END FROM dbo.fn_RPT_GetValue(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55267b57ac80d32867000124', '55084891ac80d36a7300019d')) as [Reason_For_Referral]					
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55267b57ac80d32867000124', '541a7eccac80d3319b000002') ) as [Other_Referral_Source]		
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