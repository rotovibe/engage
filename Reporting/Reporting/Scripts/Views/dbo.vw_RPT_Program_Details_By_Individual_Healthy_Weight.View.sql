SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Program_Details_By_Individual_Healthy_Weight]
AS
	SELECT 
		pt.PatientId
		, pt.firstName 
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		, (SELECT TOP 1 ps.SystemId FROM RPT_PatientSystem ps with (nolock) WHERE ps.PatientId = pt.PatientId) as SystemId
		, (SELECT TOP 1 ps.SystemName FROM RPT_PatientSystem ps with (nolock) WHERE ps.PatientId = pt.PatientId) as SystemName
		, (SELECT TOP 1 tz.Name FROM RPTMongoTimeZoneLookUp AS tz with (nolock) WHERE tz.TimeZoneId = c.TimeZone) AS [TimeZone]
		, (SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp with (nolock) WHERE cp.ContactId = c.contactid AND cp.OptOut != 'True') as [Phone_1]
		, (SELECT TOP 1
				CASE WHEN (SELECT COUNT(*) from RPT_ContactPhone cp with (nolock) where cp.ContactId = c.ContactId) > 1
				THEN
					 t.Number
				ELSE
					NULL
				END 		 
			FROM ( SELECT TOP 2 d.PhoneId, d.Number, d.contactId FROM (SELECT cp.PhoneId, cp.Number, cp.ContactId, cp.PhonePreferred from RPT_ContactPhone cp with (nolock) where cp.ContactId = c.ContactId AND cp.OptOut != 'True') as d ORDER BY d.PhoneId DESC) as t) as [Phone_2]
		, (SELECT TOP 1 em.Text  FROM vw_RPT_Current_Email em WHERE em.ContactId = c.ContactId ORDER BY em.Preferred DESC) AS [Email_1]	
		, (SELECT TOP 1 em.preferred  FROM vw_RPT_Current_Email em WHERE em.ContactId = c.ContactId ORDER BY em.Preferred DESC) AS [Email_1_Preferred]		
		, (SELECT TOP 1 em.Type  FROM vw_RPT_Current_Email em WHERE em.ContactId = c.ContactId ORDER BY em.Preferred DESC) AS [Email_1_Type]
		, (SELECT TOP 1 a1.Line1  FROM dbo.vw_RPT_Contact_Address a1 WHERE a1.ContactId = c.ContactId) AS [Address_1]
		, (SELECT TOP 1 a2.Line2  FROM dbo.vw_RPT_Contact_Address a2 WHERE a2.ContactId = c.ContactId) AS [Address_2]
		, (SELECT TOP 1 a3.Line3  FROM dbo.vw_RPT_Contact_Address a3 WHERE a3.ContactId = c.ContactId) AS [Address_3]
		, (SELECT TOP 1 ac.city  FROM dbo.vw_RPT_Contact_Address ac WHERE ac.ContactId = c.ContactId) AS [Address_City]
		, (SELECT TOP 1 ast.state  FROM dbo.vw_RPT_Contact_Address ast WHERE ast.ContactId = c.ContactId) AS [Address_State]
		, (SELECT TOP 1 az.ZIP  FROM dbo.vw_RPT_Contact_Address az WHERE az.ContactId = c.ContactId) AS [Address_ZIP_Code]
		, (SELECT TOP 1 cm.pref_name  FROM dbo.vw_RPT_Patient_CM_Info cm WHERE cm.patientid = pt.PatientId) AS [Assigned_PCM]
		, (SELECT TOP 1 cm.fullname  FROM dbo.vw_RPT_Program_CM cm WHERE cm.patientid = pt.PatientId) AS [Program_CM]		
		, ( SELECT TOP 1 [TEXT] FROM [vw_RPT_DoYouHaveaPCP] WHERE PatientId = pt.PatientId AND Selected = 'True' AND Program = 'BSHSI - Healthy Weight') as [Do_you_currently_have_a_PCP]
		, (SELECT TOP 1 [Value] FROM [vw_RPT_ProviderName] where PatientId = pt.PatientId AND Value != '' AND Program = 'BSHSI - Healthy Weight' ) as [Provider_Name]
		, pt.[Priority]
		, pt.LSSN
		, (SELECT TOP 1 [Value] FROM vw_RPT_Pre_Survey_Response WHERE PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC) as [Pre_Survey_Date_Administered]
		, (SELECT TOP 1 [Value] FROM vw_RPT_Post_Survey_Response where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Post_Survey_Date_Administered]
		, (SELECT TOP 1 [Value] FROM vw_RPT_Pending_Enrollment where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Pending_Enrollment_Referral_Date]
		, (SELECT TOP 1 [Value] FROM vw_RPT_Enrolled_Date where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Enrolled_Date]	
		, (SELECT TOP 1 [TEXT]  FROM vw_RPT_Market where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND Selected = 'True' ORDER BY RecordCreatedOn DESC) as [Market]
		, (SELECT TOP 1 [Value] FROM vw_RPT_DidNotEnrollDate where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Date_did_not_enroll]	
		, (SELECT TOP 1 [TEXT]  FROM vw_RPT_DidNotEnrollReason where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND Selected = 'True' ORDER BY RecordCreatedOn DESC) as [Date_did_not_enroll_Reason]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_Enroll_General_Comments where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Value] != ''  ORDER BY RecordCreatedOn DESC ) as [Enrollment_General_Comments]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_DisEnrollDate where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Disenrolled_Date]	
		, (SELECT TOP 1 [TEXT]  FROM vw_RPT_DisenrolledReason where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND Selected = 'True' ORDER BY RecordCreatedOn DESC) as [Disenrolled_Reason]		
		, (SELECT TOP 1 [Value] FROM vw_RPT_Disenrolled_General_Comments where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight'  AND [Value] != '' ORDER BY RecordCreatedOn DESC ) as [Disenrolled_General_Comments]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_ReEnrollment_Date where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Re_enrollment_Date]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_ReEnrolled_Reason where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight'  AND [Value] != ''  ORDER BY RecordCreatedOn DESC) as [Re_enrollment_Reason]		
		, (SELECT TOP 1 [Value] FROM vw_RPT_Program_Completed_Date where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Program_Completed_Date]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_Program_Completed_General_Comments where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Value] != ''  ORDER BY RecordCreatedOn DESC ) as [Program_Completed_General_Comments]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_Risk_Level where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Value] != ''  ORDER BY RecordCreatedOn DESC ) as [Risk_Level]	
		, (SELECT TOP 1 [TEXT]  FROM vw_RPT_Acuity_Level where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND Selected = 'True' ORDER BY RecordCreatedOn DESC) as [Acuity_Level]				
		, (SELECT TOP 1 [Value] FROM vw_RPT_PHQ2_Total_Point_Score where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Value] != ''  ORDER BY RecordCreatedOn DESC ) as [PHQ2_Total_Point_Score]	
		, (SELECT TOP 1 [Value] FROM vw_RPT_Referral_Provided_EAP where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' ORDER BY RecordCreatedOn DESC ) as [Referral_Provided_Depression_EAP]		
		, (SELECT TOP 1 [Value] FROM vw_RPT_Referral_Provided_Community_Resources where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Text] Like '%Community Resources%' ORDER BY RecordCreatedOn DESC ) as [Referral_Provided_Depression_Community_Resources]		
		, (SELECT TOP 1 [Value] FROM vw_RPT_Referral_Provided_Part_Declined where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [TEXT] Like '%Participant Declined%' ORDER BY RecordCreatedOn DESC ) as [Referral_Provided_Depression_Participant_Declined]		
		, (SELECT TOP 1 [Value] FROM vw_RPT_Referral_Other where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Value] != ''  ORDER BY RecordCreatedOn DESC ) as [Other_Referral_Information_Depression]		
		, (SELECT TOP 1 [Value] FROM vw_RPT_PHQ2_Depression_Comments where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight' AND [Value] != ''  ORDER BY RecordCreatedOn DESC ) as [Depression_Screening_General_Comments]		
	FROM 
		RPT_Patient pt with (nolock) 
		INNER JOIN RPT_Contact c with (nolock) ON c.PatientId = pt.PatientId
	WHERE 
		pt.[Delete] = 'False'
GO
