using C3.Business;
using C3.Web.Helpers;
using Phytel.Framework.SQL;
using System;
using System.Collections.Generic;

namespace C3.Web
{
    public partial class SecurityQuestion : BasePage
    {
        
        /// <summary>
        /// Panel1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel Panel1;
        
        /// <summary>
        /// SecurityQuestionListLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label SecurityQuestionListLabel;
        
        /// <summary>
        /// SecurityQuestionList control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::C3.WebControls.MultiDropDown SecurityQuestionList;
        
        /// <summary>
        /// SecurityQuestionAnswerLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label SecurityQuestionAnswerLabel;
        
        /// <summary>
        /// SecurityQuestionAnswer control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox SecurityQuestionAnswer;
        
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
        
        /// <summary>
        /// instructions control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label instructions;
    

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
                List<Data.SecurityQuestion> securityQuestions = new SecurityQuestionService().GetAll();

                SecurityQuestionList.DataSource = ListConverter.ToDataTable(securityQuestions);
                SecurityQuestionList.DataBind();

                if (CurrentUser != null)
                {
                    if (!String.IsNullOrEmpty(CurrentUser.PasswordQuestion))
                    {
                        if (securityQuestions.Exists(delegate(Data.SecurityQuestion a) { return a.Question == CurrentUser.PasswordQuestion; }))
                        {
                            Data.SecurityQuestion selectedQuestion = new Data.SecurityQuestion();
                            selectedQuestion = securityQuestions.Find(delegate(Data.SecurityQuestion a) { return a.Question == CurrentUser.PasswordQuestion; });
                            string[] secQuestion = {selectedQuestion.Question};
                            SecurityQuestionList.SetSelectedItems(secQuestion);
                                                        
                        }
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetPageHeaderText("Security Question");
            UserPageInfo.InformationMessageCode = "INF_016";
            SetInformationText(ApplicationMessageService.Instance.GetMessage("INF_016"));
            instructions.Text = ApplicationMessageService.Instance.GetMessage("INF_025").Text;

            SecurityQuestionList.Focus();

            base.OnPreRender(e);
        }
        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            bool errorMsg = false;
            if (SecurityQuestionAnswer.Text.Length == 0 || SecurityQuestionAnswer.Text.Trim().Length == 0 || SecurityQuestionAnswer.Text.Length >= 250)
            {
                errorMsg = true;
            }
          

            if (!errorMsg)
            {

                PhytelEncrypter phytelEncrypter = new PhytelEncrypter();
                string securityAnswerEncrypted = phytelEncrypter.Encrypt(SecurityQuestionAnswer.Text.ToLower());

                string[] selectedQuestionId = SecurityQuestionList.GetSelectedItems();
                string selectedQuestionText = selectedQuestionId[0];

                CurrentUser.SetSecurityQuestion(selectedQuestionText, securityAnswerEncrypted);
                //Log audit of Security Question Change                
                LogAuditEvent("SecurityQuestionChange", null);

                UserHelper.RedirectUser(CurrentUser, true);
            }
            else
            {
                UserPageInfo.ErrorMessageCode = "ERR_007";
                SetPageErrorText(ApplicationMessageService.Instance.GetMessage("ERR_007"));
            }
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentUser.PasswordQuestion))
            {
                UserPageInfo.ErrorMessageCode = "ERR_034";
                
                UserHelper.ClearUserSessionVariables(true);
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect(GlobalSiteRoot + "Login.aspx");
            }
            else
            {
                UserPageInfo.InformationMessageCode = "";
                UserPageInfo.ErrorMessageCode = "";
                UserHelper.RedirectUser(CurrentUser, CurrentUser.UserPageInfo.LastPage, CurrentUser.UserPageInfo.LastPageName, true);
            }
        }

        #endregion

        #region private

        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "SecurityQuestion";
        }

        #endregion


    }
}
