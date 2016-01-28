IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_GoalStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_GoalStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_GoalStatistics]
As
Begin
	Truncate Table [RPT_Flat_GoalStatistics]

	Create Table #GoalAttribute (
		MongoPatientGoalId VARCHAR(50),
		GoalAttributeName  VARCHAR(100),
		GoalAttributeValue VARCHAR(50)
	)

	Insert Into #GoalAttribute(
		MongoPatientGoalId, 
		GoalAttributeName, 
		GoalAttributeValue
		)
		Select pga.MongoPatientGoalId, ga.Name, gao.Value
			From RPT_PatientGoalAttribute pga with (nolock)
				LEFT OUTER JOIN RPT_GoalAttribute ga with (nolock) on pga.MongoGoalAttributeId = ga.MongoId
				LEFT OUTER JOIN RPT_PatientGoalAttributeValue pgav with (nolock) on pga.PatientGoalAttributeId = pgav.PatientGoalAttributeId
				LEFT OUTER JOIN RPT_GoalAttributeOption gao with (nolock) on pgav.Value = gao.[Key] and ga.MongoId = gao.MongoGoalAttributeId

	Insert Into [RPT_Flat_GoalStatistics]
	(
		MongoGoalId, 
		MongoPatientId,
		Confidence, 
		Importance, 
		StageofChange, 
		CreatedOn, 
		CreatedBy, 
		UpdatedOn, 
		UpdatedBy, 
		Name, 
		Details, 
		TemplateId, 
		[Source],
		TargetDate, 
		TargetValue, 
		[Status], 
		StartDate, 
		EndDate,
		FocusAreas,
		[Type],
		PrimaryCareManagerMongoId,
		PrimaryCareManagerPreferredName
	)
	Select 
		pg.MongoId as MongoGoalId
		,pg.MongoPatientId
		,gasub1.GoalAttributeValue as Confidence
		,gasub2.GoalAttributeValue as Importance
		,gasub3.GoalAttributeValue as 'StageofChange'
		,pg.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pg.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pg.Name
		,pg.Details
		,pg.TemplateId 
		,pgl.Name as Source
		,pg.TargetDate
		,pg.TargetValue
		,pg.[Status]
		,pg.StartDate
		,pg.EndDate
		, STUFF(
				(Select
					Distinct '| ' + pgfa.Name
				From RPT_PatientGoalFocusArea pgf with (nolock)
					LEFT OUTER JOIN RPT_FocusAreaLookUp pgfa with (nolock) on pgf.MongoFocusAreaId = pgfa.MongoId
				Where pgf.MongoPatientGoalId = pg.MongoId
				Order By '| ' + pgfa.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as FocusAreas
		,pg.[Type] as [Type]
		,u1.MongoId AS 'PrimaryCareManagerMongoId'
		,u1.PreferredName AS 'PrimaryCareManagerPreferredName'
	From RPT_PatientGoal as pg with (nolock) 
		LEFT OUTER JOIN RPT_SourceLookUp as pgl with (nolock) on pg.Source = pgl.MongoId
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Confidence'
				) gasub1 ON pg.MongoId = gasub1.MongoPatientGoalId
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Importance'
				) gasub2 ON pg.MongoId = gasub2.MongoPatientGoalId				
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Stage of Change'
				) gasub3 ON pg.MongoId = gasub3.MongoPatientGoalId
		LEFT OUTER JOIN [RPT_CareMember] cm WITH(NOLOCK) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u1 WITH(NOLOCK) on cm.MongoUserId = u1.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pg.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pg.MongoUpdatedBy = u3.MongoId							
	Where pg.[Delete] = 'False' and pg.TTLDate IS NULL

	Drop Table #GoalAttribute

End

GO