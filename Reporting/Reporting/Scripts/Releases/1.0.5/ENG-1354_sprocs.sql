
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Transition]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Transition]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Transition]
AS
BEGIN
	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '566f8316ac80d3662b00036f';
	
	Truncate Table [RPT_Flat_Transition_Info]

	INSERT INTO [RPT_Flat_Transition_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		Enrollment_Status,
		Enrolled_Date,
		Did_Not_Enroll_Date,
		Did_Not_Enroll_Reason,
		Disenroll_Date,
		Disenroll_Reason,
		Program_Completed_Date,
		Call_Within_48Hours_PostDischarge,
		Discharge_Type,
		Total_LACE_Score,
		High_Risk_For_Readmission,
		Program_Status
	)

	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId as MongoPatientId
		,ppt.PatientProgramId
		,ppt.MongoId as MongoPatientProgramId
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, '55177225ac80d37c8000038c', '54fdd419ac80d329bf000064')) as [Enrollment_Status]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55177225ac80d37c8000038c', '54fdd477ac80d329bf000065') ) as [Enrolled_Date]     	     	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55177225ac80d37c8000038c', '54fdd48eac80d32a86000081') ) as [Did_Not_Enroll_Date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55177225ac80d37c8000038c', '54fdd49fac80d329bf000066')) as [Did_Not_Enroll_Reason]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55177225ac80d37c8000038c', '54fdd51eac80d32a86000082')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55177225ac80d37c8000038c', '54fdd530ac80d329bf000069') ) as [Disenroll_Reason] 
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55177225ac80d37c8000038c', '54fdd649ac80d329bf00006b')) as [Program_Completed_Date]     	       	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d2a8ac80d308d000031e', '55a576f5ac80d308d00001e7')) as [Call_Within_48Hours_PostDischarge]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d2a8ac80d308d000031e', '55a7bce0ac80d308d20002dc')) as [Discharge_Type]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d828ac80d308d0000373', '55a57377ac80d308d2000126')) as [Total_LACE_Score]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d828ac80d308d0000373', '55a575ffac80d308d00001e6')) as [High_Risk_For_Readmission]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55f0d598ac80d37770000156', '55f0d4fbac80d377730001e6')) as [Program_Status]
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId	
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'

END
