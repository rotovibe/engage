using C3.Business;
using C3.Data;
using C3.Web.Helpers;
using Phytel.Framework.SQL;
using System;
using System.Web.Security;

namespace C3.Web
{
    public partial class ForgotPassword :BasePage
    {
        
        /// <summary>
        /// UpdatePanel1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.UpdatePanel UpdatePanel1;
        
        /// <summary>
        /// UserNamePanel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel UserNamePanel;
        
        /// <summary>
        /// UserNameLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label UserNameLabel;
        
        /// <summary>
        /// txtUserName control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtUserName;
        
        /// <summary>
        /// UserNameSubmitButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button UserNameSubmitButton;
        
        /// <summary>
        /// UserNameCancelButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button UserNameCancelButton;
        
        /// <summary>
        /// QuestionPanel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel QuestionPanel;
        
        /// <summary>
        /// QuestionLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label QuestionLabel;
        
        /// <summary>
        /// Question control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label Question;
        
        /// <summary>
        /// AnswerLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label AnswerLabel;
        
        /// <summary>
        /// txtAnswer control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtAnswer;
        
        /// <summary>
        /// QuestionSubmitButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button QuestionSubmitButton;
        
        /// <summary>
        /// QuestionCancelButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button QuestionCancelButton;
        
        /// <summary>
        /// InstructionsLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label InstructionsLabel;
    

        #region protected        
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            SetPageProperties();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState.Add("PasswordAnswerCount", passwordAnswerCount);
                UserNamePanel.Style["display"] = "block";
                QuestionPanel.Style["display"] = "none";                

                base.SetPageHeaderText("Forgot your password?");
                base.SetInformationText(ApplicationMessageService.Instance.GetMessage("INF_017"));

                txtUserName.Focus();
            }

        }

        protected void QuestionSubmitButton_OnClick(object sender, EventArgs e)
        {
            bool errorMsg = false;
            muser = Membership.GetUser(txtUserName.Text);
            C3User user = new C3User(muser);
            passwordAnswerCount = user.FailedPasswordAnswerAttemptCount;
            if (txtAnswer.Text.Length == 0)
            {
                errorMsg = true;
            }
            else
            {
                PhytelEncrypter phytelEncrypter = new PhytelEncrypter();
                string passwordAnswerEncrypted = phytelEncrypter.Encrypt(txtAnswer.Text.ToLower());
                
                if (user.PasswordAnswer == passwordAnswerEncrypted)
                {
                    user.ResetPassword();

                    DateTime expiration = System.DateTime.Now.AddMinutes(-1);
                    //string expiration = System.DateTime.Today.ToShortDateString();

                    user.SetPasswordExpiration(expiration.ToString());

                    Membership.ValidateUser(user.UserName, user.GetPassword());                            
                    FormsAuthentication.SetAuthCookie(user.UserName, false);

                    user.ResetFailedAttemptCounts();                           

                    Session.Add("C3User", user);
                    UserPageInfo.ErrorMessageCode = string.Empty;
                    Response.Redirect(GlobalSiteRoot + "ChangePassword.aspx");
                }
                else
                {
                    errorMsg = true;                    
                    passwordAnswerCount += 1;                    
                    user.SetFailedPasswordAnswerAttemptCount(passwordAnswerCount);
                }

            }

            if (errorMsg == true)
                if (passwordAnswerCount == 5)
                {
                    //Lock out user
                    user.LockOutUser();
                    UserPageInfo.ErrorMessageCode = "ERR_009";
                    Response.Redirect(GlobalSiteRoot + "Login.aspx");
                }
                else
                {
                    UserPageInfo.InformationMessageCode = "INF_018";
                    UserPageInfo.ErrorMessageCode = "ERR_004";
                    SetPageErrorText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.ErrorMessageCode));
                    SetInformationText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.InformationMessageCode));
                }

        }

        protected void QuestionCancelButton_OnClick(object sender, EventArgs e)
        {
            UserPageInfo.ErrorMessageCode = "";
            UserPageInfo.InformationMessageCode = "";
            Response.Redirect(GlobalSiteRoot + "Login.aspx");
        }

        protected void UserNameSubmitButton_Click(object sender, EventArgs e)
        {
            bool errorMsg = false;

            if (txtUserName.Text.Length == 0)
                errorMsg = true;
            else
            {
                muser = Membership.GetUser(txtUserName.Text);
                if (muser != null)
                {
                    C3User user = new C3User(muser);
                    if (!user.IsActive)
                    {
                        UserPageInfo.ErrorMessageCode = "ERR_010";
                        UserHelper.RedirectUser(user, "~/Login.aspx", "Login", true);
                    }
                    else if (user.IsDeleted)
                    {
                        UserPageInfo.ErrorMessageCode = "ERR_011";
                        UserHelper.RedirectUser(user, GlobalSiteRoot + "Login.aspx", "Login", true);
                    }
                    else if (user.IsLocked)
                    {
                        UserPageInfo.ErrorMessageCode = "ERR_012";
                        UserHelper.RedirectUser(user, GlobalSiteRoot + "Login.aspx", "Login", true);
                    }
                    else if (string.IsNullOrEmpty(user.PasswordQuestion))
                    {
                        UserPageInfo.ErrorMessageCode = "ERR_008";
                        UserHelper.RedirectUser(user, GlobalSiteRoot + "Login.aspx", "Login", true);
                    }
                    else
                    {
                        Question.Text = muser.PasswordQuestion;

                        UserPageInfo.InformationMessageCode = "INF_018";
                        UserPageInfo.ErrorMessageCode = "";
                        SetInformationText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.InformationMessageCode));
                        SetPageErrorText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.ErrorMessageCode));

                        UserNamePanel.Style["display"] = "none";
                        QuestionPanel.Style["display"] = "block";
                        txtAnswer.Focus();
                    }
                }
                else
                {
                    errorMsg = true;   
                }
            }

            if (errorMsg == true)
            {
                UserPageInfo.ErrorMessageCode = "ERR_002";
                UserPageInfo.InformationMessageCode = "INF_017";
                SetPageErrorText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.ErrorMessageCode));
                SetInformationText(ApplicationMessageService.Instance.GetMessage(UserPageInfo.InformationMessageCode));
            }           

        }

        protected void UserNameCancelButton_OnClick(object sender, EventArgs e)
        {
            UserPageInfo.ErrorMessageCode = "";
            UserPageInfo.InformationMessageCode = "";
            Response.Redirect(GlobalSiteRoot + "Login.aspx");
        }
        #endregion

        #region private     
 
        private Message message = new Message();
        private MembershipUser muser;
        private int passwordAnswerCount = 0;

        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "ForgotPassword";
        }

        #endregion


    }
}
