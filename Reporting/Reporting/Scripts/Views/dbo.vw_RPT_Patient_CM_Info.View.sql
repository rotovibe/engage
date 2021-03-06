SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Patient_CM_Info]
AS
	SELECT
		PatientId, 
		(select 
			(select c.[Primary] from RPT_CareMember cm with (nolock) INNER JOIN RPT_User u with (nolock) ON cm.UserId = u.UserId where CareMemberId = c.CareMemberId) as [preferred name]
				from RPT_CareMember c with (nolock) where PatientId = pt.PatientId) as [Primary], 
		(select 
			(select u.LastName +', '+ u.FirstName from RPT_CareMember cm with (nolock)  INNER JOIN RPT_User u ON cm.UserId = u.UserId where CareMemberId = c.CareMemberId) as [preferred name]
				from RPT_CareMember c with (nolock)  where PatientId = pt.PatientId) as [name], 
		(select 
			(select u.PreferredName from RPT_CareMember cm with (nolock) INNER JOIN RPT_User u ON cm.UserId = u.UserId where CareMemberId = c.CareMemberId) as [preferred name]
				from RPT_CareMember c with (nolock) where PatientId = pt.PatientId) as [pref_name], 
		pt.LastUpdatedOn
	FROM	
		dbo.RPT_Patient AS pt with (nolock) 
	WHERE
		([Delete] = 'False')
GO
