IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name IN (N'StartTime', N'Duration') AND object_id = Object_ID(N'[dbo].RPT_ToDo'))
	BEGIN
		ALTER TABLE RPT_ToDo
		ADD [StartTime] [time] NULL, [Duration] [int] NULL
	END
GO


IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name IN (N'StartTime', N'Duration') AND object_id = Object_ID(N'[dbo].RPT_Dim_ToDo'))
	BEGIN
		ALTER TABLE RPT_Dim_ToDo
		ADD [StartTime] [time] NULL, [Duration] [int] NULL
		
	END
GO

