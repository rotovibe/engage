/**** RPT_CareBridge_Enrollment_Info ****/
IF EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician' AND OBJECT_ID = OBJECT_ID(N'RPT_CareBridge_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_CareBridge_Enrollment_Info
	DROP COLUMN 
		[Primary_Physician];
END
GO

IF EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician_Practice' AND OBJECT_ID = OBJECT_ID(N'RPT_CareBridge_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_CareBridge_Enrollment_Info 
	DROP COLUMN 
		[Primary_Physician_Practice];	
END	
GO

/**** RPT_Engage_Enrollment_Info ****/
IF EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician' AND OBJECT_ID = OBJECT_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_Engage_Enrollment_Info 
	DROP COLUMN 
		[Primary_Physician];
END
GO

IF EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'Primary_Physician_Practice' AND OBJECT_ID = OBJECT_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE 
		RPT_Engage_Enrollment_Info 
	DROP COLUMN 
		[Primary_Physician_Practice];	
END	
GO