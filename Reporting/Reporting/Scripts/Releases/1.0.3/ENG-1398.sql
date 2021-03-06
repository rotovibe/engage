IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Program_Enrollment_Beacon')
	DROP TABLE [dbo].[RPT_Flat_Program_Enrollment_Beacon]
GO

CREATE TABLE [dbo].[RPT_Flat_Program_Enrollment_Beacon](
[PatientId] [varchar](50) NULL
,[PatientProgramId] [varchar](50) NULL
,[ProgramName] [varchar](200) NULL
,[Practice] [varchar](50) NULL	
,[PCP] [varchar](50) NULL
,[Program_Completed_Date] [date] NULL
,[Re_enrollment_Date] [date] NULL
,[Enrolled_Date] [date] NULL
,[Enrollment_Action_Completion_Date] [date] NULL
,[Disenroll_Date] [date] NULL
,[Disenroll_Reason] [varchar](max) NULL
,[Other_Disenroll_Reason] [varchar](max) NULL
,[did_not_enroll_date] [date] NULL
,[Did_Not_Enroll_Reason] [varchar](max) NULL
,[Did_Not_Enroll_Reason_Other] [varchar](max) NULL
,[Did_not_enroll_exclusion_criteria] [varchar](max) NULL
,[Engage_Program_Transferred] [varchar](50) NULL
,[Disenrolled_exclusion_criteria] [varchar](50) NULL
,[Completion_By_Death] [varchar](50) NULL
,[Consistancy_With_Advance_directives] [varchar](50) NULL
,[Hospice_Care_Till_Death] [varchar](50) NULL
,[Wish_To_Die_At_Home_Honored] [varchar](50) NULL
,[Referral_Date] [date] NULL
,[Referral_Source] [varchar](max) NULL
,[Reason_For_Referral] [varchar](max) NULL
,[Other_Referral_Source] [varchar](max) NULL
) ON [PRIMARY]
GO

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_Program_Enrollment_Beacon';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Program_Enrollment_Beacon', 'false');
GO