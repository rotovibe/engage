/*** sproc automation ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_SprocNames')
	DROP TABLE RPT_SprocNames;
GO


/***** spPhy_RPT_Execute_Sproc ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Execute_Sproc')
	DROP PROCEDURE [spPhy_RPT_Execute_Sproc];
GO

/***** [spPhy_RPT_Load_Controller] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Load_Controller')
	DROP PROCEDURE [dbo].[spPhy_RPT_Load_Controller];
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/15/2015 13:34:55 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Initialize_Flat_Tables')
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	DECLARE @StartTime Datetime;
	
	SET @StartTime = GETDATE();
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_ProgramResponse_Flat', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));

	SET @StartTime = GETDATE();
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract],[Time]) VALUES ('spPhy_RPT_Flat_BSHSI_HW2', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));

	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_CareBridge', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_Engage];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_Engage', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));	
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_Observations_Dim];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_Observations_Dim', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));		

	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_TouchPoint_Dim];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_TouchPoint_Dim', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));	
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_SavePatientInfo];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_SavePatientInfo', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));		
END
