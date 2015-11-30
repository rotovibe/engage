IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveVisitTypeLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveVisitTypeLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveUtilizationLocationLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveUtilizationLocationLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveDispositionLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveDispositionLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveUtilizationSourceLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveUtilizationSourceLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_PatientUtilization_Dim]
GO

