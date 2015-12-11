IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveMaritalStatusLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveMaritalStatusLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveStatusReasonLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveStatusReasonLookUp]
GO

