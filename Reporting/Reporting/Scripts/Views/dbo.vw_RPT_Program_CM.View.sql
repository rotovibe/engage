SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Program_CM]
AS
select p.patientid, pp.sourceid, pp.Name, pp.assignedToId, (u.lastname + ', ' + u.firstname) as [preferred_name], (u.firstname + ' ' + u.lastname) as [fullname]
from 
	rpt_patient as p with (nolock)
	INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.patientid = pp.patientid
	INNER JOIN rpt_contact as ct with (nolock) ON ct.contactid = pp.assignedtoid 
	INNER JOIN rpt_user as u with (nolock) ON ct.contactid = u.userid
GO
