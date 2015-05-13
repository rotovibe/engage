using System;
using System.Web.UI;
using C3.Web.Helpers;

namespace Atmosphere.Support
{
    public partial class SupportFormPage : Page
    {
        public string EmailValue { get; set; }
        public string CompanyNameValue { get; set; }
        public string PhytelUsernameValue { get; set; }
        public string DisplayNameValue { get; set; }
        public string PhoneValue { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = (C3User)Session["C3User"];
            if (user != null)
            {
                this.EmailValue = user.Email;
                this.CompanyNameValue = user.CurrentContract.Name;
                this.PhytelUsernameValue = user.UserName;
                this.DisplayNameValue = user.DisplayName;
                this.PhoneValue = user.PhoneExt;
            }
        }

        protected string SupportConfirmationUrl
        {
            get
            {
                string url = Request.Url.ToString();
                return url.Substring(0, url.LastIndexOf(@"/")) + "/SupportFormConfirmation.aspx";
            }
        }
    }

    public partial class SupportFormPage
    {

        /// <summary>
        /// btnSupportFormCancel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnSupportFormCancel;

        /// <summary>
        /// btnSupportFormSubmit control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnSupportFormSubmit;
    }
}