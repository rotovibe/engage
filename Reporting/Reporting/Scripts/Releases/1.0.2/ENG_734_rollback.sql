/***************** TO DO METRICS *******************/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Dim_ToDo]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Dim_ToDo]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_ToDo_Dim]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
GO

/*** RPT_SprocName ***/
DELETE RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_ToDo_Dim'
GO