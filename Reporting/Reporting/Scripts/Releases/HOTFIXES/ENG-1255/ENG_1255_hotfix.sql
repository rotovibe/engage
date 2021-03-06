/*** sproc automation ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_SprocNames')
	DROP TABLE RPT_SprocNames;
GO

CREATE TABLE RPT_SprocNames
	(
	id int NOT NULL IDENTITY (1, 1),
	SprocName varchar(100) NOT NULL,
	Prerequire bit NOT NULL,
	Description varchar(2000) NULL
	)  ON [PRIMARY]
GO

INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_ProgramResponse_Flat', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_PatientInformation', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_BSHSI_HW2', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_CareBridge', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Engage', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Observations_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_TouchPoint_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_SavePatientInfo', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Program_Details_By_Individual_Healthy_Weight', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_ToDo_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_PatientNotes_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_SavePatientGoalMetrics', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_MedicationMap_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_PatientMedSup_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_SavePatientClinicalData', 'false');
GO

/***** spPhy_RPT_Execute_Sproc ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Execute_Sproc')
	DROP PROCEDURE [spPhy_RPT_Execute_Sproc];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Execute_Sproc]
(	
	@Sproc VARCHAR(2000)
)
AS
BEGIN                    
	DECLARE @StartTime Datetime;
	DECLARE @Sql VARCHAR(2000);
	
	SET @StartTime = GETDATE();
	
	SET @Sql = N'EXECUTE ['+ @Sproc +'];'
	EXEC(@Sql);
	
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES (@Sproc, @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));
END
GO

/***** [spPhy_RPT_Load_Controller] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Load_Controller')
	DROP PROCEDURE [dbo].[spPhy_RPT_Load_Controller];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Load_Controller]
AS
BEGIN                    

	/***** Cursor to run through prereq sproc list  ******/
	DECLARE @Cursor CURSOR;
	DECLARE @SprocName VARCHAR(200);
	BEGIN
		SET @Cursor = CURSOR FOR
		select SprocName from RPT_SprocNames where Prerequire = 'true'      

		OPEN @Cursor 
		FETCH NEXT FROM @Cursor 
		INTO @SprocName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			Print @SprocName
			EXEC [spPhy_RPT_Execute_Sproc] @Sproc = @SprocName;
		  FETCH NEXT FROM @Cursor 
		  INTO @SprocName 
		END; 

		CLOSE @Cursor ;
		DEALLOCATE @Cursor;
	END;	
	
	/***** Cursor to run through sproc list  ******/
	DECLARE @sCursor CURSOR;
	DECLARE @Sproc VARCHAR(200);
	BEGIN
		SET @sCursor = CURSOR FOR
		select SprocName from RPT_SprocNames where Prerequire = 'false'      

		OPEN @sCursor 
		FETCH NEXT FROM @sCursor 
		INTO @Sproc

		WHILE @@FETCH_STATUS = 0
		BEGIN
			Print @Sproc
			EXEC [spPhy_RPT_Execute_Sproc] @Sproc = @Sproc;
			
		  FETCH NEXT FROM @sCursor 
		  INTO @Sproc 
		END; 

		CLOSE @sCursor ;
		DEALLOCATE @sCursor;
	END;	


END
GO


/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/15/2015 13:34:55 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Initialize_Flat_Tables')
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXEC [spPhy_RPT_Load_Controller];
END
GO


/*** [RPT_PatientInformation] ****/
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


/******* [RPT_Program_Details_By_Individual_Healthy_Weight2] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight2')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight2]
GO

CREATE TABLE [RPT_Program_Details_By_Individual_Healthy_Weight2]
(
[PatientId] [int] NOT NULL,
[MongoPatientId] [VARCHAR](50) NOT NULL,
[PatientProgramId] [int] NOT NULL,
[MongoPatientProgramId] [VARCHAR](50) NOT NULL,
[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
[Provider_Name] [varchar](5000) NULL,
[Pre_Survey_Date_Administered] [datetime] NULL,
[Post_Survey_Date_Administered] [datetime] NULL,
[Enrollment_General_Comments] [varchar](5000) NULL,
[Disenrolled_General_Comments] [varchar](5000) NULL,
[Re_enrollment_Reason] [varchar](5000) NULL,
[Program_Completed_General_Comments] [varchar](5000) NULL,
[PHQ2_Total_Point_Score] [varchar](1000) NULL,
[Other_Referral_Information_Depression] [varchar](1000) NULL,
[Depression_Screening_General_Comments] [varchar](5000) NULL,
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight2]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight2]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Enrollment_General_Comments],
		[Disenrolled_General_Comments],
		[Re_enrollment_Reason],
		[Program_Completed_General_Comments],
		[PHQ2_Total_Point_Score],
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '541943a6bdd4dfa5d90002da';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca09ac80d31390000001' ) ) AS [Do_you_currently_have_a_PCP] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f6cb33ac80d3138d00000a' , '53f6ca4aac80d31390000002'  ) ) AS [Provider_Name]  --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4ea64ac80d30e00000021' , '53f4c273ac80d30e00000009' )) AS [Pre_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f4fd75ac80d30e00000083', '53f4f8a0ac80d30e02000073'  ) )	AS [Enrollment_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57115ac80d31203000014', '53f4f8a0ac80d30e02000073') ) AS [Disenrolled_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f5720bac80d3120300001c') ) AS [Re_enrollment_Reason] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f57383ac80d31203000033', '53f4f8a0ac80d30e02000073' ) ) AS [Program_Completed_General_Comments] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '540694b2ac80d330cb000010' ) ) AS [PHQ2_Total_Point_Score] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406956aac80d330c8000014') ) AS [Other_Referral_Information_Depression] --*
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5406b0dcac80d330cb00016d', '5406957eac80d330cb000011') ) AS [Depression_Screening_General_Comments] --*
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO


/******* [RPT_Program_Details_By_Individual_Healthy_Weight] **************/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
GO

CREATE TABLE [RPT_Program_Details_By_Individual_Healthy_Weight]
(
[PatientId] [int] NOT NULL,
[MongoPatientId] [VARCHAR](50) NOT NULL,
[PatientProgramId] [int] NOT NULL,
[MongoPatientProgramId] [VARCHAR](50) NOT NULL,
[Do_you_currently_have_a_PCP] [varchar](5000) NULL,
[Provider_Name] [varchar](5000) NULL,
[Pre_Survey_Date_Administered] [datetime] NULL,
[Post_Survey_Date_Administered] [datetime] NULL,
[Pending_Enrollment_Referral_Date] [datetime] NULL,
[Enrolled_Date] [datetime] NULL,
[Market] [varchar](1000) NULL,
[Date_did_not_enroll] [varchar](1000) NULL,
[Date_did_not_enroll_Reason] [varchar](5000) NULL,
[Enrollment_General_Comments] [varchar](5000) NULL,
[Disenrolled_Date] [datetime] NULL,
[Disenrolled_Reason] [varchar](5000) NULL,
[Disenrolled_General_Comments] [varchar](5000) NULL,
[Re_enrollment_Date] [datetime] NULL,
[Re_enrollment_Reason] [varchar](5000) NULL,
[Program_Completed_Date] [datetime] NULL,
[Program_Completed_General_Comments] [varchar](5000) NULL,
[Risk_Level] [varchar](1000) NULL,
[Acuity_Level] [varchar](1000) NULL,
[PHQ2_Total_Point_Score] [varchar](1000) NULL,
[Referral_Provided_Depression_EAP] [varchar](1000) NULL,
[Referral_Provided_Depression_Community_Resources] [varchar](1000) NULL,
[Referral_Provided_Depression_Participant_Declined] [varchar](1000) NULL,
[Other_Referral_Information_Depression] [varchar](1000) NULL,
[Depression_Screening_General_Comments] [varchar](5000) NULL,
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Program_Details_By_Individual_Healthy_Weight')
	DROP PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight];
GO
CREATE PROCEDURE [spPhy_RPT_Program_Details_By_Individual_Healthy_Weight]
AS
	DECLARE @ProgramSourceId VARCHAR(50);
	SET @ProgramSourceId = '5330920da38116ac180009d2';
	
	DELETE [RPT_Program_Details_By_Individual_Healthy_Weight]
	INSERT INTO [RPT_Program_Details_By_Individual_Healthy_Weight]
	(
		[PatientId],
		[MongoPatientId],
		[PatientProgramId],
		[MongoPatientProgramId],
		[Do_you_currently_have_a_PCP],
		[Provider_Name],
		[Pre_Survey_Date_Administered],
		[Post_Survey_Date_Administered],
		[Pending_Enrollment_Referral_Date],
		[Enrolled_Date],
		[Market],
		[Date_did_not_enroll],
		[Date_did_not_enroll_Reason],
		[Enrollment_General_Comments],
		[Disenrolled_Date],
		[Disenrolled_Reason],
		[Disenrolled_General_Comments],
		[Re_enrollment_Date],
		[Re_enrollment_Reason],
		[Program_Completed_Date],
		[Program_Completed_General_Comments],
		[Risk_Level],
		[Acuity_Level],
		[PHQ2_Total_Point_Score],
		[Referral_Provided_Depression_EAP],
		[Referral_Provided_Depression_Community_Resources] ,
		[Referral_Provided_Depression_Participant_Declined] ,
		[Other_Referral_Information_Depression],
		[Depression_Screening_General_Comments]	
	)
	
	--DECLARE @ProgramSourceId VARCHAR(50);
	--SET @ProgramSourceId = '5330920da38116ac180009d2';	
	SELECT DISTINCT
	pt.[PatientId] AS [PatientId]
	,pt.[MongoId] AS [MongoPatientId]
	,ppt.PatientProgramId
	,ppt.MongoId
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fb32c34786626c000029' ) ) AS [Do_you_currently_have_a_PCP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '52f8c601c34786763500007f' , '52f4fff2c34786626c00002a'  ) ) AS [Provider_Name] 
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531a304bc347860424000109' , '531a2ec7c347860424000023' )) AS [Pre_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5329d20ba3811660150000ea' , '531a2ec7c347860424000023' )) AS [Post_Survey_Date_Administered]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446' , '532c3de1f8efe368860003b2'  )) AS [Pending_Enrollment_Referral_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e1ef8efe368860003b3')) AS [Enrolled_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3e76c347865db8000001')) AS [Market]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '532c3f40c347865db8000002')) AS Date_did_not_enroll
	,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c45bff8efe36886000446', '532c3fc2f8efe368860003b5'))	AS [Date_did_not_enroll_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c45bff8efe36886000446', '530f844ef8efe307660001ab'  ) )	AS [Enrollment_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '532c4061f8efe368860003b6')) AS [Disenrolled_Date]
	,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '532c46b3c347865db8000092', '532c407fc347865db8000003') ) AS [Disenrolled_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c46b3c347865db8000092', '530f844ef8efe307660001ab') ) AS [Disenrolled_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40d0f8efe368860003b7')) AS [Re_enrollment_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c4804f8efe368860004e2', '532c40f3f8efe368860003b8') ) AS [Re_enrollment_Reason]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
		from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '532c411bf8efe368860003b9')) AS [Program_Completed_Date]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c48d7c347865db80000a1', '530f844ef8efe307660001ab' ) ) AS [Program_Completed_General_Comments]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50d60c34786662e000001'  ) ) AS [Risk_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '532c9833a38116ac18000371', '52f50dc3c34786662e000002') ) AS [Acuity_Level]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54de1c34786660a0000de') ) AS [PHQ2_Total_Point_Score]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_EAP]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_Community_Resources]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
		from fn_RPT_GetText(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '531f2db3c347861e77000009') )	AS [Referral_Provided_Depression_Participant_Declined]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '52f54e8cc34786660a0000e0') ) AS [Other_Referral_Information_Depression]
	,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
				from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '531f300ac347861e7700001b', '530f844ef8efe307660001ab') ) AS [Depression_Screening_General_Comments]
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId
		WHERE
			pt.[Delete] = 'False' 	
			AND ppt.SourceId = @ProgramSourceId
			AND ppt.[Delete] = 'False'
GO


/****** [spPhy_RPT_Flat_BSHSI_HW2] *******/
ALTER PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
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
		did_not_enroll_reason,
		Risk_Level,
		Acuity_Frequency
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
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			FROM fn_RPT_GetText_SingleSelect(pt.PatientId,ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fb39ac80d30e00000067') ) as [Market]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56eb7ac80d31203000001')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f57115ac80d31203000014', '53f56f10ac80d31200000001') ) as [Disenroll_Reason]	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4fc71ac80d30e02000074') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fd75ac80d30e00000083', '53f4f885ac80d30e00000065')) as [did_not_enroll_reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f6ce5bac80d3138d000022', '53f6cc01ac80d3139000000d') ) as [Risk_Level]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f6ce5bac80d3138d000022', '53f6cc4cac80d3138d000012') )as [Acuity_Frequency]
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