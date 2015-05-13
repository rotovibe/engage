using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using C3.Business;
using C3.Data;
using C3.Web.Helpers;
using Phytel.Web.Cache;
using Atmosphere.Web;
using System.Collections.Generic;
using Atmosphere.Core;

namespace C3.Web
{
    public class BasePage : System.Web.UI.Page
    {
        #region Protected Override

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Session.IsNewSession && (PhytelPageName != "Login" && PhytelPageName != string.Empty))
            {
                if ((UserPageInfo.SessionId != Session.SessionID) && UserPageInfo.SessionId != null)
                {
                    UserPageInfo.SessionId = Session.SessionID;

                    Response.Redirect(GlobalSiteRoot + "Login.aspx?timeout=1", true);
                }
            }

            if (string.IsNullOrEmpty(UserPageInfo.SessionId))
                UserPageInfo.SessionId = Session.SessionID;
        }

        protected override void OnLoad(EventArgs e)
        {
            PageInfo info = UserHelper.RedirectUser(CurrentUser, false);

            if (PhytelPageName == info.RedirectPageName || (info.UserIsValid && PhytelPageName != info.RedirectPageName))
            {
                SetInformationText(ApplicationMessageService.Instance.GetMessage(info.InformationMessageCode));

                if (info.ErrorMessageCode != string.Empty)
                    SetPageErrorText(ApplicationMessageService.Instance.GetMessage(info.ErrorMessageCode));
                else if (info.LastSystemErrorMessage != null)
                    SetPageErrorText(ApplicationMessageService.Instance.GetMessage("ERR_026"), info.LastSystemErrorMessage.Message);

                info.ErrorMessageCode = string.Empty;
                info.InformationMessageCode = string.Empty;
                info.LastSystemErrorMessage = null;

                if (CurrentUser != null && !IsPostBack)
                {
                    var auditLog = GetAuditLog();
                    auditLog.Type = "PageView";
                    AuditService.Instance.LogEvent(auditLog);
                }
            }

            if (!info.UserIsValid && PhytelPageName != info.RedirectPageName)
            {
                if (info.ClearUserSession)
                    UserHelper.ClearUserSessionVariables(true);

                if ((PhytelPageName != "Login") && (PhytelPageName != "ForgotPassword"))
                {
                    Response.Redirect(info.RedirectURL);
                }
            }


            base.OnLoad(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            PageInfo info = UserHelper.RedirectUser(CurrentUser, false);

            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer != null)
                    info.LastPage = (Request.UrlReferrer.PathAndQuery != info.LastPage ? Request.UrlReferrer.PathAndQuery : info.LastPage);
            }
            base.OnLoadComplete(e);
        }

        //ORIGINAL CODE
        //This overrides where the ViewState is stored.
        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    object viewStateBag = null;
        //    string m_viewState = (string)Session["ViewState"];
        //    LosFormatter m_formatter = new LosFormatter();
        //    try
        //    {
        //        viewStateBag = m_formatter.Deserialize(m_viewState);
        //    }
        //    catch
        //    {
        //        UserHelper.RedirectUser(CurrentUser);
        //    }
        //    return viewStateBag;
        //}

        ////This saves the viewstate into the session on the server
        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    LosFormatter m_formatter = new LosFormatter();
        //    m_formatter.Serialize(ms, viewState);
        //    ms.Position = 0;
        //    StreamReader sr = new StreamReader(ms);
        //    string viewStateString = sr.ReadToEnd();
        //    Session["ViewState"] = viewStateString;
        //    ms.Close();
        //    return;
        //}

        //This overrides where the ViewState is stored.
        protected override object LoadPageStateFromPersistenceMedium()
        {
            TimeSpan cacheTimeout;

            if (CurrentUser != null && CurrentUser.SessionTimeoutInterval > 0)
                cacheTimeout = new TimeSpan(0, CurrentUser.SessionTimeoutInterval, 0);
            else
                cacheTimeout = new TimeSpan(2, 0, 0);

            return MongoWebCache.Instance.Get<object>(Session.SessionID, this.Request.Path + "-ViewState", cacheTimeout);
            //return Session[this.Request.Path + "-ViewState"];
        }

        //This saves the viewstate into the session on the server
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            TimeSpan cacheTimeout;

            if (CurrentUser != null && CurrentUser.SessionTimeoutInterval > 0)
                cacheTimeout = new TimeSpan(0, CurrentUser.SessionTimeoutInterval, 0);
            else
                cacheTimeout = new TimeSpan(2, 0, 0);

            MongoWebCache.Instance.Put<object>(Session.SessionID, this.Request.Path + "-ViewState", viewState, cacheTimeout);
            //Session[this.Request.Path + "-ViewState"] = viewState;
        }

        protected override void OnError(EventArgs e)
        {
            Exception ex2 = Server.GetLastError();
            Exception ex;
            ex = ex2;

            if (CurrentUser != null)
                CurrentUser.UserPageInfo.LastSystemErrorMessage = ex;
            else
                ((PageInfo)Session["UserPageInfo"]).LastSystemErrorMessage = ex;

            var auditLog = GetAuditLog();
            if (ex != null)
            {
                auditLog.Message = AuditService.Instance.PopulateMessage(ex);
            }
            else
            {
                auditLog.Message = AuditService.Instance.PopulateMessage(ApplicationMessageService.Instance.GetMessage("ERR_026"));
            }
            auditLog.Type = "SystemError";
            AuditService.Instance.LogEvent(auditLog);

            UserHelper.RedirectUser(CurrentUser, GlobalSiteRoot + "/ErrorMessage.aspx", "ErrorMessage", true);
            base.OnError(e);
        }

        #endregion

        #region Public Properties

        public string GlobalSiteRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["GlobalSiteRoot"];
            }
        }

        public PageInfo UserPageInfo
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.UserPageInfo;
                else
                    return ((PageInfo)Session["UserPageInfo"]);
            }
            set
            {
                Session["UserPageInfo"] = value;
            }
        }

        public bool DisplayInformationText
        {
            get
            {
                System.Web.UI.Control infLabel = BaseMaster.FindControl("formInformationMessage");
                return infLabel.Visible;
            }

            set
            {
                System.Web.UI.Control infLabel = BaseMaster.FindControl("formInformationMessage");
                infLabel.Visible = value;
            }
        }

        public string PhytelPageName
        {
            get
            {
                return (string)Session["PhytelPageName"];
            }
            set
            {
                Session["PhytelPageName"] = value;
            }
        }

        public C3User CurrentUser
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
                    //TODO: Need to throw exception and go to error message page.                    
                    return null;
                }
            }
            set { Session["C3User"] = value; }
        }

        public Data.Control Control
        {
            get
            {
                Data.Control _control = new Data.Control();

                _control = ControlService.Instance.GetControlByName(PhytelPageName);
                //Temporary Code for pages to continue to work
                if (_control == null) { _control = new Data.Control(); }
                return _control;
            }
        }

        public AuditData GetAuditLog()
        {
            return AuditHelper.GetAuditLog();
        }

        public void LogAuditEvent(string auditType, List<int> patients)
        {
            if (patients.HasItems())
            {
                List<int> auditList = new List<int>(patients);

                while (auditList.HasItems())
                {
                    int count = auditList.Count > 1000 ? 1000 : auditList.Count;
                    List<int> patientListChunk = new List<int>(auditList.GetRange(0, count));
                    auditList.RemoveRange(0, count);

                    if (patientListChunk.Count > 0)
                    {
                        var auditLog = GetAuditLog();
                        auditLog.Type = auditType;
                        auditLog.Patients = patientListChunk;
                        AuditService.Instance.LogEvent(auditLog);
                    }
                }
            }
            else
            {
                var auditLog = GetAuditLog();
                auditLog.Type = auditType;
                AuditService.Instance.LogEvent(auditLog);
            }

        }

        #endregion

        #region Public Methods
        public void SetPageHeaderText(string headerText)
        {
            try
            {
                Label headerLabel = (Label)BaseMaster.FindControl("formHeaderText");
                headerLabel.Text = headerText;
            }
            catch { }
        }

        public void SetPageErrorText(ApplicationMessage appMessage, string specificErrorText)
        {
            try
            {
                string errorText = specificErrorText;

                Label errLabel = (Label)BaseMaster.FindControl("formErrorMessage");
                errLabel.Text = errorText;

                HtmlGenericControl divError = (HtmlGenericControl)BaseMaster.FindControl("divError");

                divError.Attributes.Add("class", (string.IsNullOrEmpty(errorText) ? "errorMessageContainerHidden" : "errorMessageContainer"));
                ((UpdatePanel)BaseMaster.FindControl("MasterUpdatePanel")).Update();
            }
            catch { }

            AuditErrorMessage(appMessage);
        }

        protected void AuditErrorMessage(ApplicationMessage appMessage)
        {
            if (appMessage != null && appMessage.Audit)
            {
                var auditLog = GetAuditLog();
                auditLog.Type = "ErrorMessage";
                auditLog.Message = AuditService.Instance.PopulateMessage(appMessage);
                AuditService.Instance.LogEvent(auditLog);
            }
        }

        public void SetPageErrorText(ApplicationMessage appMessage)
        {
            try
            {
                string errorText = (appMessage != null ? appMessage.Text : "Unknown Error");

                Label errLabel = (Label)BaseMaster.FindControl("formErrorMessage");
                errLabel.Text = errorText;

                HtmlGenericControl divError = (HtmlGenericControl)BaseMaster.FindControl("divError");

                divError.Attributes.Add("class", (string.IsNullOrEmpty(errorText) ? "errorMessageContainerHidden" : "errorMessageContainer"));
                ((UpdatePanel)BaseMaster.FindControl("MasterUpdatePanel")).Update();
            }
            catch { }

            AuditErrorMessage(appMessage);
        }

        public void SetInformationText(ApplicationMessage appMessage)
        {
            try
            {
                string informationText = (appMessage != null ? appMessage.Text : string.Empty);

                Label infLabel = (Label)BaseMaster.FindControl("formInformationMessage");
                infLabel.Text = informationText;

                HtmlGenericControl divError = (HtmlGenericControl)BaseMaster.FindControl("divInformation");
                divError.Attributes.Add("class", (string.IsNullOrEmpty(informationText) ? "informationMessageContainerHidden" : "informationMessageContainer"));
                ((UpdatePanel)BaseMaster.FindControl("MasterUpdatePanel")).Update();
            }
            catch { }
        }

        public void SetInformationText(ApplicationMessage appMessage, string alternateText)
        {
            try
            {
                string informationText = (alternateText != String.Empty ? alternateText : string.Empty);

                Label infLabel = (Label)BaseMaster.FindControl("formInformationMessage");
                infLabel.Text = informationText;

                HtmlGenericControl divError = (HtmlGenericControl)BaseMaster.FindControl("divInformation");
                divError.Attributes.Add("class", (string.IsNullOrEmpty(informationText) ? "informationMessageContainerHidden" : "informationMessageContainer"));
            }
            catch { }
        }

        #endregion

        protected MasterPage BaseMaster
        {
            get
            {
                MasterPage master = Master;
                while (master.Master != null)
                {
                    master = master.Master;
                }
                return master;
            }
        }
    }
}