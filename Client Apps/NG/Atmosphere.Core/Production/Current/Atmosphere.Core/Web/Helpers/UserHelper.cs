using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net.Mail;

using C3.Data;
using C3.Business;
using Phytel.Framework.SQL.Data;
using System.Configuration;
using Phytel.Web.Cache;

//using Phytel.CQS.Utility.Persistence;
//using Phytel.CQS.Model.Interfaces;

namespace C3.Web.Helpers
{
    public static class UserHelper
    {
        public static string GlobalSiteRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["GlobalSiteRoot"];
            }
        }

        #region public methods

        #region User Management
        public static PageInfo RedirectUser(C3User user)
        {
            return RedirectUser(user, string.Empty, string.Empty, true);
        }
        public static PageInfo RedirectUser(C3User user, bool autoRedirect)
        {
            return RedirectUser(user, string.Empty, string.Empty, autoRedirect);
        }
        public static PageInfo RedirectUser(C3User user, string specificURL, string specificPageName, bool autoRedirect)
        {
            PageInfo info = null;

            if (user != null)
                info = user.UserPageInfo;
            else
                info = (PageInfo)HttpContext.Current.Session["UserPageInfo"];

            info.RedirectPageName = specificPageName;
            info.RedirectURL = specificURL;
            info.UserIsValid = true;
            info.ClearUserSession = false;
            info.UserTimedOut = false;

            if (specificPageName != "ErrorMessage")
            {
                if (user == null || (!HttpContext.Current.User.Identity.IsAuthenticated && String.IsNullOrEmpty(specificURL)))
                {
                    info.ClearUserSession = true;

                    info.UserIsValid = false;
                    info.RedirectPageName = "Login";
                    info.RedirectURL = GlobalSiteRoot + "Login.aspx";
                }
                else if (user.FirstTimeUser && !user.IsBeingImpersonated)
                {
                    info.UserIsValid = false;
                    info.ErrorMessageCode = "ERR_031";
                    info.RedirectPageName = "ChangePassword";
                    info.RedirectURL = GlobalSiteRoot + "ChangePassword.aspx";
                }
                else if (user.PasswordIsExpired && !user.IsBeingImpersonated)
                {
                    info.UserIsValid = false;
                    info.ErrorMessageCode = "ERR_003";
                    info.RedirectPageName = "ChangePassword";
                    info.RedirectURL = GlobalSiteRoot + "ChangePassword.aspx";
                }
                else if (string.IsNullOrEmpty(user.PasswordQuestion) && !user.IsBeingImpersonated)
                {
                    info.UserIsValid = false;
                    info.RedirectPageName = "SecurityQuestion";
                    info.RedirectURL = GlobalSiteRoot + "SecurityQuestion.aspx";
                }
                else if (!user.AcceptedLatestTOS && !user.IsBeingImpersonated)
                {
                    info.UserIsValid = false;
                    info.RedirectPageName = "TermsOfService";
                    info.RedirectURL = GlobalSiteRoot + "TermsOfService.aspx";
                }
                else if (user.UserType == Data.Enum.UserTypes.PhytelUser)
                {
                    if (specificPageName != "ChangePassword")
                    {
                        info.InformationMessageCode = "INF_020";
                        info.RedirectPageName = "Impersonation";
                        info.RedirectURL = GlobalSiteRoot + "Admin/Impersonation.aspx";
                    }
                }
                else if (ManageSession(false))
                {
                    info.ErrorMessageCode = "INF_SESSTO";
                    info.ClearUserSession = true;
                    info.UserIsValid = false;
                    info.UserTimedOut = true;
                    info.RedirectPageName = "Login";
                    info.RedirectURL = GlobalSiteRoot + "Login.aspx";
                }
            }

            if (info.RedirectURL == string.Empty)
            {
                //TODO: Set error message = no permissions, can't help you.
                if (user.Controls.Count == 0)
                {
                    info.ErrorMessageCode = "ERR_038";
                    info.ClearUserSession = true;
                    info.RedirectPageName = "Login";
                    info.RedirectURL = GlobalSiteRoot + "Login.aspx";
                }
                else
                {
                    info.RedirectURL = GlobalSiteRoot + user.Controls[0].Path;
                    info.RedirectPageName = user.Controls[0].Name;

                    if (user != null && user.LoginToken.Trim() == string.Empty)
                    {
                        UserService userSvc = new UserService();
                        user.LoginToken = userSvc.GenerateToken(user.UserId);
                    }
                }
            }

            if (user != null && user.LoginToken.Trim() != string.Empty)
            {
                info.RedirectURL += "/#ut/" + user.LoginToken;

                //this is a one time token redirect, so after we redirect them, clear the login token
                user.LoginToken = string.Empty;
            }

            if (autoRedirect || info.UserTimedOut)
            {
                if (info.ClearUserSession)
                    UserHelper.ClearUserSessionVariables(true);

                HttpContext.Current.Response.Redirect(info.RedirectURL);
            }
            return info;
        }
        public static bool ManageSession(bool removeSession)
        {
            bool delete = false;
            C3User user = HttpContext.Current.Session["C3User"] as C3User;
            if (user != null)
            {
                delete = removeSession ? removeSession : user.SessionExpiration != null ? user.SessionExpiration <= DateTime.Now : removeSession;
                UserService svc = new UserService();
                DateTime? expiration = svc.SaveSession(user.UserId, HttpContext.Current.Session.SessionID, user.SessionTimeoutInterval, delete);
                user.SessionExpiration = expiration;
            }

            return delete;
        }
        public static void ClearUserSessionVariables(bool clearAll)
        {
            //We do this because we do not want to clear all session variables, so we need to clear out specific session variables
            ManageSession(true);
            C3User user = (C3User)HttpContext.Current.Session["C3User"];

            
            MongoWebCache.Instance.ClearCache(HttpContext.Current.Session.SessionID);
            if (clearAll)
            {
                var userPageInfo = HttpContext.Current.Session["UserPageInfo"];
                var phytelPageName = HttpContext.Current.Session["PhytelPageName"];

                HttpContext.Current.Session.Clear();

                HttpContext.Current.Session["UserPageInfo"] = userPageInfo;
                HttpContext.Current.Session["PhytelPageName"] = phytelPageName;

               
            }
        }
        #endregion

        #region User Contract Info
        public static Contract GetDefaultContract(C3User user)
        {
            Contract contract = new Contract();

            if (user.Contracts.Count > 1)
            {
                for (int i = 0; i < user.Contracts.Count; i++)
                {
                    if (user.Contracts[i].DefaultContract == true)
                    {
                        contract = user.Contracts[i];
                        break;
                    }
                }
            }

            if (contract.ContractId == 0)
                contract = user.Contracts[0];

            return contract;
        }
        #endregion

        #region Email Functions
        public static void SendEmail(C3User user, string subject, string body)
        {
            try
            {
                // Setup email object
                // Place the patient specific details into the body of the email object                
                string emailBody = body;
                emailBody = emailBody.Replace("<UserFirstName>", user.FirstName);
                emailBody = emailBody.Replace("<UserLastName>", user.LastName);
                emailBody = emailBody.Replace("<br/>", Environment.NewLine);

                SendEmail(emailBody, subject, user.Email, user.DisplayName);
            }
            catch { }
        }
        public static void SendEmail(C3User user, string subject, string body, C3User adminUser)
        {
            try
            {
                // Setup email object
                // Place the patient specific details into the body of the email object                
                string emailBody = body;
                emailBody = emailBody.Replace("<AdministratorFirstName>", adminUser.FirstName);
                emailBody = emailBody.Replace("<AdministratorLastName>", adminUser.LastName);
                emailBody = emailBody.Replace("<User Name>", user.UserName);
                emailBody = emailBody.Replace("<br/>", Environment.NewLine);

                SendEmail(emailBody, subject, adminUser.Email, adminUser.DisplayName);
            }
            catch { }
        }
        public static void SendEmail(C3User user, string subject, string body, string[] reports)
        {
            try
            {
                // Setup email object
                // Place the patient specific details into the body of the email object                
                string emailBody = body;
                emailBody = emailBody.Replace("<AdministratorFirstName>", user.FirstName);
                emailBody = emailBody.Replace("<AdministratorLastName>", user.LastName);

                StringBuilder sb = new StringBuilder();
                foreach (string report in reports)
                {
                    sb.Append(" - ");
                    sb.Append(report);
                    sb.Append("<br/>");
                }
                emailBody = emailBody.Replace("<Name of Report>", sb.ToString());
                emailBody = emailBody.Replace("<c3 website>", ApplicationSettingService.Instance.GetSetting("C3_URL").Value);
                emailBody = emailBody.Replace("<br/>", Environment.NewLine);

                SendEmail(emailBody, subject, user.Email, user.DisplayName);
            }
            catch { }
        }

        public static void SendEmail(C3User user, string subject, string body, C3User adminUser, string assignedContract)
        {
            try
            {
                // Setup email object
                // Place the patient specific details into the body of the email object                
                string emailBody = body;
                emailBody = emailBody.Replace("<AdministratorFirstName>", adminUser.FirstName);
                emailBody = emailBody.Replace("<AdministratorLastName>", adminUser.LastName);
                emailBody = emailBody.Replace("<UserFirstName>", user.FirstName);
                emailBody = emailBody.Replace("<UserLastName>", user.LastName);
                emailBody = emailBody.Replace("<AssignedContract>", assignedContract);
                emailBody = emailBody.Replace("<br/>", Environment.NewLine);
                SendEmail(emailBody, subject, adminUser.Email, adminUser.DisplayName);
            }
            catch
            {
            }
        }

        public static void SendEmail(string firstName, string lastName, string displayname, string address, string subject, string body, bool isBodyHTML)
        {
            // Setup email object
            // Place the patient specific details into the body of the email object                
            string emailBody = body;
            emailBody = emailBody.Replace("<UserFirstName>", firstName);
            emailBody = emailBody.Replace("<UserLastName>", lastName);
            emailBody = emailBody.Replace("<br/>", Environment.NewLine);

            SendEmail(emailBody, subject, address, displayname, isBodyHTML);
        }

        private static void SendEmail(string body, string subject, string address, string display, bool IsBodyHtml = false)
        {
            MailMessage msg = new MailMessage();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException("Recipient Email Address is null");
                }
                else
                {
                    // Get email details from ApplicationSetting object
                    string emailServer = ApplicationSettingService.Instance.GetSetting("SMTP_SERVER").Value;
                    string fromAddress = ApplicationSettingService.Instance.GetSetting("EMAIL_FROM_ADDRESS").Value;
                    string fromDisplay = ApplicationSettingService.Instance.GetSetting("EMAIL_FROM_DISPLAY").Value;

                    // Setup email server object
                    SmtpClient smtp = new SmtpClient(emailServer);

                    if(emailServer.ToUpper().Trim() == "LOCALHOST")
                        smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;

                    // Populate properties of the email object
                    msg.From = new MailAddress(fromAddress, fromDisplay);
                    msg.IsBodyHtml = IsBodyHtml;
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.To.Add(new MailAddress(address, display));

                    // Send the email            
                    smtp.Send(msg);
                }
            }
            finally
            {
                msg.Dispose();
            }
        }
        #endregion

        #endregion
    }
}
