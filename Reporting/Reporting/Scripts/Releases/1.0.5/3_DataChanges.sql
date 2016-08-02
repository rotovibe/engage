----------------------------------------------------------------------------------------------------------------------------------
--ENG-1354
----------------------------------------------------------------------------------------------------------------------------------

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_Transition';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Transition', 'false');
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1589
----------------------------------------------------------------------------------------------------------------------------------
DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_GoalStatistics';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_GoalStatistics', 'false');
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1590
----------------------------------------------------------------------------------------------------------------------------------
DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_InterventionStatistics';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_InterventionStatistics', 'false');
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1591
----------------------------------------------------------------------------------------------------------------------------------

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_TaskStatistics';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_TaskStatistics', 'false');
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1592
----------------------------------------------------------------------------------------------------------------------------------

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_BarrierStatistics';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_BarrierStatistics', 'false');
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1506
----------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM [dbo].[RPT_SprocNames] WHERE SprocName = 'spPhy_RPT_Flat_Latest_PatientObservations')
BEGIN
	INSERT INTO [dbo].[RPT_SprocNames]([SprocName],[Prerequire],[Description])
	VALUES ('spPhy_RPT_Flat_Latest_PatientObservations', 0, null)	
END
GO