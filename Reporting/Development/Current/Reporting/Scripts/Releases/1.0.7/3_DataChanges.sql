----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_AssignedPCM';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_AssignedPCM', 'false');
GO