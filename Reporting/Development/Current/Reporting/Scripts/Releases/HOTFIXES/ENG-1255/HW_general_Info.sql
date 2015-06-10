/*** patient information ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_PatientInformation')
	DROP TABLE [RPT_PatientInformation]
GO

CREATE TABLE [dbo].[RPT_PatientInformation](
		 [PatientId] [int] NOT NULL
		, [MongoId] [varchar](100) NOT NULL
		, [firstName] [varchar](100) NULL
		, [LastName] [varchar](100) NULL
		, [MiddleName] [varchar](100) NULL
		, [Suffix] [varchar](100) NULL
		, [DateOfBirth] [varchar](100) NULL
		, [AGE] [INT] NULL
		, [Gender]	[varchar](100) NULL
		, [Priority] [varchar](100) NULL
		,[SystemId] [varchar](100) NULL
		,[SystemName] [varchar](100) NULL
		,[TimeZone] [varchar](100) NULL
		,[Phone_1] [varchar](100) NULL
		,[Phone_2] [varchar](100) NULL
		,[Email_1] [varchar](100) NULL
		,[Email_1_Preferred] [varchar](100) NULL
		,[Email_1_Type] [varchar](100) NULL
		,[Address_1] [varchar](100) NULL
		,[Address_2] [varchar](100) NULL
		,[Address_3] [varchar](100) NULL
		,[Address_City] [varchar](100) NULL
		,[Address_State] [varchar](100) NULL
		,[Address_ZIP_Code] [varchar](100) NULL
		,[Assigned_PCM] [varchar](100) NULL
		,[LSSN]	[varchar](100) NULL
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_PatientInformation')
	DROP PROCEDURE [spPhy_RPT_PatientInformation];
GO

CREATE PROCEDURE [spPhy_RPT_PatientInformation]
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
	FROM 
		RPT_Patient pt with (nolock) 
		LEFT JOIN RPT_Contact c with (nolock) ON c.MongoPatientId = pt.MongoId
	WHERE 
		pt.[Delete] = 'False'
		AND pt.[TTLDate] IS NULL
GO
