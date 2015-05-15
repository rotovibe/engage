/****** Object:  Table [dbo].[RPT_TouchPoint_Dim]    Script Date: 05/15/2015 16:23:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_TouchPoint_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_TouchPoint_Dim]
GO


/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TouchPoint_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
AS
BEGIN
	DELETE [RPT_TouchPoint_Dim]
	INSERT INTO [RPT_TouchPoint_Dim]
	(
	[PatientNoteId],
	[Method],
	[Who],
	[Source],
	[Outcome],
	[ContactedOn],
	[Name],
	[Duration],
	[ValidatedIntentity],
	[Text],
	[RecordCreatedOn],
	[Record_Created_By],
	[PATIENTID],
	[FIRSTNAME],
	[LASTNAME],
	[DATEOFBIRTH],
	[AGE],
	[GENDER],
	[PRIORITY],
	[SYSTEMID],
	[ASSIGNED_PCM],
	[State],
	[StartDate],
	[EndDate],
	[AssignedOn]		
	)
	SELECT 
		pn.PatientNoteId,
		nm.Name as [Method],
		nw.Name as [Who],
		ns.Name as [Source],
		noc.Name as [Outcome],
		pn.ContactedOn,
		pp.Name,
		nd.Name as [Duration],
		pn.ValidatedIntentity,
		pn.Text,
		pn.RecordCreatedOn,
		u.LastName + ', ' + u.FirstName as [Record_Created_By],
		PT.PATIENTID,
		PT.FIRSTNAME,
		PT.LASTNAME,
		PT.DATEOFBIRTH,
		CASE
			WHEN PT.DATEOFBIRTH IS NOT NULL AND PT.DateOfBirth != '' THEN 
				CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT)
		END  AS [AGE],
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID
		,(SELECT  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_PATIENT AS P,
			RPT_USER AS U,
			RPT_CAREMEMBER AS C 	 		 	  
		  WHERE
			P.MONGOID = C.MONGOPATIENTID
			AND C.MONGOUSERID = U.MONGOID
			AND P.PATIENTID = PT.PATIENTID 	) AS [ASSIGNED_PCM]
		, pp.[State]
		, pp.[StartDate]
		, pp.[EndDate]
		, pp.[AssignedOn]
	FROM 
		RPT_PatientNote pn 
		INNER JOIN RPT_PATIENT PT ON pn.PatientId = pt.PatientId
		INNER JOIN RPT_PATIENTSYSTEM PS ON PT.DISPLAYPATIENTSYSTEMID = PS.PATIENTSYSTEMID
		INNER JOIN RPT_PATIENTPROGRAM PP ON PT.PatientId = pt.PatientId
		INNER JOIN RPT_NoteMethodLookUp nm ON pn.MethodId = nm.NoteMethodId
		INNER JOIN RPT_NoteWhoLookUp nw ON pn.WhoId = nw.NoteWhoId
		INNER JOIN RPT_NoteSourceLookUp ns ON pn.SourceId = ns.NoteSourceId
		INNER JOIN RPT_NoteOutcomeLookUp noc ON pn.OutcomeId = noc.NoteOutcomeId
		INNER JOIN RPT_NoteDurationLookUp nd ON pn.DurationId = nd.NoteDurationId
		INNER JOIN RPT_User u ON pn.RecordCreatedById = u.UserId
	WHERE
		pn.[Type] = '54909997d43323251c0a1dfe'		
	
END
GO