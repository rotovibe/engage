IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
		ALTER TABLE dbo.[RPT_PatientProgramStep] DROP CONSTRAINT FK_PatientProgramModuleActionStep_PatientProgramModuleAction
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionObjective_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModuleActionObjective]'))
	ALTER TABLE dbo.[RPT_PatientProgramModuleActionObjective] DROP CONSTRAINT FK_PatientProgramModuleActionObjective_PatientProgramModuleAction
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
	ALTER TABLE dbo.[RPT_PatientProgramAction] DROP CONSTRAINT FK_PatientProgramModuleAction_PatientProgramModule
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleObjective_PatientProgramModule]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModuleObjective]'))
		ALTER TABLE dbo.[RPT_PatientProgramModuleObjective] DROP CONSTRAINT FK_PatientProgramModuleObjective_PatientProgramModule
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleSpawn_PatientProgramModule]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModuleSpawn]'))
		ALTER TABLE dbo.[RPT_PatientProgramModuleSpawn] DROP CONSTRAINT FK_PatientProgramModuleSpawn_PatientProgramModule
GO 

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModule_PatientProgram]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]'))
		ALTER TABLE dbo.[RPT_PatientProgramModule] DROP CONSTRAINT FK_PatientProgramModule_PatientProgram
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramObjective_PatientProgram]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramObjective]'))		
		ALTER TABLE dbo.[RPT_PatientProgramObjective] DROP CONSTRAINT FK_PatientProgramObjective_PatientProgram
go

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramSpawn_PatientProgram]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramSpawn]'))		
		ALTER TABLE dbo.[RPT_PatientProgramSpawn] DROP CONSTRAINT FK_PatientProgramSpawn_PatientProgram
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgram_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]'))		
		ALTER TABLE dbo.[RPT_PatientProgram] DROP CONSTRAINT FK_PatientProgram_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNote_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNote]'))		
		ALTER TABLE dbo.[RPT_PatientNote] DROP CONSTRAINT FK_PatientNote_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))		
		ALTER TABLE dbo.[RPT_PatientSystem] DROP CONSTRAINT FK_PatientSystem_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProblem_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProblem]'))		
		ALTER TABLE dbo.[RPT_PatientProblem] DROP CONSTRAINT FK_PatientProblem_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientObservation_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientObservation]'))		
		ALTER TABLE dbo.[RPT_PatientObservation] DROP CONSTRAINT FK_PatientObservation_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientUser_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientUser]'))		
		ALTER TABLE dbo.[RPT_PatientUser] DROP CONSTRAINT FK_PatientUser_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CareMember_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_CareMember]'))		
		ALTER TABLE dbo.[RPT_CareMember] DROP CONSTRAINT FK_CareMember_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CohortPatientView_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_CohortPatientView]'))				
		ALTER TABLE dbo.[RPT_CohortPatientView] DROP CONSTRAINT FK_CohortPatientView_Patient
GO		

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Contact]'))						
		ALTER TABLE dbo.[RPT_Contact] DROP CONSTRAINT FK_Contact_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoal_Patient]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoal]'))						
		ALTER TABLE dbo.[RPT_PatientGoal] DROP CONSTRAINT FK_PatientGoal_Patient
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoal_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoal]'))								
		ALTER TABLE dbo.[RPT_PatientGoal] DROP CONSTRAINT FK_PatientGoal_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoal_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoal]'))										
		ALTER TABLE dbo.[RPT_PatientGoal] DROP CONSTRAINT FK_PatientGoal_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalAttribute_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalAttribute]'))	
		ALTER TABLE dbo.[RPT_PatientGoalAttribute] DROP CONSTRAINT FK_PatientGoalAttribute_UserMongoRecordCreatedBy
GO		

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalAttribute_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalAttribute]'))			
		ALTER TABLE dbo.[RPT_PatientGoalAttribute] DROP CONSTRAINT FK_PatientGoalAttribute_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalAttributeValue_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalAttributeValue]'))			
		ALTER TABLE dbo.[RPT_PatientGoalAttributeValue] DROP CONSTRAINT FK_PatientGoalAttributeValue_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalAttributeValue_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalAttributeValue]'))   
ALTER TABLE dbo.[RPT_PatientGoalAttributeValue] DROP CONSTRAINT FK_PatientGoalAttributeValue_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalFocusArea_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalFocusArea]'))   
ALTER TABLE dbo.[RPT_PatientGoalFocusArea] DROP CONSTRAINT FK_PatientGoalFocusArea_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalFocusArea_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalFocusArea]'))   
ALTER TABLE dbo.[RPT_PatientGoalFocusArea] DROP CONSTRAINT FK_PatientGoalFocusArea_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalProgram_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalProgram]'))   
ALTER TABLE dbo.[RPT_PatientGoalProgram] DROP CONSTRAINT FK_PatientGoalProgram_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientGoalProgram_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientGoalProgram]'))   
ALTER TABLE dbo.[RPT_PatientGoalProgram] DROP CONSTRAINT FK_PatientGoalProgram_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientIntervention_User]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientIntervention]'))   
ALTER TABLE dbo.[RPT_PatientIntervention] DROP CONSTRAINT FK_PatientIntervention_User
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientIntervention_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientIntervention]'))   
ALTER TABLE dbo.[RPT_PatientIntervention] DROP CONSTRAINT FK_PatientIntervention_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientIntervention_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientIntervention]'))   
ALTER TABLE dbo.[RPT_PatientIntervention] DROP CONSTRAINT FK_PatientIntervention_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))   
ALTER TABLE dbo.[RPT_Patient] DROP CONSTRAINT FK_Patient_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))   
ALTER TABLE dbo.[RPT_Patient] DROP CONSTRAINT FK_Patient_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientInterventionBarrier_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientInterventionBarrier]'))   
ALTER TABLE dbo.[RPT_PatientInterventionBarrier] DROP CONSTRAINT FK_PatientInterventionBarrier_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientInterventionBarrier_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientInterventionBarrier]'))   
ALTER TABLE dbo.[RPT_PatientInterventionBarrier] DROP CONSTRAINT FK_PatientInterventionBarrier_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNote_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNote]'))   
ALTER TABLE dbo.[RPT_PatientNote] DROP CONSTRAINT FK_PatientNote_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNote_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNote]'))   
ALTER TABLE dbo.[RPT_PatientNote] DROP CONSTRAINT FK_PatientNote_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))   
ALTER TABLE dbo.[RPT_PatientNoteProgram] DROP CONSTRAINT FK_PatientNoteProgram_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))   
ALTER TABLE dbo.[RPT_PatientSystem] DROP CONSTRAINT FK_PatientSystem_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))   
ALTER TABLE dbo.[RPT_PatientNoteProgram] DROP CONSTRAINT FK_PatientNoteProgram_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))   
ALTER TABLE dbo.[RPT_PatientSystem] DROP CONSTRAINT FK_PatientSystem_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProblem_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProblem]'))   
ALTER TABLE dbo.[RPT_PatientProblem] DROP CONSTRAINT FK_PatientProblem_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProblem_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProblem]'))   
ALTER TABLE dbo.[RPT_PatientProblem] DROP CONSTRAINT FK_PatientProblem_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTask_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTask]'))   
ALTER TABLE dbo.[RPT_PatientTask] DROP CONSTRAINT FK_PatientTask_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTask_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTask]'))   
ALTER TABLE dbo.[RPT_PatientTask] DROP CONSTRAINT FK_PatientTask_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTaskAttribute_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTaskAttribute]'))   
ALTER TABLE dbo.[RPT_PatientTaskAttribute] DROP CONSTRAINT FK_PatientTaskAttribute_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientObservation_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientObservation]'))   
ALTER TABLE dbo.[RPT_PatientObservation] DROP CONSTRAINT FK_PatientObservation_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTaskAttribute_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTaskAttribute]'))   
ALTER TABLE dbo.[RPT_PatientTaskAttribute] DROP CONSTRAINT FK_PatientTaskAttribute_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientObservation_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientObservation]'))   
ALTER TABLE dbo.[RPT_PatientObservation] DROP CONSTRAINT FK_PatientObservation_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTaskAttributeValue]'))   
ALTER TABLE dbo.[RPT_PatientTaskAttributeValue] DROP CONSTRAINT FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTaskAttributeValue_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTaskAttributeValue]'))   
ALTER TABLE dbo.[RPT_PatientTaskAttributeValue] DROP CONSTRAINT FK_PatientTaskAttributeValue_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTaskBarrier_UserMongoRecordCreatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTaskBarrier]'))   
ALTER TABLE dbo.[RPT_PatientTaskBarrier] DROP CONSTRAINT FK_PatientTaskBarrier_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientTaskBarrier_UserMongoUpdatedBy]') 
       AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientTaskBarrier]'))   
ALTER TABLE dbo.[RPT_PatientTaskBarrier] DROP CONSTRAINT FK_PatientTaskBarrier_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientUser_Contact]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientUser]'))	
		ALTER TABLE dbo.[RPT_PatientUser] DROP CONSTRAINT FK_PatientUser_Contact
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientUser_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientUser]'))			
		ALTER TABLE dbo.[RPT_PatientUser] DROP CONSTRAINT FK_PatientUser_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientUser_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientUser]'))			
		ALTER TABLE dbo.[RPT_PatientUser] DROP CONSTRAINT FK_PatientUser_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserRecentList_User]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_UserRecentList]'))			
		ALTER TABLE dbo.[RPT_UserRecentList] DROP CONSTRAINT FK_UserRecentList_User
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CareMember_User]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_CareMember]'))
		ALTER TABLE dbo.[RPT_CareMember] DROP CONSTRAINT FK_CareMember_User
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CareMember_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_CareMember]'))		
		ALTER TABLE dbo.[RPT_CareMember] DROP CONSTRAINT FK_CareMember_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CareMember_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_CareMember]'))		
		ALTER TABLE dbo.[RPT_CareMember] DROP CONSTRAINT FK_CareMember_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Contact]'))		
		ALTER TABLE dbo.[RPT_Contact] DROP CONSTRAINT FK_Contact_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Contact]'))		
		ALTER TABLE dbo.[RPT_Contact] DROP CONSTRAINT FK_Contact_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientBarrier_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientBarrier]'))
		ALTER TABLE dbo.[RPT_PatientBarrier] DROP CONSTRAINT FK_PatientBarrier_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactAddress_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactAddress]'))
		ALTER TABLE dbo.[RPT_ContactAddress] DROP CONSTRAINT FK_ContactAddress_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientBarrier_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientBarrier]'))
		ALTER TABLE dbo.[RPT_PatientBarrier] DROP CONSTRAINT FK_PatientBarrier_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactAddress_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactAddress]'))
		ALTER TABLE dbo.[RPT_ContactAddress] DROP CONSTRAINT FK_ContactAddress_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactEmail_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactEmail]'))
		ALTER TABLE dbo.[RPT_ContactEmail] DROP CONSTRAINT FK_ContactEmail_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactEmail_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactEmail]'))
		ALTER TABLE dbo.[RPT_ContactEmail] DROP CONSTRAINT FK_ContactEmail_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactLanguage_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactLanguage]'))
		ALTER TABLE dbo.[RPT_ContactLanguage] DROP CONSTRAINT FK_ContactLanguage_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactLanguage_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactLanguage]'))
		ALTER TABLE dbo.[RPT_ContactLanguage] DROP CONSTRAINT FK_ContactLanguage_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactMode_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactMode]'))
		ALTER TABLE dbo.[RPT_ContactMode] DROP CONSTRAINT FK_ContactMode_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactMode_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactMode]'))
		ALTER TABLE dbo.[RPT_ContactMode] DROP CONSTRAINT FK_ContactMode_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactPhone_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactPhone]'))
		ALTER TABLE dbo.[RPT_ContactPhone] DROP CONSTRAINT FK_ContactPhone_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactPhone_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactPhone]'))
		ALTER TABLE dbo.[RPT_ContactPhone] DROP CONSTRAINT FK_ContactPhone_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactRecentList_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactRecentList]'))
		ALTER TABLE dbo.[RPT_ContactRecentList] DROP CONSTRAINT FK_ContactRecentList_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactRecentList_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactRecentList]'))
		ALTER TABLE dbo.[RPT_ContactRecentList] DROP CONSTRAINT FK_ContactRecentList_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactTimeOfDay_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactTimeOfDay]'))
		ALTER TABLE dbo.[RPT_ContactTimeOfDay] DROP CONSTRAINT FK_ContactTimeOfDay_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactTimeOfDay_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactTimeOfDay]'))
		ALTER TABLE dbo.[RPT_ContactTimeOfDay] DROP CONSTRAINT FK_ContactTimeOfDay_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactWeekDay_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactWeekDay]'))
		ALTER TABLE dbo.[RPT_ContactWeekDay] DROP CONSTRAINT FK_ContactWeekDay_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ContactWeekDay_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_ContactWeekDay]'))
		ALTER TABLE dbo.[RPT_ContactWeekDay] DROP CONSTRAINT FK_ContactWeekDay_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GoalAttribute_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_GoalAttribute]'))
		ALTER TABLE dbo.[RPT_GoalAttribute] DROP CONSTRAINT FK_GoalAttribute_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GoalAttributeOption_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_GoalAttributeOption]'))
		ALTER TABLE dbo.[RPT_GoalAttributeOption] DROP CONSTRAINT FK_GoalAttributeOption_UserMongoUpdatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Observation_UserMongoRecordCreatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Observation]'))
		ALTER TABLE dbo.[RPT_Observation] DROP CONSTRAINT FK_Observation_UserMongoRecordCreatedBy
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Observation_UserMongoUpdatedBy]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Observation]'))
		ALTER TABLE dbo.[RPT_Observation] DROP CONSTRAINT FK_Observation_UserMongoUpdatedBy
GO

/****** Object:  ForeignKey [FK_Patient_UserMongoRecordCreatedBy]    Script Date: 05/04/2015 10:47:47 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
/****** Object:  ForeignKey [FK_Patient_UserMongoUpdatedBy]    Script Date: 05/04/2015 10:47:47 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
/****** Object:  ForeignKey [FK_PatientProgram_Patient]    Script Date: 05/04/2015 10:47:49 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgram_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]'))
ALTER TABLE [dbo].[RPT_PatientProgram] DROP CONSTRAINT [FK_PatientProgram_Patient]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleAction_PatientProgramModule]    Script Date: 05/04/2015 10:47:51 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction] DROP CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
/****** Object:  ForeignKey [FK_PatientProgramModule_PatientProgram]    Script Date: 05/04/2015 10:47:55 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModule_PatientProgram]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]'))
ALTER TABLE [dbo].[RPT_PatientProgramModule] DROP CONSTRAINT [FK_PatientProgramModule_PatientProgram]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]    Script Date: 05/04/2015 10:47:57 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]    Script Date: 05/04/2015 10:47:57 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]    Script Date: 05/04/2015 10:48:00 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
/****** Object:  ForeignKey [FK_PatientSystem_Patient]    Script Date: 05/04/2015 10:48:02 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_Patient]
GO
/****** Object:  ForeignKey [FK_PatientSystem_UserMongoRecordCreatedBy]    Script Date: 05/04/2015 10:48:02 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
/****** Object:  ForeignKey [FK_PatientSystem_UserMongoUpdatedBy]    Script Date: 05/04/2015 10:48:02 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/04/2015 10:48:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_CareBridge]    Script Date: 05/04/2015 10:48:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_CareBridge]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_CareBridge]
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_Engage]    Script Date: 05/04/2015 10:48:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Engage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_CareBridge_GetPCP]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_CareBridge_GetPCP]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_CareBridge_GetPCP]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_GetPCP]    Script Date: 05/04/2015 10:48:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_GetPCP]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Engage_GetPCP]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_PCP_Practice_Val]    Script Date: 05/04/2015 10:48:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Engage_PCP_Practice_Val]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetPractice_Engage]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetPractice_Engage]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetPractice_Engage]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_PCP_Practice_Val]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_PCP_Practice_Val]
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]    Script Date: 05/04/2015 10:48:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_ProgramResponse_Flat]    Script Date: 05/04/2015 10:48:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_ProgramResponse_Flat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_ProgramResponse_Flat]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_PCPOther]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_PCPOther]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_PCPOther]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetRecentActionCompletedDate]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetRecentActionCompletedDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetRecentActionCompletedDate]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetText]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetText]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetText_ZeroVal]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText_ZeroVal]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetText_ZeroVal]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetValue]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetValue]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Enrollment]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Enrollment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Enrollment]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetDate]    Script Date: 05/04/2015 10:48:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetDate]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_DidNotEnrollReason]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DidNotEnrollReason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_DidNotEnrollReason]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_DisEnrollmentReason]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DisEnrollmentReason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_DisEnrollmentReason]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_DisEnrollmentReasonV2]    Script Date: 05/04/2015 10:48:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DisEnrollmentReasonV2]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_DisEnrollmentReasonV2]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramResponse]    Script Date: 05/04/2015 10:47:57 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramResponse]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/04/2015 10:48:00 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramStep]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramAction]    Script Date: 05/04/2015 10:47:51 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction] DROP CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramAction]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramModule]    Script Date: 05/04/2015 10:47:55 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModule_PatientProgram]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]'))
ALTER TABLE [dbo].[RPT_PatientProgramModule] DROP CONSTRAINT [FK_PatientProgramModule_PatientProgram]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramModule]
GO
/****** Object:  Table [dbo].[RPT_PatientProgram]    Script Date: 05/04/2015 10:47:49 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgram_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]'))
ALTER TABLE [dbo].[RPT_PatientProgram] DROP CONSTRAINT [FK_PatientProgram_Patient]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgram]
GO
/****** Object:  Table [dbo].[RPT_PatientSystem]    Script Date: 05/04/2015 10:48:02 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_Patient]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientSystem]
GO
/****** Object:  Table [dbo].[RPT_Patient]    Script Date: 05/04/2015 10:47:47 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionCompleted_Text]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Text]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionCompleted_Value]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Value]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionNotCompleted_Text]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Text]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionNotCompleted_Value]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionSaved_Text]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionSaved_Text]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionSaved_Value]    Script Date: 05/04/2015 10:48:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionSaved_Value]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramAttribute]    Script Date: 05/04/2015 10:47:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramAttribute]
GO
/****** Object:  Table [dbo].[RPT_ProgramResponse_Flat]    Script Date: 05/04/2015 10:48:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_ProgramResponse_Flat]
GO
/****** Object:  Table [dbo].[RPT_User]    Script Date: 05/04/2015 10:48:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_User]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_User]
GO
/****** Object:  Table [dbo].[RPT_User]    Script Date: 05/04/2015 10:48:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[ResourceId] [varchar](50) NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[PreferredName] [varchar](100) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_User]') AND name = N'IX_RPT_User_MongoId')
CREATE NONCLUSTERED INDEX [IX_RPT_User_MongoId] ON [dbo].[RPT_User] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_ProgramResponse_Flat]    Script Date: 05/04/2015 10:48:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_ProgramResponse_Flat](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[ProgramSourceId] [varchar](50) NULL,
	[ActionSourceId] [varchar](50) NULL,
	[ActionArchived] [varchar](50) NULL,
	[ActionArchivedDate] [datetime] NULL,
	[StepSourceId] [varchar](50) NULL,
	[Text] [varchar](max) NULL,
	[Value] [varchar](max) NULL,
	[Selected] [varchar](50) NULL,
	[Delete] [varchar](50) NULL,
	[ActionCompleted] [varchar](50) NULL,
	[StepCompleted] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[ActionName] [varchar](200) NULL,
	[Question] [varchar](max) NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_ActionSourceID')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_ActionSourceID] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[ActionSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_Composite')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_Composite] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[ProgramSourceId] ASC,
	[StepSourceId] ASC,
	[ActionSourceId] ASC,
	[ActionArchived] ASC,
	[ActionCompleted] ASC,
	[PatientId] ASC,
	[PatientProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_PatientID')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_PatientID] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_ProgramSourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_ProgramSourceId] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[ProgramSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = N'IX_RPT_ProgramResponse_Flat_StepSourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_StepSourceId] ON [dbo].[RPT_ProgramResponse_Flat] 
(
	[StepSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramAttribute]    Script Date: 05/04/2015 10:47:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramAttribute](
	[PatientProgramAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NULL,
	[Completed] [varchar](50) NULL,
	[DidNotEnrollReason] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[InelligibleReason] [varchar](50) NULL,
	[Lock] [varchar](50) NULL,
	[OptOut] [varchar](50) NULL,
	[OverrideReason] [varchar](50) NULL,
	[MongoPlanElementId] [varchar](50) NULL,
	[PlanElementId] [int] NULL,
	[Population] [varchar](50) NULL,
	[RemovedReason] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [varchar](50) NULL,
	[Delete] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[CompletedBy] [int] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_PatientProgramAttribute] PRIMARY KEY CLUSTERED 
(
	[PatientProgramAttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND name = N'IX_RPT_PatientProgramAttribute_MongoPlanElementId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAttribute_MongoPlanElementId] ON [dbo].[RPT_PatientProgramAttribute] 
(
	[MongoPlanElementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND name = N'IX_RPT_PatientProgramAttribute_PlanElementId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAttribute_PlanElementId] ON [dbo].[RPT_PatientProgramAttribute] 
(
	[PlanElementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionSaved_Value]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionSaved_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			CASE WHEN p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 THEN
				p.[Value]
			ELSE
				''0''
			END
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''False''
			AND p.[Delete] = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionSaved_Text]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionSaved_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( [Value] VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			(CASE WHEN p.[Selected] = ''True'' THEN
				p.[Text]
			ELSE
				''0''
			END) as [Value]
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''False''
			AND p.[Delete] = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionNotCompleted_Value]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			''0'' as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''False''
			AND p.StepCompleted = ''False''
			--AND p.[Selected] = ''False''
			AND p.[Delete] = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionNotCompleted_Text]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			''0'' as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''False''
			AND p.StepCompleted = ''False''
			AND p.[Selected] = ''False''
			AND p.[Delete] = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionCompleted_Value]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Delete] = ''False'' AND (p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 )THEN
					p.[Value]		
			ELSE 						
				''0''
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionCompleted_Text]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			(CASE WHEN p.[Selected] = ''True'' AND p.[Delete] = ''False'' THEN
					p.[Text]		
			ELSE 						
				NULL --''0''
			END) as [Value]
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
' 
END
GO
/****** Object:  Table [dbo].[RPT_Patient]    Script Date: 05/04/2015 10:47:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_Patient](
	[PatientId] [int] IDENTITY(1,1) NOT NULL,
	[DisplayPatientSystemId] [int] NULL,
	[MongoPatientSystemId] [varchar](50) NULL,
	[MongoId] [varchar](50) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[PreferredName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[Priority] [varchar](50) NULL,
	[Background] [varchar](max) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
	[FSSN] [varchar](100) NULL,
	[LSSN] [int] NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient]') AND name = N'IX_RPT_Patient_MongoId')
CREATE NONCLUSTERED INDEX [IX_RPT_Patient_MongoId] ON [dbo].[RPT_Patient] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientSystem]    Script Date: 05/04/2015 10:48:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientSystem](
	[PatientSystemId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Label] [varchar](50) NULL,
	[SystemId] [varchar](50) NULL,
	[SystemName] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_PatientSystem] PRIMARY KEY CLUSTERED 
(
	[PatientSystemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]') AND name = N'IX_RPT_PatientSystem_PatientId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientSystem_PatientId] ON [dbo].[RPT_PatientSystem] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientProgram]    Script Date: 05/04/2015 10:47:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgram](
	[PatientProgramId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NULL,
	[ShortName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[AttributeStartDate] [datetime] NULL,
	[AttributeEndDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[AssignedOn] [datetime] NULL,
	[MongoAssignedBy] [varchar](50) NULL,
	[AssignedBy] [int] NULL,
	[MongoAssignedToId] [varchar](50) NULL,
	[AssignedToId] [int] NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[EligibilityReason] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[ContractProgramId] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[Enabled] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[CompletedBy] [int] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[EligibilityRequirements] [varchar](max) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_PatientProgram] PRIMARY KEY CLUSTERED 
(
	[PatientProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]') AND name = N'IX_RPT_PatientProgram_MongoId')
CREATE UNIQUE NONCLUSTERED INDEX [IX_RPT_PatientProgram_MongoId] ON [dbo].[RPT_PatientProgram] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]') AND name = N'IX_RPT_PatientProgram_MongoPatientId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgram_MongoPatientId] ON [dbo].[RPT_PatientProgram] 
(
	[MongoPatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]') AND name = N'IX_RPT_PatientProgram_PatientID')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgram_PatientID] ON [dbo].[RPT_PatientProgram] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramModule]    Script Date: 05/04/2015 10:47:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramModule](
	[PatientProgramModuleId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoProgramId] [varchar](50) NOT NULL,
	[AttributeStartDate] [datetime] NULL,
	[AttributeEndDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[AssignedOn] [datetime] NULL,
	[MongoAssignedBy] [varchar](50) NULL,
	[AssignedBy] [int] NULL,
	[MongoAssignedTo] [varchar](50) NULL,
	[AssignedTo] [int] NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[Enabled] [varchar](50) NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[LastupdatedOn] [datetime] NULL,
	[RecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModule] PRIMARY KEY CLUSTERED 
(
	[PatientProgramModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]') AND name = N'IX_RPT_PatientProgramModule_SourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramModule_SourceId] ON [dbo].[RPT_PatientProgramModule] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramAction]    Script Date: 05/04/2015 10:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramAction](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramModuleId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoModuleId] [varchar](50) NOT NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[AssignedOn] [datetime] NULL,
	[MongoAssignedBy] [varchar](50) NULL,
	[AssignedBy] [int] NULL,
	[MongoAssignedTo] [varchar](50) NULL,
	[AssignedTo] [int] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[AttributeEndDate] [datetime] NULL,
	[AttributeStartDate] [datetime] NULL,
	[Enabled] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[Archived] [varchar](50) NULL,
	[ArchivedDate] [datetime] NULL,
	[MongoArchiveOriginId] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleAction] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND name = N'IX_RPT_PatientProgramAction')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAction] ON [dbo].[RPT_PatientProgramAction] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND name = N'IX_RPT_PatientProgramAction_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAction_1] ON [dbo].[RPT_PatientProgramAction] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/04/2015 10:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramStep](
	[StepId] [int] IDENTITY(1,1) NOT NULL,
	[MongoActionId] [varchar](50) NOT NULL,
	[ActionId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[AttributeEndDate] [datetime] NULL,
	[AttributeStartDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Header] [varchar](100) NULL,
	[SelectedResponseId] [varchar](50) NULL,
	[ControlType] [varchar](50) NULL,
	[SelectType] [varchar](50) NULL,
	[IncludeTime] [varchar](50) NULL,
	[Question] [varchar](max) NULL,
	[Title] [varchar](2000) NULL,
	[Description] [varchar](max) NULL,
	[Notes] [varchar](max) NULL,
	[Text] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[Response] [varchar](50) NULL,
	[StepTypeId] [varchar](50) NULL,
	[Enabled] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStep] PRIMARY KEY CLUSTERED 
(
	[StepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep] ON [dbo].[RPT_PatientProgramStep] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_1] ON [dbo].[RPT_PatientProgramStep] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_2')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_2] ON [dbo].[RPT_PatientProgramStep] 
(
	[StepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_ActionId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_ActionId] ON [dbo].[RPT_PatientProgramStep] 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramResponse]    Script Date: 05/04/2015 10:47:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramResponse](
	[ResponseId] [int] IDENTITY(1,1) NOT NULL,
	[MongoStepId] [varchar](50) NULL,
	[StepId] [int] NULL,
	[MongoNextStepId] [varchar](50) NULL,
	[NextStepId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoActionId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Text] [varchar](max) NULL,
	[Value] [varchar](max) NULL,
	[Nominal] [varchar](50) NULL,
	[Required] [varchar](50) NULL,
	[Selected] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime2](7) NULL,
	[Delete] [varchar](50) NULL,
	[MongoStepSourceId] [varchar](50) NULL,
	[StepSourceId] [int] NULL,
	[ActionId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime2](7) NULL,
	[TTLDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStepResponse] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_1] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_2')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_2] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_State')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_State] ON [dbo].[RPT_PatientProgramResponse] 
(
	[Selected] ASC,
	[Delete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_DisEnrollmentReasonV2]    Script Date: 05/04/2015 10:48:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DisEnrollmentReasonV2]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_DisEnrollmentReasonV2] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = ''True'' AND pr7.Selected = ''True'' THEN --pr.[delete] = ''False'' AND pa.[State] = ''Completed''
					CASE WHEN pr7.[Delete] = ''False'' THEN
						pr7.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				ps7.SourceId= ''542561ec890e942ba1000004''
				AND pa7.SourceId = ''545c0805ac80d36bd4000089''
				AND pp7.Name = ''BSHSI - Healthy Weight v2''
				AND (pp7.[Delete] = ''False'')
				AND pr7.Selected = ''True''
				AND pa7.[State] IN (''Completed'')
				AND pa7.Archived = ''True''
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT 
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							ps8.SourceId = ''542561ec890e942ba1000004''
							AND pa8.SourceId = ''545c0805ac80d36bd4000089''
							AND pp8.Name = ''BSHSI - Healthy Weight v2''
							AND (pp8.[Delete] = ''False'')
							AND pa8.[State] IN (''Completed'')
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_DisEnrollmentReason]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DisEnrollmentReason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_DisEnrollmentReason] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = ''True'' AND pr7.Selected = ''True'' THEN --pr.[delete] = ''False'' AND pa.[State] = ''Completed''
					CASE WHEN pr7.[Delete] = ''False'' THEN
						pr7.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				ps7.SourceId= @StepSourceId
				AND pa7.SourceId = @ActionSourceId
				AND pp7.SourceId = @ProgramSourceId
				AND (pp7.[Delete] = ''False'')
				AND pr7.Selected = ''True''
				AND pa7.[State] IN (''Completed'')
				AND pa7.Archived = ''True''
				AND pa7.ArchivedDate in (SELECT DISTINCT TOP 1 PERCENT  ActionArchivedDate FROM RPT_ProgramResponse_Flat WHERE PatientId = @patientid AND PatientProgramId = @patientprogramid and ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)				
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT TOP 1
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							ps8.SourceId = @StepSourceId
							AND pa8.SourceId = @ActionSourceId
							AND pp8.SourceId = @ProgramSourceId
							AND (pp8.[Delete] = ''False'')
							AND pa8.[State] IN (''Completed'')
							AND pa8.ArchivedDate in (SELECT DISTINCT TOP 1 PERCENT  ActionArchivedDate FROM RPT_ProgramResponse_Flat WHERE PatientId = @patientid AND PatientProgramId = @patientprogramid and ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)										
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_DidNotEnrollReason]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DidNotEnrollReason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_DidNotEnrollReason] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @DidNotEnrollTable TABLE( Reason VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = ''True'' AND pr13.Selected = ''True'' THEN --pr.[delete] = ''False'' AND pa.[State] = ''Completed''
					CASE WHEN pr13.[Delete] = ''False'' THEN
						pr13.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = ''False'')
				AND (pa13.Archived = ''True'')
				AND pa13.ArchivedDate in (SELECT DISTINCT TOP 1 PERCENT  ActionArchivedDate FROM RPT_ProgramResponse_Flat WHERE PatientId = @patientid AND PatientProgramId = @patientprogramid and ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)								
				AND (pr13.Selected = ''True'')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN (''Completed'')
				AND pt13.PatientId = @patientid
				AND pp13.patientprogramid = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT TOP 1
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = ''False'')
						--AND (pr14.Selected = ''True'')
						AND pa14.ArchivedDate in (SELECT DISTINCT TOP 1 PERCENT  ActionArchivedDate FROM RPT_ProgramResponse_Flat WHERE PatientId = @patientid AND PatientProgramId = @patientprogramid and ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)																
						AND pt14.PatientId = @patientid
						AND pa14.[State] IN (''Completed'')
						AND pp14.patientprogramid = @patientprogramid
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetDate]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_GetDate] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where Value != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL as [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = ''True'' THEN
					CASE WHEN pr13.[Delete] = ''False'' THEN
						pr13.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = ''False'')
				AND (pa13.Archived = ''True'')
				--AND (pr13.Selected = ''True'')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN (''Completed'')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = ''False'')
						--AND (pr14.Selected = ''True'')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN (''Completed'')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Enrollment]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Enrollment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_Enrollment] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @EnrollmentTable TABLE( Enrollment VARCHAR(2000))
AS
BEGIN
		INSERT INTO @EnrollmentTable
		select 
			ppa.Enrollment
		from 
			RPT_PatientProgram p,
			RPT_PatientProgramAttribute ppa,
			RPT_PatientProgramModule m,
			RPT_PatientProgramAction a,
			RPT_Patientprogramstep s,
			RPT_PatientProgramResponse r
		where 
			p.MongoId = ppa.MongoPlanElementId
			and p.MongoId = m.MongoProgramId
			and m.MongoId = a.MongoModuleId
			and a.mongoId = s.MongoActionId
			and s.MongoId = r.MongoStepId
			and p.PatientProgramId = @patientprogramid
			and p.SourceId = @ProgramSourceId
			and a.SourceId = @ActionSourceId
			and s.SourceId = @StepSourceId
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetValue]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_GetValue] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where Value != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = ''True'' THEN
					CASE WHEN pr13.[Delete] = ''False'' THEN
						pr13.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = ''False'')
				AND (pa13.Archived = ''True'')
				--AND (pr13.Selected = ''True'')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN (''Completed'')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = ''False'')
						--AND (pr14.Selected = ''True'')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN (''Completed'')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetText_ZeroVal]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText_ZeroVal]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_GetText_ZeroVal] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
	--------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 8;
	--SET @patientprogramid = 279;
	--SET @ProgramSourceId = ''54b69910ac80d33c2c000032'';
	--SET @ActionSourceId = ''545bfc3bac80d36bd10000a7'';
	--SET @StepSourceId = ''5453e3bbac80d37bc0000f03'';
	
	----------------------------------------

		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	-------
	--select @CompletedCount;
	--select @SavedCount;
	--select @ActionNotComplete;
	
	--SELECT TOP 1 [Value] FROM 	
	--	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) ORDER BY [Value] DESC
	-------
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId)
			ORDER BY [Value] DESC
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId)
			ORDER BY [Value] DESC
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = ''True'' THEN
					CASE WHEN pr13.[Delete] = ''False'' THEN
						pr13.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = ''False'')
				AND (pa13.Archived = ''True'')
				AND (pr13.Selected = ''True'')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN (''Completed'')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ArchivedDate in (	SELECT DISTINCT TOP 1 PERCENT  
												ActionArchivedDate 
											FROM RPT_ProgramResponse_Flat 
											WHERE 
												PatientId = @patientid AND 
												PatientProgramId = @patientprogramid AND 
												ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)				
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT TOP 1
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = ''False'')
						--AND (pr14.Selected = ''True'')
						AND pa14.ArchivedDate in (	SELECT DISTINCT TOP 1 PERCENT  
														ActionArchivedDate 
													FROM RPT_ProgramResponse_Flat 
													WHERE 
														PatientId = @patientid AND 
														PatientProgramId = @patientprogramid AND 
														ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)			
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN (''Completed'')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetText]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_GetText] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where Value != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = ''True'' THEN
					CASE WHEN pr13.[Delete] = ''False'' THEN
						pr13.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = ''False'')
				AND (pa13.Archived = ''True'')
				AND (pr13.Selected = ''True'')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN (''Completed'')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = ''False'')
						AND (pr14.Selected = ''True'')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN (''Completed'')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetRecentActionCompletedDate]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetRecentActionCompletedDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_GetRecentActionCompletedDate] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_RecentActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_RecentActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) where Value != ''0''
			GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @CompletedCount = 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = ''True'' THEN
					CASE WHEN pr13.[Delete] = ''False'' OR pr13.[Delete] = ''True'' THEN
						CASE WHEN pa13.[DateCompleted] IS NOT NULL AND LEN(pa13.[DateCompleted]) > 0 THEN
								pa13.[DateCompleted]
						ELSE 						
							NULL
						END					
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = ''False'')
				AND (pa13.Archived = ''True'')
				--AND (pr13.Selected = ''True'')
				AND pa13.ArchivedDate IS NOT NULL
				--AND pa13.[State] IN (''Completed'')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = ''False'')
						--AND (pr14.Selected = ''True'')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						--AND pa14.[State] IN (''Completed'')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO


/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetText_SingleSelect]    Script Date: 05/07/2015 15:08:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetText_SingleSelect]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetText_SingleSelect]
GO

CREATE FUNCTION [dbo].[fn_RPT_GetText_SingleSelect] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN

-------
--DECLARE @patientid INT;
--DECLARE @patientprogramid INT;
--DECLARE @ProgramSourceId VARCHAR(50);
--DECLARE @ActionSourceId VARCHAR(50);	
--DECLARE @StepSourceId VARCHAR(50);

--SET @patientid = 45;
--SET @patientprogramid = 304;
--SET @ProgramSourceId ='5453f570bdd4dfcef5000330';
--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';	
--SET @StepSourceId = '5422dd36ac80d3356d000001';
-------
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
-------
--SELECT @CompletedCount;
--SELECT @SavedCount;
--SELECT @ActionNotComplete;
-------	
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' THEN
						pr13.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ArchivedDate in (	SELECT DISTINCT TOP 1 PERCENT  
												ActionArchivedDate 
											FROM RPT_ProgramResponse_Flat 
											WHERE 
												PatientId = @patientid AND 
												PatientProgramId = @patientprogramid AND 
												ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT TOP 1
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pa14.ArchivedDate in (	SELECT DISTINCT TOP 1 PERCENT  
														ActionArchivedDate 
													FROM RPT_ProgramResponse_Flat 
													WHERE 
														PatientId = @patientid AND 
														PatientProgramId = @patientprogramid AND 
														ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)			
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END

GO


/****** Object:  UserDefinedFunction [dbo].[fn_RPT_PCPOther]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_PCPOther]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_PCPOther] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	----------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 582;
	--SET @patientprogramid = 38;
	--SET @ProgramSourceId = ''5453f570bdd4dfcef5000330'';
	--SET @ActionSourceId = ''5453cf73bdd4dfc95100001e'';
	--SET @StepSourceId = ''5422df97ac80d3356d000004'';
	------------------------------------

		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	------
	--select @CompletedCount;
	--select @SavedCount;
	--select @ActionNotComplete;
	------
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != ''0''
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != ''0''
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = ''True'' THEN
					CASE WHEN pr7.[Delete] = ''False'' THEN
						pr7.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				ps7.SourceId= @StepSourceId
				AND pa7.SourceId = @ActionSourceId
				AND pp7.SourceId = @ProgramSourceId
				AND pp7.[Delete] = ''False''
				AND pr7.Selected = ''False''
				AND pa7.[State] IN (''Completed'')
				AND pa7.Archived = ''True''
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT 
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							ps8.SourceId = @StepSourceId
							AND pa8.SourceId = @ActionSourceId
							AND pp8.SourceId = @ProgramSourceId
							AND (pp8.[Delete] = ''False'')
							AND pa8.[State] IN (''Completed'')
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_ProgramResponse_Flat]    Script Date: 05/04/2015 10:48:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_ProgramResponse_Flat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[spPhy_RPT_ProgramResponse_Flat]
AS
BEGIN
	TRUNCATE TABLE RPT_ProgramResponse_Flat
	INSERT INTO RPT_ProgramResponse_Flat
		([PatientId]			,
		[PatientProgramId]	,
		[ProgramSourceId]	,
		[ActionSourceId]	,
		[ActionCompleted]	,
		[ActionArchived]	,
		[ActionArchivedDate],	
		[StepSourceId]		,
		[Text]				,
		[Value]				,
		[Selected]			,
		[Delete],
		[StepCompleted]		,
		[DateCompleted]		,
		[ActionName]	,
		[Question])
	SELECT
		ptx.PatientId as			[PatientId]			,
		ppx.PatientProgramId as		[PatientProgramId]	,
		ppx.SourceId as				[ProgramSourceId]	,
		pax.SourceId as				[ActionSourceId]	,
		pax.Completed as            [ActionCompleted]	,
		pax.Archived as				[ActionArchived]	,
		pax.ArchivedDate as			[ActionArchivedDate],	
		psx.SourceId as				[StepSourceId]		,
		prx.[Text] as				[Text]				,
		prx.[Value] as				[Value]				,
		prx.Selected as				[Selected]			,
		prx.[Delete] as				[Delete]			,
		psx.Completed as			[StepCompleted]		,
		pax.DateCompleted as		[DateCompleted]		,
		pax.Name as					[ActionName],
		psx.Question as				[Question]
	FROM
		RPT_Patient AS ptx with (nolock) INNER JOIN 
		RPT_PatientProgram AS ppx with (nolock)  ON ptx.PatientId = ppx.PatientId
		INNER JOIN RPT_PatientProgramModule AS pmx with (nolock) ON ppx.PatientProgramId = pmx.PatientProgramId  				
		INNER JOIN RPT_PatientProgramAction AS pax with (nolock)  ON pmx.PatientProgramModuleId = pax.PatientProgramModuleId  				
		INNER JOIN RPT_PatientProgramStep AS psx with (nolock) ON pax.ActionId = psx.ActionId  				
		LEFT OUTER JOIN RPT_PatientProgramResponse AS prx with (nolock) ON psx.StepId = prx.StepId
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]    Script Date: 05/04/2015 10:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
AS
BEGIN
	DELETE [RPT_BSHSI_HW2_Enrollment_Info]
	INSERT INTO [RPT_BSHSI_HW2_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Pending_Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Market,
		Disenroll_Date,
		Disenroll_Reason,
		did_not_enroll_date,
		did_not_enroll_reason
	) 
	SELECT DISTINCT 	
		pt.PatientId
		,ppt.PatientProgramId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  	 	
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = ''541943a6bdd4dfa5d90002da''
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,''541943a6bdd4dfa5d90002da'', ''53f4fd75ac80d30e00000083'', ''53f4fb39ac80d30e00000067'')) as [Enrollment]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f57383ac80d31203000033'', ''53f57309ac80d31200000017'' )) as [Program_Completed_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f572caac80d31203000020'', ''53f571e0ac80d3120300001b'')) as [Re_enrollment_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f4fd75ac80d30e00000083'', ''53f4f920ac80d30e00000066'') ) as [Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f4fd75ac80d30e00000083'', ''53f4f7ecac80d30e02000072'')) as [Pending_Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f4fd75ac80d30e00000083'', ''53f4f920ac80d30e00000066'')) as [Enrollment_Action_Completion_Date]
		,( select Market FROM dbo.fn_RPT_Market(pt.PatientId,ppt.PatientProgramId) ) as [Market]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f57115ac80d31203000014'', ''53f56eb7ac80d31203000001'')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,''541943a6bdd4dfa5d90002da'', ''53f57115ac80d31203000014'', ''53f56f10ac80d31200000001'') ) as [Disenroll_Reason]	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, ''541943a6bdd4dfa5d90002da'', ''53f4fd75ac80d30e00000083'', ''53f4fc71ac80d30e02000074'') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,''541943a6bdd4dfa5d90002da'', ''53f4fd75ac80d30e00000083'', ''53f4f885ac80d30e00000065'')) as [did_not_enroll_reason]
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = ''False'' 	
		AND ppt.SourceId = ''541943a6bdd4dfa5d90002da''
		AND ppt.[Delete] = ''False''
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_PCP_Practice_Val]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_PCP_Practice_Val] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 135;
	--SET @patientprogramid = 269;
	--SET @ProgramSourceId = ''5453f570bdd4dfcef5000330'';
	--SET @ActionSourceId = ''5453cf73bdd4dfc95100001e'';
	--SET @StepSourceId = ''5422deccac80d3356d000002'';
	--------------------------------

	DECLARE @CPCTemp VARCHAR(200);
	DECLARE @Result VARCHAR(200);
	
	SET @CPCTemp = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
	SET @CPCTemp = LOWER(RTRIM(LTRIM(@CPCTemp)));
	
	SET @Result =
		CASE
			WHEN @CPCTemp = ''other'' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422df97ac80d3356d000004'') )							
			ELSE
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
		END;

	INSERT INTO @ResultTable (Value) VALUES (@Result);
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_GetPractice_Engage]    Script Date: 05/04/2015 10:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetPractice_Engage]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_GetPractice_Engage] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
	DECLARE @Practice VARCHAR(100);
	DECLARE @Result VARCHAR(200);
	
	--Commonwealth Primary Care
	--Dominion Medical Associates
	--Lee Davis Medical Associates
	--Other
	--Virginia Diabetes and Endocrinology
	--Virginia Family Physicians
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(@patientid, @patientprogramid, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''544efd6fac80d37bc000027b'') );
			
	SET @Practice = RTRIM(LTRIM(@Practice));
	--select @Practice;
			
	--SET @Result = 
	--	CASE					
	--		WHEN @Practice = ''Other'' THEN -- get this from the other open textbox
	--			(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId,''545cee7a890e9458aa000003'', ''544eff8dac80d37bc000027e'') )
				
	--		ELSE
	--			@Practice			
	--	END;
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Practice);
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_PCP_Practice_Val]    Script Date: 05/04/2015 10:48:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_Engage_PCP_Practice_Val] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	----SET @patientid = 546;
	----SET @patientprogramid = 283;
	--SET @patientid = 217;
	--SET @patientprogramid = 280;	
	--SET @ProgramSourceId = ''5465e772bdd4dfb6d80004f7'';
	--SET @ActionSourceId = ''545cee7a890e9458aa000003'';
	--SET @StepSourceId = ''545ce6f8890e9458a9000002'';
	--------------------------------------


	DECLARE @CPCTemp VARCHAR(200);
	DECLARE @Result VARCHAR(200);
	
	SET @CPCTemp = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
	SET @CPCTemp = LOWER(RTRIM(LTRIM(@CPCTemp)));
	
	-------
	--select @CPCTemp;
	-------
	
	SET @Result =
		CASE
			WHEN @CPCTemp = ''other'' THEN
				''other''
			ELSE
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
		END;

	---------
	--select @Result
	---------

	INSERT INTO @ResultTable (Value) VALUES (@Result);
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_GetPCP]    Script Date: 05/04/2015 10:48:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_GetPCP]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_Engage_GetPCP] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	--------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 47;
	--SET @patientprogramid = 111;
	--SET @ProgramSourceId = ''54b69910ac80d33c2c000032'';
	--SET @ActionSourceId = ''5453cf73bdd4dfc95100001e'';
	--SET @StepSourceId = ''5422deccac80d3356d000002'';
	----------------------------------------

	DECLARE @Practice VARCHAR(100);
	DECLARE @Result VARCHAR(200);
	
	--Commonwealth Primary Care
	--Dominion Medical Associates
	--Lee Davis Medical Associates
	--Other
	--Virginia Diabetes and Endocrinology
	--Virginia Family Physicians
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''544efd6fac80d37bc000027b'') );
			
	SET @Practice = RTRIM(LTRIM(@Practice));
	
	-------
	--select @Practice;
	----select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''545ce6f8890e9458a9000002'')
	-------
			
	SET @Result = 
		CASE
			WHEN @Practice = ''Commonwealth Primary Care'' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''545ce698890e9458aa000001'') )

			WHEN @Practice = ''Dominion Medical Associates'' THEN		
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''545ce6c7890e9458aa000002'') )
		
			WHEN @Practice = ''Lee Davis Medical Associates'' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''54598bcdac80d36bd1000001'') )
			
			WHEN @Practice = ''Virginia Diabetes and Endocrinology'' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''545ce65a890e9458a9000001'') )
			                 
			WHEN @Practice = ''Virginia Family Physicians'' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''545ce6f8890e9458a9000002'') )
					
			WHEN @Practice = ''Other'' THEN -- get this from the other open textbox
				NULL --@Practice --(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId,''544eff8dac80d37bc000027e'', ''545cee7a890e9458aa000003'') )

			WHEN @Practice IS NULL THEN -- this is really CPC
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''545cee7a890e9458aa000003'', ''545ce698890e9458aa000001'') )
		END;
	
	---------
	--select @Result;
	---------
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Result);
			
	RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RPT_CareBridge_GetPCP]    Script Date: 05/04/2015 10:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_CareBridge_GetPCP]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_CareBridge_GetPCP] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	----------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 45;
	--SET @patientprogramid = 304;
	--SET @ProgramSourceId = ''5453f570bdd4dfcef5000330'';
	--SET @ActionSourceId = ''5453cf73bdd4dfc95100001e'';
	--SET @StepSourceId = ''5422deccac80d3356d000002'';
	------------------------------------
	
	DECLARE @Practice VARCHAR(100);
	DECLARE @Result VARCHAR(200);
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(@PatientId, @PatientProgramId, ''5453f570bdd4dfcef5000330'', ''5453cf73bdd4dfc95100001e'', ''5422dd36ac80d3356d000001'') );
			
			
	SET @Practice = RTRIM(LTRIM(@Practice));
	-------
	--SELECT @Practice;
	-------
			
	SET @Result = 
		CASE
			WHEN @Practice = ''CPC'' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422de0fac80d3356f000001'') )

			WHEN @Practice = ''DMA'' THEN		
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422de52ac80d3356f000002'') )
		
			WHEN @Practice = ''LDM'' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422deccac80d3356d000002'') )
			
			WHEN @Practice = ''VDE'' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422df12ac80d3356d000003'') )
			
			WHEN @Practice = ''VFP'' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422df50ac80d3356f000003'') )
					
			WHEN @Practice = ''Other'' THEN -- get this from the other open textbox
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId,''5453cf73bdd4dfc95100001e'',''5422df97ac80d3356d000004'') )

			WHEN @Practice IS NULL THEN -- this is really CPC
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422de0fac80d3356f000001'') )
		END;
	
	--select @Result;
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Result);
			
	RETURN
END
' 
END
GO

/****** Object:  Table [dbo].[RPT_Engage_Enrollment_Info]    Script Date: 05/06/2015 16:18:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Engage_Enrollment_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Engage_Enrollment_Info]
GO

/****** Object:  Table [dbo].[RPT_Engage_Enrollment_Info]    Script Date: 05/06/2015 16:18:45 ******/
CREATE TABLE [dbo].[RPT_Engage_Enrollment_Info](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[Priority] [varchar](50) NULL,
	[firstName] [varchar](100) NULL,
	[SystemId] [varchar](50) NULL,
	[LastName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[LSSN] [int] NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[Primary_Physician] [varchar](1000) NULL,
	[Primary_Physician_Practice] [varchar](2000) NULL,
	[Exclusion_Criteria] [varchar](1000) NULL,
	[Program_CM] [varchar](100) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Assigned_Date] [datetime] NULL,
	[Last_State_Update_Date] [datetime] NULL,
	[State] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Program_Completed_Date] [date] NULL,
	[Re_enrollment_Date] [date] NULL,
	[Enrolled_Date] [date] NULL,
	[Pending_Enrolled_Date] [date] NULL,
	[Enrollment_Action_Completion_Date] [date] NULL,
	[Disenroll_Date] [date] NULL,
	[Disenroll_Reason] [varchar](max) NULL,
	[Disenroll_Reason_Other] [varchar](max) NULL,
	[did_not_enroll_date] [date] NULL,
	[did_not_enroll_reason] [varchar](max) NULL,
	[Practice] [varchar](200) NULL,
	[PCP] [varchar](200) NULL,
	[did_not_enroll_reason_other] [varchar](max) NULL,
	[acuity_score] [varchar](200) NULL,
	[acuity_date] [datetime] NULL
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_Engage]    Script Date: 05/04/2015 10:48:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
AS
BEGIN
	DELETE [RPT_Engage_Enrollment_Info]

	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7';
	
	INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score		
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,ppt.PatientProgramId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [Practice]			
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255fa0890e942ba2000001')) as [Enrollment] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f571e0ac80d3120300001b')) as [Re_enrollment_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5450ff07ac80d37bc00002f6', '542561b4890e942ba1000003')) as [Disenroll_Date] 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '542561ec890e942ba1000004') ) as [Disenroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425600e890e942ba2000003') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] 
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'


---------------------------- version 2 ----------------------------------------

SET @ProgramSourceId = '54b69910ac80d33c2c000032'; 
INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score		
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '54b69910ac80d33c2c000032'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,ppt.PatientProgramId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [Practice]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId,'545c0805ac80d36bd4000089', '54255fa0890e942ba2000001')) as [Enrollment]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '54a3625d890e948042000052', '54942ba8ac80d33c29000019')) as [Re_enrollment_Date] --*  	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'545c0805ac80d36bd4000089', '542561b4890e942ba1000003')) as [Disenroll_Date] --*
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '545c0805ac80d36bd4000089', '542561ec890e942ba1000004') ) as [Disenroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --*
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '545c0805ac80d36bd4000089', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date] --(step, action)
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] --	
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'
		
END 
GO

/****** Object:  Table [dbo].[RPT_CareBridge_Enrollment_Info]    Script Date: 05/06/2015 15:52:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_CareBridge_Enrollment_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_CareBridge_Enrollment_Info]
GO

/****** Object:  Table [dbo].[RPT_CareBridge_Enrollment_Info]    Script Date: 05/06/2015 15:52:09 ******/
CREATE TABLE [dbo].[RPT_CareBridge_Enrollment_Info](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[Priority] [varchar](50) NULL,
	[firstName] [varchar](100) NULL,
	[SystemId] [varchar](50) NULL,
	[LastName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[LSSN] [int] NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[Primary_Physician] [varchar](1000) NULL,
	[Primary_Physician_Practice] [varchar](2000) NULL,
	[Exclusion_Criteria] [varchar](1000) NULL,
	[Program_CM] [varchar](100) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Assigned_Date] [datetime] NULL,
	[Last_State_Update_Date] [datetime] NULL,
	[State] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Program_Completed_Date] [date] NULL,
	[Re_enrollment_Date] [date] NULL,
	[Enrolled_Date] [date] NULL,
	[Pending_Enrolled_Date] [date] NULL,
	[Enrollment_Action_Completion_Date] [date] NULL,
	[Disenroll_Date] [date] NULL,
	[Disenroll_Reason] [varchar](max) NULL,
	[Disenroll_Reason_Other] [varchar](max) NULL,
	[did_not_enroll_date] [date] NULL,
	[did_not_enroll_reason] [varchar](max) NULL,
	[Practice] [varchar](200) NULL,
	[PCP] [varchar](200) NULL,
	[did_not_enroll_reason_other] [varchar](max) NULL
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_CareBridge]    Script Date: 05/04/2015 10:48:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_CareBridge]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_CareBridge]
AS
BEGIN
	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = ''5453f570bdd4dfcef5000330'';
	
	DELETE [RPT_CareBridge_Enrollment_Info]
	INSERT INTO [RPT_CareBridge_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other		
	)
		
	
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = ''5453f570bdd4dfcef5000330''; 	
	SELECT DISTINCT 	
		pt.PatientId
		,ppt.PatientProgramId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''543d38bbac80d33fda00002a'') )as [Exclusion_Criteria]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453cf73bdd4dfc95100001e'', ''5422dd36ac80d3356d000001'') )as [Practice]			
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_CareBridge_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, ''5453c6c7bdd4dfc94e000012'',''54255fa0890e942ba2000001'')) as [Enrollment]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''5425620b890e942ba2000005'')) as [Program_Completed_Date] --       	       	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''54255ff8890e942ba2000002'') ) as [Enrolled_Date]  --      	     	
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''54255fa0890e942ba2000001'')) as [Enrollment_Action_Completion_Date] --
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,''5453c6c7bdd4dfc94e000012'', ''542561b4890e942ba1000003'')) as [Disenroll_Date] --	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''542561ec890e942ba1000004'') ) as [Disenroll_Reason] --
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''54264df1890e942ba2000006'') )as [Disenroll_Reason_Other] --
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''5425600e890e942ba2000003'') ) as [did_not_enroll_date] --
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''542560c3890e942ba2000004'')) as [Did_Not_Enroll_Reason] --
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, ''5453c6c7bdd4dfc94e000012'', ''5425611d890e942ba1000001'') )as [Did_Not_Enroll_Reason_Other] --
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = ''False'' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = ''False''
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/04/2015 10:48:22 ******/
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
/****** Object:  ForeignKey [FK_Patient_UserMongoRecordCreatedBy]    Script Date: 05/04/2015 10:47:47 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] CHECK CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
/****** Object:  ForeignKey [FK_Patient_UserMongoUpdatedBy]    Script Date: 05/04/2015 10:47:47 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] CHECK CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
/****** Object:  ForeignKey [FK_PatientProgram_Patient]    Script Date: 05/04/2015 10:47:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgram_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]'))
ALTER TABLE [dbo].[RPT_PatientProgram]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgram_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[RPT_Patient] ([PatientId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgram_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgram]'))
ALTER TABLE [dbo].[RPT_PatientProgram] CHECK CONSTRAINT [FK_PatientProgram_Patient]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleAction_PatientProgramModule]    Script Date: 05/04/2015 10:47:51 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule] FOREIGN KEY([PatientProgramModuleId])
REFERENCES [dbo].[RPT_PatientProgramModule] ([PatientProgramModuleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction] CHECK CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
/****** Object:  ForeignKey [FK_PatientProgramModule_PatientProgram]    Script Date: 05/04/2015 10:47:55 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModule_PatientProgram]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]'))
ALTER TABLE [dbo].[RPT_PatientProgramModule]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModule_PatientProgram] FOREIGN KEY([PatientProgramId])
REFERENCES [dbo].[RPT_PatientProgram] ([PatientProgramId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModule_PatientProgram]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]'))
ALTER TABLE [dbo].[RPT_PatientProgramModule] CHECK CONSTRAINT [FK_PatientProgramModule_PatientProgram]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]    Script Date: 05/04/2015 10:47:57 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep] FOREIGN KEY([NextStepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]    Script Date: 05/04/2015 10:47:57 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep] FOREIGN KEY([StepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]    Script Date: 05/04/2015 10:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction] FOREIGN KEY([ActionId])
REFERENCES [dbo].[RPT_PatientProgramAction] ([ActionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] CHECK CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
/****** Object:  ForeignKey [FK_PatientSystem_Patient]    Script Date: 05/04/2015 10:48:02 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[RPT_Patient] ([PatientId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_Patient]
GO
/****** Object:  ForeignKey [FK_PatientSystem_UserMongoRecordCreatedBy]    Script Date: 05/04/2015 10:48:02 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
/****** Object:  ForeignKey [FK_PatientSystem_UserMongoUpdatedBy]    Script Date: 05/04/2015 10:48:02 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
