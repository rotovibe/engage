--1397
Alter Table [dbo].[RPT_PatientObservation]
Drop Column Type

Alter Table [dbo].[RPT_Patient_ClinicalData]
Drop Column Type


--ENG1675
alter table dbo.RPT_Patient
alter column LSSN varchar(4)

alter table dbo.RPT_PatientInformation
alter column LSSN varchar(4)

alter table dbo.RPT_Engage_Enrollment_Info
alter column LSSN varchar(4)

alter table dbo.RPT_CareBridge_Enrollment_Info
alter column LSSN varchar(4)

alter table dbo.RPT_BSHSI_HW2_Enrollment_Info
alter column LSSN varchar(4)

