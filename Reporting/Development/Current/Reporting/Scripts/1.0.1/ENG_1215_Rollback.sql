/**** RPT_CareBridge_Enrollment_Info ****/
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician' AND OBJECT_ID = OBJECT_ID(N'RPT_CareBridge_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_CareBridge_Enrollment_Info
	ADD 
		[Primary_Physician] VARCHAR(1000) NULL;
END
GO

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician_Practice' AND OBJECT_ID = OBJECT_ID(N'RPT_CareBridge_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_CareBridge_Enrollment_Info 
	ADD
		[Primary_Physician_Practice] VARCHAR(2000) NULL;	
END	
GO

/**** RPT_Engage_Enrollment_Info ****/
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician' AND OBJECT_ID = OBJECT_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_Engage_Enrollment_Info 
	ADD 
		[Primary_Physician] VARCHAR(1000) NULL;
END
GO

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician_Practice' AND OBJECT_ID = OBJECT_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_Engage_Enrollment_Info 
	ADD 
		[Primary_Physician_Practice] VARCHAR(2000) NULL;	
END
GO