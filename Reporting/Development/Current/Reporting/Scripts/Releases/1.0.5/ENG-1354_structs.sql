IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Transition_Info')
	DROP TABLE [dbo].[RPT_Flat_Transition_Info]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RPT_Flat_Transition_Info](
		PatientId [int] NULL,
		MongoPatientId [varchar](50) NULL,
		PatientProgramId [int] NULL,
		MongoPatientProgramId [varchar](50) NULL,
		Enrollment_Status [varchar](100) NULL,
		Enrolled_Date [date] NULL,
		Did_Not_Enroll_Date [date] NULL,
		Did_Not_Enroll_Reason [varchar](1000) NULL,
		Disenroll_Date [date] NULL,
		Disenroll_Reason [varchar](1000) NULL,
		Program_Completed_Date [date] NULL,
		Call_Within_48Hours_PostDischarge [varchar](100) NULL,
		Discharge_Type [varchar](100) NULL,
		Total_LACE_Score [varchar](100) NULL,
		High_Risk_For_Readmission [varchar](100) NULL,
		Program_Status [varchar](100) NULL,
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO


DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_Transition';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Transition', 'false');
GO

