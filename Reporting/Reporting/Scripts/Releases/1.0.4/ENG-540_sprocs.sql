IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveVisitTypeLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveVisitTypeLookUp]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveVisitTypeLookUp] 
	@MongoId varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_VisitTypeLookUp Where MongoId = @MongoId)
	Begin
		Update RPT_VisitTypeLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default,
			[Active] = @Active
		Where 
			MongoId = @MongoId		
	End
	Else
	Begin
		Insert Into RPT_VisitTypeLookUp(
			LookUpType, 
			Name, 
			[Default], 
			MongoId, 
			Active
		) values (
			@LookUpType, 
			@Name, 
			@Default, 
			@MongoId, 
			@Active
		)
	End
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveUtilizationLocationLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveUtilizationLocationLookUp]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveUtilizationLocationLookUp] 
	@MongoId varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_UtilizationLocationLookUp Where MongoId = @MongoId)
	Begin
		Update RPT_UtilizationLocationLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default,
			[Active] = @Active
		Where 
			MongoId = @MongoId		
	End
	Else
	Begin
		Insert Into RPT_UtilizationLocationLookUp(
			LookUpType, 
			Name, 
			[Default], 
			MongoId, 
			Active
		) values (
			@LookUpType, 
			@Name, 
			@Default, 
			@MongoId, 
			@Active
		)
	End
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveDispositionLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveDispositionLookUp]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveDispositionLookUp] 
	@MongoId varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_DispositionLookUp Where MongoId = @MongoId)
	Begin
		Update RPT_DispositionLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default,
			[Active] = @Active
		Where 
			MongoId = @MongoId		
	End
	Else
	Begin
		Insert Into RPT_DispositionLookUp(
			LookUpType, 
			Name, 
			[Default], 
			MongoId, 
			Active
		) values (
			@LookUpType, 
			@Name, 
			@Default, 
			@MongoId, 
			@Active
		)
	End
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveUtilizationSourceLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveUtilizationSourceLookUp]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveUtilizationSourceLookUp] 
	@MongoId varchar(50),
	@LookUpType varchar(MAX),
	@Name varchar(MAX),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_UtilizationSourceLookUp Where MongoId = @MongoId)
	Begin
		Update RPT_UtilizationSourceLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Default] = @Default,
			[Active] = @Active
		Where 
			MongoId = @MongoId		
	End
	Else
	Begin
		Insert Into RPT_UtilizationSourceLookUp(
			LookUpType, 
			Name, 
			[Default], 
			MongoId, 
			Active
		) values (
			@LookUpType, 
			@Name, 
			@Default, 
			@MongoId, 
			@Active
		)
	End
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_PatientUtilization_Dim]
	INSERT INTO [RPT_PatientUtilization_Dim]
	(
		[PatientUtilizationId],
		[MongoPatientUtilizationId],
		[NoteType],
		[Reason],
		[VisitType],
		[OtherVisitType],
		[AdmitDate],
		[Admitted],
		[DischargeDate],
		[Length],
		[Location],
		[OtherLocation],
		[Disposition],
		[OtherDisposition],	
		[UtilizationSource],
		[DataSource],
		[ProgramName],
		[PatientId],
		[MongoPatientId],
		[FirstName],
		[MiddleName],
		[LastName],
		[DateOfBirth],
		[Age],
		[Gender],
		[Priority],
		[SystemId],
		[Assigned_PCM],
		[AssignedTo],
		[State],
		[StartDate],
		[EndDate],
		[AssignedOn],
		[RecordCreatedOn],
		[RecordCreated_By],
		[RecordUpdatedOn],
		[RecordUpdatedBy]
	)
	SELECT
		pn.PatientUtilizationId,
		pn.MongoId,
		(SELECT DISTINCT Name FROM RPT_NoteTypeLookUp WHERE MongoId = pn.MongoNoteTypeId) as [NoteType],
		pn.Reason,
		(SELECT DISTINCT Name FROM RPT_VisitTypeLookUp nm WHERE nm.MongoId = pn.MongoVisitTypeId) as [VisitType],
		pn.OtherVisitType,
		pn.AdmitDate,
		(CASE WHEN pn.Admitted = 'True' THEN 'Yes' WHEN pn.Admitted = 'False' THEN 'No' END) as Admitted,
		pn.DischargeDate,
		(CASE WHEN (pn.AdmitDate IS NULL AND  pn.DischargeDate IS NULL) THEN NULL
			  WHEN (pn.AdmitDate IS NULL OR  pn.DischargeDate IS NULL) THEN 1
			  WHEN (pn.AdmitDate IS NOT NULL AND pn.DischargeDate IS NOT NULL)  THEN  DATEDIFF(DAY, pn.AdmitDate, pn.DischargeDate) 
		 END) as Length, 
		(SELECT DISTINCT Name FROM RPT_UtilizationLocationLookUp nw WHERE nw.MongoId = pn.MongoLocationId) as [Location],
		pn.OtherLocation,
		(SELECT DISTINCT Name FROM RPT_DispositionLookUp ns WHERE ns.MongoId= pn.MongoDispositionId) as [Disposition],
		pn.OtherDisposition,
		(SELECT DISTINCT Name FROM RPT_UtilizationSourceLookUp noc WHERE noc.MongoId = pn.MongoUtilizationSourceId) as [UtilizationSource],
		pn.DataSource,
		pp.Name as [ProgramName],
		PT.PATIENTID,
		PT.MongoId,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DateOfBirth],
		CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age,	
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
			AND P.MongoId = PT.MongoId 	) AS [Assigned_PCM],
		(SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo],	
		pp.[State],
		pp.AttributeStartDate as [StartDate],
		pp.[AttributeEndDate]  as [EndDate],
		pp.[AssignedOn],
		pn.RecordCreatedOn,
		u.PreferredName,
		pn.LastUpdatedOn,
		(SELECT PreferredName FROM RPT_User WHERE MongoId = pn.MongoUpdatedBy)
	FROM 
		RPT_PatientUtilization pn with (nolock)
		LEFT OUTER JOIN RPT_PatientUtilizationProgram pnp with (nolock) on pn.MongoId = pnp.MongoPatientUtilizationId
		INNER JOIN RPT_PATIENT PT with (nolock) ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS with (nolock) ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP with (nolock) ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u with (nolock) ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Delete] = 'False'	
END
GO



