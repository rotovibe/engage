using C3.Business;
using C3.Data;
using C3.Web.Helpers;
using Phytel.Framework.SQL;
using System;
using System.Web.Security;
using System.Web.UI;

namespace C3.Web
{
    public partial class ChangePassword : BasePage
    {
        
        /// <summary>
        /// pnlChangePassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel pnlChangePassword;
        
        /// <summary>
        /// updChangePassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.UpdatePanel updChangePassword;
        
        /// <summary>
        /// hdnValidationFlag control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField hdnValidationFlag;
        
        /// <summary>
        /// Label1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label Label1;
        
        /// <summary>
        /// divPasswordExpired control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel divPasswordExpired;
        
        /// <summary>
        /// divNewUserPasswordExpired control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel divNewUserPasswordExpired;
        
        /// <summary>
        /// lblMin6Char control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblMin6Char;
        
        /// <summary>
        /// lblOneUpper control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblOneUpper;
        
        /// <summary>
        /// lblOneLower control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblOneLower;
        
        /// <summary>
        /// lblOneNumeric control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblOneNumeric;
        
        /// <summary>
        /// lblOneAlpha control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblOneAlpha;
        
        /// <summary>
        /// lblMatchUserName control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblMatchUserName;
        
        /// <summary>
        /// lblPastHistory control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblPastHistory;
        
        /// <summary>
        /// NewPasswordLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label NewPasswordLabel;
        
        /// <summary>
        /// NewPassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox NewPassword;
        
        /// <summary>
        /// NewPasswordValidator control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label NewPasswordValidator;
        
        /// <summary>
        /// hdnNewPassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField hdnNewPassword;
        
        /// <summary>
        /// ConfirmNewPasswordLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label ConfirmNewPasswordLabel;
        
        /// <summary>
        /// ConfirmNewPassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox ConfirmNewPassword;
        
        /// <summary>
        /// ConfirmPasswordValidator control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label ConfirmPasswordValidator;
        
        /// <summary>
        /// hdnConfirmNewPassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField hdnConfirmNewPassword;
        
        /// <summary>
        /// SubmitButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button SubmitButton;
        
        /// <summary>
        /// CancelButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button CancelButton;
    

        #region Protected

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            SetPageProperties();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((UpdateProgress)BaseMaster.FindControl("UpdateProgress1")).AssociatedUpdatePanelID = updChangePassword.UniqueID;
        }

        protected override void OnPreRender(EventArgs e)
        {
            NewPassword.Focus();

            //try
            //{
            //    Label headerLabel = (Label)BaseMaster.FindControl("formHeaderText");
            //    ((HtmlControl)BaseMaster.FindControl("clientCareEmailLink")).Visible = false;
            //    headerLabel.Text = "Change Password";
            //    headerLabel.CssClass = "label-style-local";
            //}
            //catch { }

            UpdatePanel masterPannel = (UpdatePanel)BaseMaster.FindControl("MasterUpdatePanel");
            masterPannel.Visible = false;

            //UserPageInfo.InformationMessageCode = "INF_019";
            //SetInformationText(ApplicationMessageService.Instance.GetMessage("INF_019"),string.Empty);

            base.OnPreRender(e);
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            
            CancelButton.Visible = UserPageInfo.LastPage != "/ForgotPassword.aspx";
            
            if (CurrentUser.FirstTimeUser)
                divNewUserPasswordExpired.Visible = CurrentUser.FirstTimeUser;
            else if (CurrentUser.PasswordIsExpired)
                divPasswordExpired.Visible = CurrentUser.PasswordIsExpired && CancelButton.Visible;
        }

        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            ApplicationMessage errMessage = null;
            string newPasswordEncrypted = string.Empty;

            if (String.IsNullOrEmpty(NewPassword.Text) || String.IsNullOrEmpty(ConfirmNewPassword.Text))
                errMessage = ApplicationMessageService.Instance.GetMessage("ERR_032");
            else if (ConfirmNewPassword.Text != NewPassword.Text)
                errMessage = ApplicationMessageService.Instance.GetMessage("ERR_005");

            NewPasswordValidator.Visible = String.IsNullOrEmpty(NewPassword.Text);
            ConfirmPasswordValidator.Visible = String.IsNullOrEmpty(ConfirmNewPassword.Text);

            if (errMessage == null)
            {
                if (!ValidateNewPassword())
                {
                    errMessage = ApplicationMessageService.Instance.GetMessage("ERR_006");
                    hdnNewPassword.Value = NewPassword.Text;
                    hdnConfirmNewPassword.Value = ConfirmNewPassword.Text;
                }
            }

            if (errMessage == null)
            {
                // Attempt to Change Password
                CurrentUser.ChangePassword(CurrentUser.CurrentPassword, NewPassword.Text);
                CurrentUser.SetPasswordExpiration();

                //Log Audit for Change Password
                LogAuditEvent("ChangePassword", null);

                // Store new password in the Password History table
                CurrentUser.AddToPasswordHistory(newPasswordEncrypted);

                //send email
                if (string.IsNullOrEmpty(CurrentUser.Email))
                {
                    MembershipUser mUser = Membership.GetUser(CurrentUser.AdminUserId);
                    if (mUser != null)
                    {
                        C3User adminUser = new C3User(mUser);
                        if (!string.IsNullOrEmpty(adminUser.Email))
                            UserHelper.SendEmail(CurrentUser, ApplicationSettingService.Instance.GetSetting("EMAIL_CHGPWD_ADMIN_SUBJECT").Value, ApplicationSettingService.Instance.GetSetting("EMAIL_CHGPWD_ADMIN_BODY").Value, adminUser);
                    }
                }
                else
                    UserHelper.SendEmail(CurrentUser, ApplicationSettingService.Instance.GetSetting("EMAIL_CHGPWD_USER_SUBJECT").Value, ApplicationSettingService.Instance.GetSetting("EMAIL_CHGPWD_USER_BODY").Value);

                if (CurrentUser.FirstTimeUser)
                {
                    //Set FirstTimeUser to false
                    CurrentUser.SetFirstTimeUser();
                }

                UserPageInfo.ErrorMessageCode = "";
                UserPageInfo.InformationMessageCode = "";
                UserHelper.RedirectUser(CurrentUser);
            }
            else
            {
                //SetPageErrorText(errMessage);
            }
        }

        private bool ValidateNewPassword()
        {
            bool newPasswordValid = true;
            bool confirmNewPasswordValid = true;
            bool passwordHistryNotExists = true;

            string newPasswordEncrypted = string.Empty;

            // New Password Validation
            if (NewPassword.Text.Trim().Length < 6) { newPasswordValid = false; }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(NewPassword.Text, "[A-Z]"))) { newPasswordValid = false; }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(NewPassword.Text, "[a-z]"))) { newPasswordValid = false; }
            if (!System.Text.RegularExpressions.Regex.IsMatch(NewPassword.Text, "\\d")) { newPasswordValid = false; }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(NewPassword.Text, "[^0-9a-zA-Z]"))) { newPasswordValid = false; }
            if (System.Text.RegularExpressions.Regex.IsMatch(NewPassword.Text.ToLower(), CurrentUser.UserName.ToLower())) { newPasswordValid = false; }

            // Confirm New Password Validation
            if (ConfirmNewPassword.Text.Trim().Length < 6) { confirmNewPasswordValid = false; }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(ConfirmNewPassword.Text, "[A-Z]"))) { confirmNewPasswordValid = false; }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(ConfirmNewPassword.Text, "[a-z]"))) { confirmNewPasswordValid = false; }
            if (!System.Text.RegularExpressions.Regex.IsMatch(ConfirmNewPassword.Text, "\\d")) { confirmNewPasswordValid = false; }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(ConfirmNewPassword.Text, "[^0-9a-zA-Z]"))) { confirmNewPasswordValid = false; }
            if (System.Text.RegularExpressions.Regex.IsMatch(ConfirmNewPassword.Text.ToLower(), CurrentUser.UserName.ToLower())) { confirmNewPasswordValid = false; }

            // Password Histry Validation
            PhytelEncrypter phytelEncrypter = new PhytelEncrypter();
            newPasswordEncrypted = phytelEncrypter.Encrypt(NewPassword.Text);
            PasswordHistory pwdHistory = CurrentUser.HistoricalPasswords.Find(delegate(PasswordHistory a) { return a.Password == newPasswordEncrypted; });

            if (pwdHistory != null || CurrentUser.GetPassword().Equals(NewPassword.Text)) { passwordHistryNotExists = false; }

            if (!newPasswordValid)
            {
                hdnValidationFlag.Value = "NewPasswordMinReq";
                return newPasswordValid;
            }
            else if (!confirmNewPasswordValid)
            {
                hdnValidationFlag.Value = "ConfirmNewPasswordMinReq";
                return confirmNewPasswordValid;
            }
            else if (!passwordHistryNotExists)
            {
                hdnValidationFlag.Value = "HistryExists";
                return passwordHistryNotExists;
            }

            return true;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (CurrentUser.FirstTimeUser || CurrentUser.PasswordIsExpired)
            {
                if (CurrentUser.FirstTimeUser)
                    UserPageInfo.ErrorMessageCode = "ERR_031";
                else if (CurrentUser.PasswordIsExpired)
                    UserPageInfo.ErrorMessageCode = "ERR_052";

                UserPageInfo.InformationMessageCode = "";
                UserHelper.ClearUserSessionVariables(true);
                System.Web.Security.FormsAuthentication.SignOut();
                UserHelper.RedirectUser(CurrentUser, "Login.aspx", "Login", true);
            }
            else
            {
                UserPageInfo.ErrorMessageCode = "";
                UserPageInfo.InformationMessageCode = "";
                UserHelper.RedirectUser(CurrentUser, CurrentUser.UserPageInfo.LastPage, CurrentUser.UserPageInfo.LastPageName, true);
            }
        }

        #endregion

        #region Private

        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "ChangePassword";
        }

        #endregion
    }
}
