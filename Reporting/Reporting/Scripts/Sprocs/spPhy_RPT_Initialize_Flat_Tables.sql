/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/04/2015 12:02:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	EXECUTE [spPhy_RPT_Flat_Engage];
END
' 
END
GO
