CREATE VIEW [dbo].[vw_aspnet_Profiles]
  AS SELECT [dbo].[Profile].[UserId], [dbo].[Profile].[LastUpdatedDate],
      [DataSize]=  DATALENGTH([dbo].[Profile].[PropertyNames])
                 + DATALENGTH([dbo].[Profile].[PropertyValuesString])
                 + DATALENGTH([dbo].[Profile].[PropertyValuesBinary])
  FROM [dbo].[Profile]
