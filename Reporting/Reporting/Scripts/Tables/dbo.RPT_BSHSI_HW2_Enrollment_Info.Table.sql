SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_BSHSI_HW2_Enrollment_Info](
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
