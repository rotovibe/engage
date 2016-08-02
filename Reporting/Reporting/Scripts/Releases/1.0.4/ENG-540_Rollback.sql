IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_VisitTypeLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_VisitTypeLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_UtilizationLocationLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_UtilizationLocationLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_DispositionLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_DispositionLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_UtilizationSourceLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_UtilizationSourceLookUp]
GO

DELETE [dbo].[RPT_SprocNames]
WHERE SprocName = 'spPhy_RPT_Flat_PatientUtilization_Dim'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientUtilization]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientUtilization]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientUtilizationProgram]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientUtilizationProgram]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientUtilization_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientUtilization_Dim]
GO
