ALTER TABLE [dbo].[RPT_Flat_MedicationMap_Dim]
    ALTER COLUMN [Form] [varchar](50) NULL  
GO    
  
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSupp]') AND name = N'IX_RPT_PatientMedSupp_Compound')
	DROP INDEX [IX_RPT_PatientMedSupp_Compound] ON [dbo].[RPT_PatientMedSupp] WITH ( ONLINE = OFF )
GO
  
ALTER TABLE dbo.RPT_PatientMedSupp
	ALTER COLUMN [Name] [varchar](200) NULL
GO
			 
ALTER TABLE dbo.RPT_PatientMedSupp
	ALTER COLUMN [Form] [varchar](200) NULL
GO
	
ALTER TABLE dbo.RPT_PatientMedSupp
	ALTER COLUMN [Route] [varchar](200) NULL
GO
	
ALTER TABLE dbo.RPT_PatientMedSupp
	ALTER COLUMN [Strength] [varchar](200) NULL		
GO
	
CREATE NONCLUSTERED INDEX [IX_RPT_PatientMedSupp_Compound] ON [dbo].[RPT_PatientMedSupp] 
(
	[MongoId] ASC,
	[PatientId] ASC,
	[MongoPatientId] ASC,
	[MongoFrequencyId] ASC,
	[MongoFamilyId] ASC,
	[Name] ASC,
	[TypeId] ASC,
	[Route] ASC,
	[Form] ASC,
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO	


ALTER TABLE dbo.RPT_Flat_PatientMedSup_Dim
	ALTER COLUMN [Name] [varchar](200) NULL
GO
				 
ALTER TABLE dbo.RPT_Flat_PatientMedSup_Dim
	ALTER COLUMN [Form] [varchar](300) NULL
GO
	
ALTER TABLE dbo.RPT_Flat_PatientMedSup_Dim
	ALTER COLUMN [Route] [varchar](300) NULL
GO
	
ALTER TABLE dbo.RPT_Flat_PatientMedSup_Dim
	ALTER COLUMN [Strength] [varchar](300) NULL		    
GO

ALTER TABLE dbo.RPT_Flat_PatientMedSup_Dim
	ALTER COLUMN [NDC] [varchar](50) NULL		
GO

ALTER TABLE dbo.RPT_Flat_PatientMedSup_Dim
	ALTER COLUMN [PharmClass] [varchar](200) NULL		
GO