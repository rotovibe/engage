if exists(select * from sys.columns where Name = N'Age' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info
		ALTER COLUMN  [Age] [int] NULL;
	END
GO


