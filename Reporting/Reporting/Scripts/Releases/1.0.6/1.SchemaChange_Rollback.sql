--ENG1397
Alter Table [dbo].[RPT_PatientObservation]
Add Type varchar(50)

Alter Table [dbo].[RPT_Patient_ClinicalData]
Add Type varchar(50)


--ENG1675

alter table dbo.RPT_Patient
alter column LSSN int

alter table dbo.RPT_PatientInformation
alter column LSSN int

alter table dbo.RPT_Engage_Enrollment_Info
alter column LSSN int

alter table dbo.RPT_CareBridge_Enrollment_Info
alter column LSSN int

alter table dbo.RPT_BSHSI_HW2_Enrollment_Info
alter column LSSN int

