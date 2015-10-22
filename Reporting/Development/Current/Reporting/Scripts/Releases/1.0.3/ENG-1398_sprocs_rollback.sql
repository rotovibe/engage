IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]
GO

IF  EXISTS (SELECT * FROM [dbo].[RPT_SprocNames] WHERE [SprocName] = 'spPhy_RPT_Flat_Program_Enrollment_Beacon' )
	DELETE FROM [dbo].[RPT_SprocNames] WHERE [SprocName] = 'spPhy_RPT_Flat_Program_Enrollment_Beacon'
GO
