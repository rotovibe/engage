CREATE TABLE [dbo].[RPT_CareBridge_Enrollment_Info] (
    [PatientId]                         INT            NOT NULL,
    [PatientProgramId]                  INT            NOT NULL,
    [Priority]                          VARCHAR (50)   NULL,
    [firstName]                         VARCHAR (100)  NULL,
    [SystemId]                          VARCHAR (50)   NULL,
    [LastName]                          VARCHAR (100)  NULL,
    [MiddleName]                        VARCHAR (100)  NULL,
    [Suffix]                            VARCHAR (50)   NULL,
    [Gender]                            VARCHAR (50)   NULL,
    [DateOfBirth]                       VARCHAR (50)   NULL,
    [LSSN]                              INT            NULL,
    [Assigned_PCM]                      VARCHAR (100)  NULL,
    [Primary_Physician]                 VARCHAR (1000) NULL,
    [Primary_Physician_Practice]        VARCHAR (2000) NULL,
    [Exclusion_Criteria]                VARCHAR (1000) NULL,
    [Program_CM]                        VARCHAR (100)  NULL,
    [Enrollment]                        VARCHAR (50)   NULL,
    [GraduatedFlag]                     VARCHAR (50)   NULL,
    [StartDate]                         DATETIME       NULL,
    [EndDate]                           DATETIME       NULL,
    [Assigned_Date]                     DATETIME       NULL,
    [Last_State_Update_Date]            DATETIME       NULL,
    [State]                             VARCHAR (50)   NULL,
    [Eligibility]                       VARCHAR (50)   NULL,
    [Program_Completed_Date]            DATE           NULL,
    [Re_enrollment_Date]                DATE           NULL,
    [Enrolled_Date]                     DATE           NULL,
    [Pending_Enrolled_Date]             DATE           NULL,
    [Enrollment_Action_Completion_Date] DATE           NULL,
    [Disenroll_Date]                    DATE           NULL,
    [Disenroll_Reason]                  VARCHAR (MAX)  NULL,
    [Disenroll_Reason_Other]            VARCHAR (MAX)  NULL,
    [did_not_enroll_date]               DATE           NULL,
    [did_not_enroll_reason]             VARCHAR (MAX)  NULL,
    [Practice]                          VARCHAR (200)  NULL,
    [PCP]                               VARCHAR (200)  NULL,
    [did_not_enroll_reason_other]       VARCHAR (MAX)  NULL
);

