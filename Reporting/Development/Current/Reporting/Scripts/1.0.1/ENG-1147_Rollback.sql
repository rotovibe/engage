-- Drop RPT_Patient_PCM_Program_Info table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient_PCM_Program_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient_PCM_Program_Info]
GO

-- Drop spPhy_RPT_SavePatientInfo stored procedure
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientInfo]
GO