ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_TouchPoint_Dim]
	INSERT INTO [RPT_TouchPoint_Dim]
	(
	[PatientNoteId],
	[Method],
	[Who],
	[Source],
	[Outcome],
	[ContactedOn],
	[ProgramName],
	[Duration],
	[ValidatedIntentity],
	[Text],
	[RecordCreatedOn],
	[Record_Created_By],
	[PATIENTID],
	[FIRSTNAME],
	[MIDDLENAME],
	[LASTNAME],
	[DATEOFBIRTH],
	[AGE],
	[GENDER],
	[PRIORITY],
	[SYSTEMID],
	[ASSIGNED_PCM],
	[ASSIGNEDTO],
	[State],
	[StartDate],
	[EndDate],
	[AssignedOn]		
	)
	SELECT
		pn.PatientNoteId,
		(SELECT DISTINCT Name FROM RPT_NoteMethodLookUp nm WHERE nm.MongoId = pn.MongoMethodId) as [Method],
		(SELECT DISTINCT Name FROM RPT_NoteWhoLookUp nw WHERE nw.MongoId = pn.MongoWhoId) as [Who],
		(SELECT DISTINCT Name FROM RPT_NoteSourceLookUp ns WHERE ns.MongoId= pn.MongoSourceId) as [Source],
		(SELECT DISTINCT Name FROM RPT_NoteOutcomeLookUp noc WHERE noc.MongoId = pn.MongoOutcomeId) as [Outcome],
		pn.ContactedOn,
		pp.Name as [ProgramName],
		(SELECT DISTINCT Name FROM RPT_NoteDurationLookUp nd WHERE nd.MongoId = pn.MongoDurationId) as [Duration],
		pn.ValidatedIntentity,
		pn.Text,
		pn.RecordCreatedOn,
		u.PreferredName as [Record_Created_By],
		PT.PATIENTID,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DATEOFBIRTH],
		CASE
			WHEN PT.DATEOFBIRTH IS NOT NULL AND PT.DateOfBirth != '' THEN 
				CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT)
		END  AS [AGE],
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID,
		(SELECT TOP 1	
			U.PREFERREDNAME 	  
		  FROM
			RPT_PATIENT AS P,
			RPT_USER AS U,
			RPT_CAREMEMBER AS C 	 		 	  
		  WHERE
			P.MONGOID = C.MONGOPATIENTID
			AND C.MONGOUSERID = U.MONGOID
			AND P.MongoId = PT.MongoId 	) AS [ASSIGNED_PCM]
		, (SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo]
		, pp.[State]
		, pp.AttributeStartDate as [StartDate]
		, pp.[AttributeEndDate]  as [EndDate]
		, pp.[AssignedOn]
	FROM 
		RPT_PatientNote pn
		left outer join RPT_PatientNoteProgram pnp on pn.MongoId = pnp.MongoPatientNoteId
		INNER JOIN RPT_PATIENT PT ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Type] = '54909997d43323251c0a1dfe'
		and pn.[Delete] = 'False'	
END