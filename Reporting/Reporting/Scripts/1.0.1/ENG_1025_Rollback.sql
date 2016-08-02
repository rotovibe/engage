/****** Object:  Table [dbo].[RPT_BSHSI_HW2_Enrollment_Info]    Script Date: 05/14/2015 13:27:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_BSHSI_HW2_Enrollment_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info]
GO

/****** Object:  Table [dbo].[RPT_BSHSI_HW2_Enrollment_Info]    Script Date: 05/14/2015 13:27:47 ******/
CREATE TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[Priority] [varchar](50) NULL,
	[firstName] [varchar](100) NULL,
	[SystemId] [varchar](50) NULL,
	[LastName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[LSSN] [int] NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[Program_CM] [varchar](100) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Assigned_Date] [datetime] NULL,
	[Last_State_Update_Date] [datetime] NULL,
	[State] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Program_Completed_Date] [date] NULL,
	[Re_enrollment_Date] [date] NULL,
	[Enrolled_Date] [date] NULL,
	[Pending_Enrolled_Date] [date] NULL,
	[Enrollment_Action_Completion_Date] [date] NULL,
	[Market] [varchar](max) NULL,
	[Disenroll_Date] [date] NULL,
	[Disenroll_Reason] [varchar](max) NULL,
	[did_not_enroll_date] [date] NULL,
	[did_not_enroll_reason] [varchar](max) NULL
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]    Script Date: 05/14/2015 13:52:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]    Script Date: 05/14/2015 13:52:12 ******/
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
		did_not_enroll_reason
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

GO