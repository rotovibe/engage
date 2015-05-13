using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace C3.WebControls
{
    public class AjaxValidationSummary : ValidationSummary
    {
        protected override void OnPreRender(EventArgs e)
        {
            //this is to correct a known bug in the ajax control toolkit. 
            //http://ajaxcontroltoolkit.codeplex.com/workitem/27024

            base.OnPreRender(e);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.ClientID, ";", true);
        }
    }

}
