IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Program_Enrollment_Beacon')
	DROP TABLE [dbo].[RPT_Flat_Program_Enrollment_Beacon]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Program_Enrollment_Beacon]
GO

IF EXISTS (SELECT * FROM   sys.objects WHERE  object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetPractice]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
  DROP FUNCTION [dbo].[fn_RPT_GetPractice]
GO 

IF EXISTS (SELECT * FROM   sys.objects WHERE  object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetPCP_Beacon]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
  DROP FUNCTION [dbo].[fn_RPT_GetPCP_Beacon]
GO 


DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_Program_Enrollment_Beacon';
GO
