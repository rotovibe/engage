using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using C3.Web.Helpers;
using C3.Business;

namespace C3.Web
{
    public partial class ErrorMessage : BasePage
    {
    

        #region Protected
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            SetPageProperties();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string _error = "";
            if (CurrentUser.UserPageInfo.LastSystemErrorMessage != null)
            {
                _error = CurrentUser.UserPageInfo.LastSystemErrorMessage.ToString();
            }
            else
            {
                _error = ApplicationMessageService.Instance.GetMessage("ERR_026").Text;
            }

            ViewState.Add("Error", _error);
            SetPageHeaderText("Error Message");
        }

        #endregion

        #region Private
        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "ErrorMessage";
        }

        #endregion
    }
}
