using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using C3.Business;
using C3.Data;

namespace C3.Web.Controls
{
    public partial class Footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            ApplicationMessage msg = ApplicationMessageService.Instance.GetMessage("APP_DISC");

            //TFS 15873 (TGindrup 10.25.2012):  Put a token in the copyright Title to replace the end year by
            this.footerTitle.Text = msg.Title.Replace(@"<YYYY>", DateTime.Now.Year.ToString());

            if (Session["C3User"] != null)
                this.footerText.Text = msg.Text;
            else
                this.footerText.Text = string.Empty;

            base.OnPreRender(e);
        }
    }
}