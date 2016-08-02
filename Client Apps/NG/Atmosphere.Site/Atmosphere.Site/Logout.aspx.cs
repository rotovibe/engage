using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atmosphere.Web;
using C3.Business;
using C3.Data;
using C3.Web.Helpers;

namespace Atmosphere
{
    public partial class Logout : System.Web.UI.Page
    {
        protected C3User CurrentUser
        {
            get
            {
                C3User _user;
                _user = (C3User)Session["C3User"];
                if (_user != null)
                {
                    return _user;
                }
                else
                {
                    return null;
                }
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Log Audit for Log Out
            AuditData auditLog = AuditHelper.GetAuditLog();
            auditLog.Type = "SignOut";
            auditLog.SourcePage = System.IO.Path.GetFileName(Request.PhysicalPath);
            AuditService.Instance.LogEvent(auditLog);

            UserHelper.ClearUserSessionVariables(true);
            System.Web.Security.FormsAuthentication.SignOut();
            UserHelper.RedirectUser(CurrentUser);
        }
    }
}