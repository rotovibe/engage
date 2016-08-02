IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Observations_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Observations_Dim]
GO

CREATE TABLE [dbo].[RPT_Observations_Dim](
	[DimId] [int] IDENTITY(1,1) NOT NULL,
	[ObservationId] [int] NOT NULL,
	[CodingSystem] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[Type] [varchar](100) NULL,
	[Common_Name] [varchar](500) NULL,
	[description] [varchar](500) NULL,
	[HighValue] decimal(18,4) NULL,
	[LowValue] decimal(18,4) NULL,
	[Order] INT NULL,
	[Standard] [varchar](50) NULL,
	[Source] [varchar](100) NULL,
	[status] [varchar](100) NULL,	
	[Unit] [varchar](100) NULL
)
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_Engage]    Script Date: 05/15/2015 11:44:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Observations_Dim]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Observations_Dim]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_Observations_Dim]    Script Date: 05/15/2015 11:44:30 ******/
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Observations_Dim]
AS
BEGIN
	DELETE [RPT_Observations_Dim]
	
	INSERT INTO [RPT_Observations_Dim]
	(
	[ObservationId],
	[CodingSystem],
	[Code],
	[Type],
	[Common_Name],
	[description],
	[HighValue],
	[LowValue],
	[Order],
	[Standard],
	[Source],
	[status],	
	[Unit]		
	)
	select 
		o.ObservationId,
		cs.Name as [CodingSystem],
		o.Code,	
		ot.Name as [Type],
		CASE
			WHEN o.CommonName != '' THEN o.CommonName
			ELSE NULL
		END AS [Common_Name],	
		o.[description],
		o.HighValue,
		o.LowValue,
		o.[Order],
		o.[Standard],
			CASE
			WHEN o.[Source] != '' THEN o.[Source]
			ELSE NULL
		END AS [Source],
		o.[status],
		CASE
			WHEN o.Units != '' THEN o.Units
			ELSE NULL
		END AS [Unit]
	from  
		RPT_Observation o
		INNER JOIN RPT_ObservationTypeLookUp ot ON o.ObservationTypeId = ot.ObservationTypeId
		INNER JOIN RPT_CodingSystemLookUp cs ON o.CodingSystemId = cs.MongoId
END
GO