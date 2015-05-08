DROP PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	EXECUTE [spPhy_RPT_Flat_Engage];
END
GO
