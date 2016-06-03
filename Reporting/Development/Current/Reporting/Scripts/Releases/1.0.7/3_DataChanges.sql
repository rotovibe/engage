----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_Assigned_PCM';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Assigned_PCM', 'false');
GO