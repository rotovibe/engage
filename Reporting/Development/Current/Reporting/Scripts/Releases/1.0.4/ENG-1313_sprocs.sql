IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatient]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatient]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatient]
	@MongoID	varchar(50),
	@FirstName	varchar(100),
	@MiddleName varchar(100),
	@LastName	varchar(100),
	@PreferredName varchar(100),
	@Suffix varchar(50),
	@DateOfBirth varchar(50),
	@Gender varchar(50),
	@Priority varchar(50),
	@Version float,
	@UpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Delete varchar(50),
	@BackGround varchar(MAX),
	@FSSN varchar(100),
	@LSSN varchar(50),
	@DisplayPatientSystemMongoId varchar(50),
	@TTLDate datetime,
	@ExtraElements varchar(MAX),
	@ClinicalBackGround varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @DisplayPatientSystemId INT,
			@UpdatedById INT,
			@RecordCreatedById INT
	
	Select @DisplayPatientSystemId = PatientSystemId From RPT_PatientSystem Where MongoId = @DisplayPatientSystemMongoId
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If Exists(Select Top 1 1 From RPT_Patient Where MongoId = @MongoID)
	Begin
		Update RPT_Patient
		Set FirstName = @FirstName,
			MiddleName = @MiddleName,
			LastName = @LastName,
			PreferredName = @PreferredName,
			Suffix = @Suffix,
			DateOfBirth = @DateOfBirth,
			Gender = @Gender,
			Priority = @Priority,
			Version = @Version,
			MongoUpdatedBy = @UpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Delete] = @Delete,
			Background = @BackGround,
			DisplayPatientSystemId = @DisplayPatientSystemId,
			MongoPatientSystemId = @DisplayPatientSystemMongoId,
			UpdatedById = @UpdatedById,
			RecordCreatedById = @RecordCreatedById,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements,
			LSSN = @LSSN,
			FSSN = @FSSN,
			ClinicalBackGround = @ClinicalBackGround
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_Patient
			(ExtraElements, 
			FirstName, 
			MiddleName, 
			LastName, 
			PreferredName, 
			Suffix, 
			DateOfBirth, 
			Gender, 
			[Priority], 
			[Version], 
			MongoUpdatedBy, 
			UpdatedById, 
			LastUpdatedOn, 
			MongoRecordCreatedBy, 
			RecordCreatedById, 
			RecordCreatedOn, 
			[Delete], 
			Background, 
			TTLDate, 
			MongoId,
			LSSN,
			FSSN,
			ClinicalBackGround)
		values 
			(@ExtraElements, 
			@FirstName, 
			@MiddleName, 
			@LastName, 
			@PreferredName, 
			@Suffix, 
			@DateOfBirth, 
			@Gender, 
			@Priority, 
			@Version, 
			@UpdatedBy, 
			@UpdatedById, 
			@LastUpdatedOn, 
			@RecordCreatedBy, 
			@RecordCreatedById, 
			@RecordCreatedOn, 
			@Delete, 
			@BackGround, 
			@TTLDate, 
			@MongoID,
			@LSSN,
			@FSSN,
			@ClinicalBackGround)
	End
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_PatientInformation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_PatientInformation]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_PatientInformation]
AS
	DELETE [RPT_PatientInformation]
	INSERT INTO [RPT_PatientInformation]
	(
		 [PatientId]
		, [MongoId]
		, [firstName] 
		, [LastName]
		, [MiddleName]
		, [Suffix]
		, [DateOfBirth]
		, [AGE]
		, [Gender]	
		, [Priority]	
		, [Background]
		,[ClinicalBackGround]		
		,[SystemId]
		,[SystemName]
		,[TimeZone]
		,[Phone_1]
		,[Phone_2]
		,[Email_1]
		,[Email_1_Preferred]	
		,[Email_1_Type]
		,[Address_1]
		,[Address_2]
		,[Address_3]
		,[Address_City]
		,[Address_State]
		,[Address_ZIP_Code]
		,[Assigned_PCM]
		,[LSSN]
		,[PrimaryId]
		,[PrimaryIdSystem]
		,[EngageId]
	)
	SELECT 
		pt.PatientId
		, pt.MongoId
		, pt.firstName 
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.DateOfBirth
		, (CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END) AS [AGE]
		, pt.Gender	
		, pt.[Priority]			
		, pt.Background
		, pt.ClinicalBackGround
		, (SELECT TOP 1 ps.SystemId FROM RPT_PatientSystem ps with (nolock) WHERE ps.MongoPatientId = pt.MongoId) as SystemId
		, (SELECT TOP 1 ps.SystemName FROM RPT_PatientSystem ps with (nolock) WHERE ps.MongoPatientId = pt.MongoId) as SystemName
		, (SELECT TOP 1 tz.Name FROM RPTMongoTimeZoneLookUp AS tz with (nolock) WHERE tz.MongoId = c.MongoTimeZone) AS [TimeZone]
		, (SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp with (nolock) WHERE cp.MongoContactId = c.MongoId AND cp.OptOut != 'True' AND cp.[Delete] = 'false') as [Phone_1]
		, (SELECT TOP 1
				CASE WHEN (SELECT COUNT(*) from RPT_ContactPhone cp with (nolock) where cp.MongoContactId = c.MongoId) > 1
				THEN
					 t.Number
				ELSE
					NULL
				END 		 
			FROM ( SELECT TOP 2 d.PhoneId, d.Number, d.contactId FROM (SELECT cp.PhoneId, cp.Number, cp.ContactId, cp.PhonePreferred from RPT_ContactPhone cp with (nolock) where cp.MongoContactId = c.MongoId AND cp.OptOut != 'True' AND cp.[Delete] = 'false') as d ORDER BY d.PhoneId DESC) as t) as [Phone_2]
		,(SELECT  TOP 1   ce.[Text] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId)) as [Email_1]
		,(SELECT  TOP 1   ce.[Preferred] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId) ORDER BY ce.Preferred DESC ) as [Email_1_Preferred]	
		,(SELECT  TOP 1 (SELECT TOP 1 Name FROM dbo.RPT_CommTypeLookUp AS t WITH (nolock) WHERE (t.MongoId = ce.MongoCommTypeId)) AS [Type] FROM dbo.RPT_ContactEmail AS ce WITH (nolock) WHERE (ce.[Delete] = 'False') AND (ce.OptOut = 'False') AND (ce.MongoContactId = c.MongoId) ORDER BY ce.Preferred DESC ) as [Email_1_Type]
		,(SELECT TOP 1 Line1 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_1]
		,(SELECT TOP 1 Line2 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_2]
		,(SELECT TOP 1 Line3 FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_3]
		,(SELECT TOP 1 City FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_City]
		,(SELECT TOP 1 (SELECT Code FROM dbo.RPT_StateLookUp AS st WITH (nolock) WHERE (st.MongoId = ca.MongoStateId)) AS [State] FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_State]
		,(SELECT TOP 1 PostalCode FROM dbo.RPT_ContactAddress AS ca WITH (nolock) WHERE ([Delete] = 'False') AND (OptOut = 'False') AND (ca.MongoContactId = c.MongoId) ORDER BY Preferred DESC) AS [Address_ZIP_Code]
		,(SELECT     
			  (SELECT (SELECT u.PreferredName
						FROM dbo.RPT_CareMember AS cm WITH (nolock) 
							INNER JOIN dbo.RPT_User AS u ON cm.MongoUserId = u.MongoId
						WHERE     (cm.CareMemberId = c.CareMemberId)) AS [preferred name]
				FROM dbo.RPT_CareMember AS c WITH (nolock)
				WHERE (c.MongoPatientId = ptn.MongoId)) AS [pref_name]
		  FROM dbo.RPT_Patient AS [ptn] WITH (nolock)
		  WHERE (ptn.[Delete] = 'False') AND ptn.MongoId = pt.MongoId) as [Assigned_PCM]
		, pt.LSSN
		,(select top 1 [value] from rpt_patientsystem with (nolock)  where mongopatientid = pt.MongoId and [primary] = 'True' and [Delete] = 'False' and TTLDate is null ORDER BY RecordCreatedOn DESC) AS [PrimaryId]
		,(select rs.displaylabel from rpt_system rs with (nolock)  where rs.[DeleteFlag] = 'False' and rs.TTLDate is null and rs.MongoId in (select top 1 [SysId] from rpt_patientsystem with (nolock)  where mongopatientid = pt.MongoId and [primary] = 'True' and [Delete] = 'False' ORDER BY RecordCreatedOn DESC)) AS [PrimaryIdSystem]
		,(select [value] from RPT_PatientSystem with (nolock)  where [Delete] = 'False' and TTLDate is null and  MongoRecordCreatedBy = '5368ff2ad4332316288f3e3e' and SysId = '559d8453d433232ca04b3131' and MongoPatientId = pt.MongoId) AS [EngageId]					
	FROM 
		RPT_Patient pt with (nolock) 
		LEFT JOIN RPT_Contact c with (nolock) ON c.MongoPatientId = pt.MongoId
	WHERE 
		pt.[Delete] = 'False'
		AND pt.[TTLDate] IS NULL



GO





