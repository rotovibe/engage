using System;
using System.Data;
using Phytel.Framework.SQL.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace C3.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MultiDropDown runat=server></{0}:MultiDropDown>")]
    public class MultiDropDown : WebControl
    {
        //private const string WEBCONTROLS_JS = "C3.WebControls.WebControls.js"; //This is needed for some styling issues with jquery controls
        private const string WEBCONTROLS_JS = "Atmosphere.WebControls.WebControls.js"; 

        #region Public Enum
        public enum MultiSelectTypes
        {
            MultiSelect,
            SingleSelect,
            SingleSelectWithFilter
        }
        #endregion

        #region Private Variables
        private string[] _selectedItems = new string[] { };
        private bool _usePageRequest = true;
        private bool _processRendering = true;
        #endregion 

        #region Properties

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("auto")]
        [Localizable(true)]
        public string MinWidth
        {
            get
            {
                try
                {
                    String s = (String)ViewState["MinWidth"];
                    return ((s == null) ? "auto" : s);
                }
                catch { return "auto"; }
            }

            set
            {
                ViewState["MinWidth"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                try
                {
                    String s = (String)ViewState["Text"];
                    return ((s == null) ? String.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string AltFieldName
        {
            get
            {
                try
                {
                    String s = (String)ViewState["AltFieldName"];
                    return ((s == null) ? String.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["AltFieldName"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NoneSelectedText
        {
            get
            {
                try
                {
                    String s = (String)ViewState["NoneSelectedText"];
                    return ((s == null) ? String.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["NoneSelectedText"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string DisplayMember
        {
            get
            {
                try
                {
                    String s = (String)ViewState["DisplayMember"];
                    return ((s == null) ? String.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["DisplayMember"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string ValueMember
        {
            get
            {
                try
                {
                    String s = (String)ViewState["ValueMember"];
                    return ((s == null) ? String.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["ValueMember"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SelectedMember
        {
            get
            {
                try
                {
                    String s = (String)ViewState["SelectedMember"];
                    return ((s == null) ? String.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["SelectedMember"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SelectedListCount
        {
            get
            {
                try
                {
                    string s = (string)ViewState["SelectedListCount"];
                    return ((s == null) ? "1" : s);
                }
                catch { return "1"; }
            }

            set
            {
                ViewState["SelectedListCount"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Classes
        {
            get
            {
                try
                {
                    string s = (string)ViewState["Classes"];
                    return ((s == null) ? string.Empty : s);
                }
                catch { return string.Empty; }
            }

            set
            {
                ViewState["Classes"] = value;
            }
        }
        
        public DataTable DataSource
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["DataSource"];
                }
                catch { return null; }
            }
            set
            {
                ViewState["DataSource"] = value;
                
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public MultiSelectTypes MultiSelectType
        {
            get
            {
                try
                {
                    MultiSelectTypes s = (MultiSelectTypes)ViewState["MultiSelectType"];
                    return ((s == null) ? MultiSelectTypes.MultiSelect : s);
                }
                catch { return MultiSelectTypes.MultiSelect; }
            }

            set
            {
                ViewState["MultiSelectType"] = value;
            }
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool MatchOnDisplayMember
        {
            get
            {
                try
                {
                    bool s = (bool)ViewState["MatchOnDisplayMember"];
                    return ((s == null) ? false : s);
                }
                catch { return false; }
            }

            set
            {
                ViewState["MatchOnDisplayMember"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("left top")]
        [Localizable(true)]
        public string PositionMy
        {
            get
            {
                try
                {
                    string s = (string)ViewState["PositionMy"];
                    return ((s == null) ? "left top" : s);
                }
                catch { return "left top"; }
            }

            set
            {
                ViewState["PositionMy"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("left bottom")]
        [Localizable(true)]
        public string PositionAt
        {
            get
            {
                try
                {
                    string s = (string)ViewState["PositionAt"];
                    return ((s == null) ? "left bottom" : s);
                }
                catch { return "left bottom"; }
            }

            set
            {
                ViewState["PositionAt"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(0)]
        [Localizable(true)]
        public int DisplayMaxLength
        {
            get
            {
                try
                {
                    int s = (int)ViewState["DisplayMaxLength"];
                    return ((s == null) ? 0 : s);
                }
                catch { return 0; }
            }

            set
            {
                ViewState["DisplayMaxLength"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(0)]
        [Localizable(true)]
        public int MenuHeight
        {
            get
            {
                try
                {
                    int s = (int)ViewState["MenuHeight"];
                    return ((s == null) ? 0 : s);
                }
                catch { return 0; }
            }

            set
            {
                ViewState["MenuHeight"] = value;
            }
        }

        #endregion

        #region Public Methods
        public string[] GetSelectedItems()
        {
            string returnItems = string.Empty;

            try
            {
                returnItems = (string)this.Page.Request[string.Format("{0}[]", this.ID)];
            }
            catch (Exception) { }
            if (returnItems != null)
                _selectedItems = returnItems.Split(",".ToCharArray());

                if (_selectedItems.Length == 1 && _selectedItems[0].Trim() == string.Empty)
                    _selectedItems = new string[] { };
            return _selectedItems;
        }

        public void SetSelectedItems(string[] items)
        {
            _selectedItems = items;
            _usePageRequest = true;
        }

        public void SetSelectedItems(string[] items, bool usePageRequest)
        {
            _selectedItems = items;
            _usePageRequest = usePageRequest;
        }

        public void SetRenderingStatus(bool allowRendering)
        {
            _processRendering = allowRendering;
        }
        #endregion

        #region Protected Methods

        protected override void RenderContents(HtmlTextWriter writer)
        {
            string ddData = string.Empty;

            if (_processRendering)
            {
                if (_usePageRequest)
                    this.GetSelectedItems();

                switch (this.MultiSelectType)
                {
                    case MultiSelectTypes.MultiSelect:
                        ddData = "<div runat='server' id='{0}Div' style='display: inline;'>" +
                            "<select id='{1}' name='{2}[]' {3} multiple='multiple' class='filter' size='1' runat='server'>{4}</select></div>";
                        break;

                    case MultiSelectTypes.SingleSelect:
                        ddData = "<div runat='server' id='{0}Div' style='display: inline;'>" +
                            "<select id='{1}' name='{2}[]' {3} size='1'>{4}</select></div>";
                        break;

                    case MultiSelectTypes.SingleSelectWithFilter:
                        ddData = "<div runat='server' id='{0}Div' style='display: inline;'>" +
                            "<select id='{1}' name='{2}[]' {3} class='filter' size='1' runat='server'>{4}</select></div>";
                        break;
                }

                string ddOptions = string.Empty;

                try
                {
                    if (this.DataSource != null)
                    {
                        int maxLength = DisplayMaxLength;

                        List<string> addedItems = new List<string>();
                        foreach (DataRow dr in this.DataSource.Rows)
                        {
                            if (!addedItems.Contains(dr[ValueMember].ToString()))
                            {
                                string matchValue = (MatchOnDisplayMember ? dr[DisplayMember].ToString() : dr[ValueMember].ToString());
                                string displayValue = dr[DisplayMember].ToString();

                                if (maxLength > 0 && displayValue.Length > maxLength)
                                    displayValue = displayValue.Substring(0, maxLength);

                                if (displayValue != string.Empty)
                                {
                                    if (_selectedItems.Contains(matchValue))
                                        ddOptions += string.Format("<option value='{0}' selected>{1}</option>",
                                            dr[ValueMember].ToString(), displayValue);
                                    else if (SelectedMember != string.Empty)
                                        ddOptions += string.Format("<option value='{0}' {1}>{2}</option>",
                                            dr[ValueMember].ToString(), dr[SelectedMember].ToString(), displayValue);
                                    else
                                        ddOptions += string.Format("<option value='{0}'>{1}</option>",
                                            dr[ValueMember].ToString(), displayValue);

                                    addedItems.Add(dr[ValueMember].ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    ddOptions = string.Format("<option value=''>No {0} Available</option>", this.NoneSelectedText);
                }

                ddData = string.Format(ddData, this.ID, this.ID, this.ID, (this.Enabled ? String.Empty : "disabled='disabled'"), ddOptions);

                switch (this.MultiSelectType)
                {
                    case MultiSelectTypes.SingleSelect:
                        writer.WriteLine("<script type='text/javascript'>");
                        writer.WriteLine("$(function(){");
                        writer.WriteLine(string.Format("$('#{0}').multiselect(", this.ID));
                        writer.WriteLine("{");
                        writer.WriteLine("multiple: false,");
                        writer.WriteLine(string.Format("minWidth: '{0}',", MinWidth));
                        writer.WriteLine("header: false,");
                        writer.WriteLine("collision: 'flip',");

                        if (MenuHeight > 0)
                            writer.WriteLine(string.Format("height: '{0}',", MenuHeight));

                        writer.WriteLine("position: {");
                        writer.WriteLine(string.Format("my: '{0}', at: '{1}'", PositionMy, PositionAt));
                        writer.WriteLine("},");
                        writer.WriteLine(string.Format("noneSelectedText: '{0}',", NoneSelectedText));
                        writer.WriteLine(string.Format("classes: '{0}',", Classes));
                        writer.WriteLine("selectedList: 1");
                        writer.WriteLine("});");
                        writer.WriteLine("});");
                        writer.WriteLine("</script>");
                        break;

                    case MultiSelectTypes.SingleSelectWithFilter:
                        writer.WriteLine("<script type='text/javascript'>");
                        writer.WriteLine("$(document).ready(function ()");
                        writer.WriteLine("{");
                        writer.WriteLine(string.Format("$('#{0}', document)", this.ID));
                        writer.WriteLine(".multiselect({");
                        writer.WriteLine("multiple: false,");
                        writer.WriteLine(string.Format("minWidth: '{0}',", MinWidth));
                        writer.WriteLine("collision: 'flip',");

                        if (MenuHeight > 0)
                            writer.WriteLine(string.Format("height: '{0}',", MenuHeight));

                        writer.WriteLine("position: {");
                        writer.WriteLine(string.Format("my: '{0}', at: '{1}'", PositionMy, PositionAt));
                        writer.WriteLine("},");
                        writer.WriteLine(string.Format("noneSelectedText: 'Select {0}&hellip;',", NoneSelectedText));
                        writer.WriteLine(string.Format("classes: '{0}',", Classes));
                        writer.WriteLine(string.Format("selectedText: '# {0}',", NoneSelectedText));
                        writer.WriteLine(string.Format("selectedList: {0}", SelectedListCount));
                        writer.WriteLine("})");
                        writer.WriteLine(".multiselectfilter();})");
                        writer.WriteLine("</script>");
                        break;

                    case MultiSelectTypes.MultiSelect:
                        writer.WriteLine("<script type='text/javascript'>");
                        writer.WriteLine("$(document).ready(function ()");
                        writer.WriteLine("{");
                        writer.WriteLine(string.Format("$('#{0}', document)", this.ID));
                        writer.WriteLine(".multiselect({");
                        writer.WriteLine(string.Format("minWidth: '{0}',", MinWidth));
                        writer.WriteLine("collision: 'flip',");

                        if (MenuHeight > 0)
                            writer.WriteLine(string.Format("height: '{0}',", MenuHeight));

                        writer.WriteLine("position: {");
                        writer.WriteLine(string.Format("my: '{0}', at: '{1}'", PositionMy, PositionAt));
                        writer.WriteLine("},");
                        writer.WriteLine(string.Format("noneSelectedText: 'Select {0}&hellip;',", NoneSelectedText));
                        writer.WriteLine(string.Format("classes: '{0}',", Classes));
                        writer.WriteLine(string.Format("selectedText: '# {0}',", NoneSelectedText));
                        writer.WriteLine(string.Format("selectedList: {0}", SelectedListCount));
                        writer.WriteLine("})");
                        writer.WriteLine(".multiselectfilter();})");
                        writer.WriteLine("</script>");
                        break;
                }
                writer.WriteLine(ddData);

                if (AltFieldName != string.Empty)
                {
                    writer.WriteLine("<script type='text/javascript'>");
                    writer.WriteLine(string.Format("$('#{0}')", this.ID));
                    writer.WriteLine(".multiselect({");
                    writer.WriteLine("beforeclose: function () {");
                    writer.WriteLine("var items = $('#{0}').multiselect('getChecked').map(function () {1}).get();", this.ID, "{ return this.value; }");
                    writer.WriteLine(string.Format("$('#{0}').val(items);", AltFieldName));
                    writer.WriteLine("}");
                    writer.WriteLine("});");
                    writer.WriteLine("</script>");
                }
                base.RenderContents(writer);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Type t = this.GetType();
            string url = Page.ClientScript.GetWebResourceUrl(t, WEBCONTROLS_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, WEBCONTROLS_JS))
                Page.ClientScript.RegisterClientScriptInclude(t, WEBCONTROLS_JS, url);
            
            base.OnLoad(e);
        }
        #endregion
    }
}

