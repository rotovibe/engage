using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;
using System.Web;
using C3.Web.Helpers;

namespace Atmosphere.Web
{
    public class AuditHelper
    {
        /// <summary>
        /// Gets an auditdata record for the current context
        /// </summary>
        /// <returns></returns>
        public static AuditData GetAuditLog()
        {
            AuditData auditLog = new AuditData();

            var session = HttpContext.Current.Session;

            auditLog.SourceIP = HttpContext.Current.Request.UserHostAddress;
            auditLog.Browser = HttpContext.Current.Request.Browser.Type;
            auditLog.SessionId = session.SessionID;
            var user = (C3User)session["C3User"];
            auditLog.UserId = user == null ? Guid.Empty : user.UserId;
            auditLog.ImpersonatingUserId = user == null || user.ImpersonatingUserId == Guid.Empty ? Guid.Empty : user.ImpersonatingUserId;
            auditLog.EditedUserId = Guid.Empty;
            auditLog.ContractID = user == null ? 0 : user.CurrentContract.ContractId;
            auditLog.EnteredUserName = string.Empty;
            auditLog.EventDateTime = DateTime.Now;

            string pageName = (string)session["PhytelPageName"];
            auditLog.SourcePage = !String.IsNullOrEmpty(pageName) ? pageName : string.Empty;
            return auditLog;
        }
    }
}
