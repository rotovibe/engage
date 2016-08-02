using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Security.Principal;
using System.Web.Security;
using C3.Data;
using C3.Web.Helpers;
using C3.Business;
using System.Net;

namespace C3.Web
{
    public partial class Login : BasePage
    {
        
        /// <summary>
        /// updLogin control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.UpdatePanel updLogin;
        
        /// <summary>
        /// Panel1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel Panel1;
        
        /// <summary>
        /// modal_pop_panel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl modal_pop_panel;
        
        /// <summary>
        /// modal_pop control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl modal_pop;
        
        /// <summary>
        /// ui_dialog_title control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl ui_dialog_title;
        
        /// <summary>
        /// ui_dialog_text control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl ui_dialog_text;
        
        /// <summary>
        /// btnContinuePassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnContinuePassword;
        
        /// <summary>
        /// btnCancelPassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnCancelPassword;
        
        /// <summary>
        /// pageRedirect control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlInputHidden pageRedirect;
        
        /// <summary>
        /// divLogin control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divLogin;
        
        /// <summary>
        /// ValidationSummary1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
        
        /// <summary>
        /// UserNameLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label UserNameLabel;
        
        /// <summary>
        /// UserName control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox UserName;
        
        /// <summary>
        /// UserNameRequired control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.RequiredFieldValidator UserNameRequired;
        
        /// <summary>
        /// PasswordLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label PasswordLabel;
        
        /// <summary>
        /// Password control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox Password;
        
        /// <summary>
        /// PasswordRequired control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.RequiredFieldValidator PasswordRequired;
        
        /// <summary>
        /// btnLogin control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnLogin;
        
        /// <summary>
        /// PasswordRecoveryLink control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.HyperLink PasswordRecoveryLink;
        
        /// <summary>
        /// divPhytelNews control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divPhytelNews;
        
        /// <summary>
        /// news1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel news1;
        
        /// <summary>
        /// dataSource1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.XmlDataSource dataSource1;
        
        /// <summary>
        /// dataList1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.DataList dataList1;
        
        /// <summary>
        /// news1alternate control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel news1alternate;
        
        /// <summary>
        /// dataSource1alternate control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.XmlDataSource dataSource1alternate;
        
        /// <summary>
        /// dataList1alternate control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.DataList dataList1alternate;
        
        /// <summary>
        /// news1missing control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel news1missing;
        
        /// <summary>
        /// news1missingText control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label news1missingText;

        #region Protected 
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            SetPageProperties();

            if (CurrentUser != null) //Someone is already logged in, redirect them to the home page
                UserHelper.RedirectUser(CurrentUser);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ((UpdateProgress)BaseMaster.FindControl("UpdateProgress1")).AssociatedUpdatePanelID = updLogin.UniqueID;
            Form.DefaultFocus = "Content_UserName";
            Form.DefaultButton = null;
            UserName.Focus();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.SetPageHeaderText("Sign In");

            base.SetInformationText(ApplicationMessageService.Instance.GetMessage("INF_022"));

            if (Request.QueryString["timeout"] != null && Request.QueryString["timeout"].Equals("1"))
                SetPageErrorText(ApplicationMessageService.Instance.GetMessage("INF_SESSTO"));

            base.OnPreRender(e);
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            LoginPage();
        }

        public void LoginPage()
        {
            HidePopup();
            SetPageErrorText(ApplicationMessageService.Instance.GetMessage(string.Empty));

            bool invalid = false;
            
            if (Membership.ValidateUser(UserName.Text, Password.Text))
            {
                string[] validationResults = new UserService().ValidateUser(UserName.Text);

                if (validationResults.Length > 0)
                {
                    if (validationResults[0] != "Active")
                    {
                        UserPageInfo.ErrorMessageCode = validationResults[1];
                        SetPageErrorText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.ErrorMessageCode));

                        invalid = true;
                    }
                    else
                    {
                        if (int.Parse(validationResults[2]) <= 14 && int.Parse(validationResults[2]) > 0)
                        {
                            string msg = ApplicationMessageService.Instance.GetMessage("INF_PWWRN").Text;
                            msg = msg.Replace("<Number>", validationResults[2]);
                            ui_dialog_title.InnerHtml = "Password Expiration";
                            ui_dialog_text.InnerHtml = msg;
                            btnContinuePassword.CssClass = "button"; // "sign_in_button_style";
                            btnCancelPassword.CssClass = "button"; // "sign_in_button_style";
                            ShowPopup();
                        }
                        else
                            LoginUser(null);
                    }
                }
                else
                {
                    UserPageInfo.ErrorMessageCode = "ERR_002";
                    SetPageErrorText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.ErrorMessageCode));

                    invalid = true;
                }
            }
            else
            {
                UserPageInfo.ErrorMessageCode = "ERR_002";
                MembershipUser muser = Membership.GetUser(UserName.Text);
                if (muser != null && Membership.GetUser(UserName.Text).IsLockedOut)
                    UserPageInfo.ErrorMessageCode = "ERR_012";

                invalid = true;

                SetPageErrorText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.ErrorMessageCode));
            }

            if (invalid)
            {
                var auditLog = GetAuditLog();
                auditLog.EnteredUserName = UserName.Text;
                auditLog.Type = "InvalidSignIn";
                AuditService.Instance.LogEvent(auditLog);
            }
        }

        protected void Initiate_Login(object sender, EventArgs e)
        {
            HidePopup();
            LoginUser(null);
        }

        protected void Initiate_Login_PasswordChange(object sender, EventArgs e)
        {
            HidePopup();
            LoginUser("~/ChangePassword.aspx");
        }

        protected void Hide_Popup(object sender, EventArgs e)
        {
            HidePopup();
        }

        #endregion

        #region Private
        private Message message = new Message();

        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "Login";
        }

        private void ShowPopup()
        {
            UserName.Attributes.Add("readonly", "readonly");
            Password.Attributes.Add("readonly", "readonly");
            PasswordRecoveryLink.Enabled = false;
            btnLogin.Enabled = false;
            //divPhytelNews.Disabled = true;
            modal_pop.Attributes["class"] = "ui-dialog ui-widget-content show";
            modal_pop_panel.Attributes["class"] = "ui-widget-overlay show";
            btnCancelPassword.Focus();
        }
        private void HidePopup()
        {
            UserName.Attributes.Remove("readonly");
            Password.Attributes.Remove("readonly");
            PasswordRecoveryLink.Enabled = true;
            btnLogin.Enabled = true;
            //divPhytelNews.Disabled = false;
            modal_pop.Attributes["class"] = "ui-dialog ui-widget-content hide";
            modal_pop_panel.Attributes["class"] = "ui-widget-overlay hide";
        }

        private void LoginUser(string destination)
        {
            MembershipUser muser = Membership.GetUser(UserName.Text);
            C3User user = new C3User(muser);
            Session.Add("C3User", user);
            UserHelper.ManageSession(false);

            destination = Request["ReturnURL"] == null ? destination : Request["ReturnURL"];

            if (string.IsNullOrEmpty(destination))
            destination = user.Controls.Count == 0 ? "~/AccessDenied.aspx" : user.Controls[0].Path;

            if (!string.IsNullOrEmpty(UserHelper.GetDefaultContract(user).ContractId.ToString()))
            {
                user.CurrentContract = UserHelper.GetDefaultContract(user);
            }

            PageInfo info = UserHelper.RedirectUser(user, destination, string.Empty, false);

            var auditLog = GetAuditLog();
            auditLog.Type = "SignIn";
            auditLog.LandingPage = info.RedirectURL;
            AuditService.Instance.LogEvent(auditLog);
            
            FormsAuthentication.SetAuthCookie(UserName.Text, false);

            Response.Redirect(info.RedirectURL);
        }

        private static bool IsAvailable(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AllowAutoRedirect = false;
            req.Method = "HEAD";
            req.KeepAlive = false;
            req.Timeout = 2000;
            try
            {
                return (((HttpWebResponse)req.GetResponse()).StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
