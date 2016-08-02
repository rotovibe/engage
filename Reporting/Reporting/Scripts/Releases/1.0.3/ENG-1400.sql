-- alter proc
ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
AS
BEGIN
	DELETE [RPT_Patient_PCM_Program_Info]
	INSERT INTO [RPT_Patient_PCM_Program_Info]
	(
		PatientId,
		[MongoPatientId],
		PatientProgramId,
		[MongoPatientProgramId],				
		ProgramName,
		[State],
		Assigned_Date,
		StartDate,
		EndDate,
		[LastStateUpdateDate],
		[GraduatedFlag],
		[Eligibility],
		[Enrollment],						
		Program_CM
	) 
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId as [MongoPatientId]
		,ppt.PatientProgramId
		,ppt.MongoId as [MongoPatientProgramId]		
		,ppt.Name
		,ppt.[State] as [State] 
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 
		,ppt.StateUpdatedOn as [LastStateUpdateDate]
		,ppa.GraduatedFlag as [GraduatedFlag]
		,ppa.Eligibility
		,ppa.Enrollment
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]
	FROM
		RPT_Patient as pt with (nolock) 	
		LEFT JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId and ppt.[Delete] = 'False' and ppt.TTLDate IS NULL
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId		
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.MongoId = ps.MongoPatientId
	WHERE
		pt.[Delete] = 'False' and pt.TTLDate IS NULL
END
GO