IF EXISTS (SELECT * FROM sys.columns WHERE Name IN (N'StartTime', N'Duration') AND object_id = Object_ID(N'[dbo].RPT_ToDo'))
	BEGIN
		ALTER TABLE RPT_ToDo
		DROP COLUMN [StartTime], COLUMN [Duration]
	END
GO


IF EXISTS (SELECT * FROM sys.columns WHERE Name IN (N'StartTime', N'Duration') AND object_id = Object_ID(N'[dbo].RPT_Dim_ToDo'))
	BEGIN
		ALTER TABLE RPT_Dim_ToDo
		DROP COLUMN [StartTime], COLUMN [Duration]
	END
GO