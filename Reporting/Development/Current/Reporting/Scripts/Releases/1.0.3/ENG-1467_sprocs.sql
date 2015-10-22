-- recreate original sproc
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientInfo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
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
		LEFT JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId and ppt.[Delete] = 'False' and ppt.TTLDate IS NULL
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId		
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.MongoId = ps.MongoPatientId
	WHERE
		pt.[Delete] = 'False' and pt.TTLDate IS NULL
END
GO