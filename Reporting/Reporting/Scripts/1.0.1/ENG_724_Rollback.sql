/****** Object:  Table [dbo].[RPT_TouchPoint_Dim]    Script Date: 05/15/2015 16:23:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_TouchPoint_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_TouchPoint_Dim]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TouchPoint_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
GO