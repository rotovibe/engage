using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace C3.Web
{
    public partial class AccessDenied : BasePage
    {
        #region Protected
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            SetPageProperties();
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Page"] != null)
            {
                Session.Add("Page", Request.QueryString["Page"].ToString());
            }
            else
            {
                Session.Add("Page", "");
            }
            SetPageHeaderText("Access Denied");

            SetInformationText(null, string.Format("Please contact your administrator to gain access to <span>{0}</span> page.", Session["Page"].ToString()));
        }

        #endregion

        #region Private
        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "AccessDenied";
        }

        #endregion
    }
}
