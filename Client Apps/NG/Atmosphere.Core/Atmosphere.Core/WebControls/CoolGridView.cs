//------------------------------------------------------------------------------
// IdeaSparx.CoolControls.Web
// Author: John Eric Sobrepena (2009)
// You can use these codes in whole or in parts without warranty.
// By using any part of this code, you agree 
// to keep this information about the author intact.
// http://johnsobrepena.blogspot.com
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace C3.WebControls
{
    [ToolboxData("<{0}:CoolGridView Width=\"400px\" Height=\"300px\"></{0}:CoolGridView>")]
    public partial class CoolGridView : System.Web.UI.WebControls.GridView
    {
        private const string WEBCONTROLS_JS = "Atmosphere.WebControls.WebControls.js";
        private const string SELECTEDITEMS = "gridSelectedItems";

        private const string Suffix = "jEsCoOl";
        private CoolGridViewRow PagerRow = null;
        private string _HiddenFieldDataValue = String.Empty;

        private string _ObjectIdControl;
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        public string ObjectIdControl
        {
            get { return _ObjectIdControl; }
            set { _ObjectIdControl = value; }
        }

        private bool _FixHeaders = false;
        [Browsable(true), Category("Behavior"), DefaultValue(false)]
        public bool FixHeaders
        {
            get { return _FixHeaders; }
            set { _FixHeaders = value; }
        }

        private bool _AllowResizeColumn = false;
        /// <summary>
        /// Get or set if user can resize the column.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false)]
        public bool AllowResizeColumn
        {
            get { return _AllowResizeColumn; }
            set { _AllowResizeColumn = value; }
        }

        private string _PagerPassThruField;
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        public string PagerPassThruField
        {
            get { return _PagerPassThruField; }
            set { _PagerPassThruField = value; }
        }

        private string _PagerPassThruButton;
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        public string PagerPassThruButton
        {
            get { return _PagerPassThruButton; }
            set { _PagerPassThruButton = value; }
        }

        private Unit _DefaultColumnWidth = new Unit(100, UnitType.Pixel);
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Unit), "100px")]
        public Unit DefaultColumnWidth
        {
            get { return _DefaultColumnWidth; }
            set
            {
                if (value == Unit.Empty)
                    throw new ArgumentException("DefaultColumnWidth cannot be empty.");
                if (value.Type != UnitType.Pixel)
                    throw new ArgumentException("DefaultColumnWidth can only be of type Pixel");
                _DefaultColumnWidth = value;
            }
        }

        #region Parsers
        public List<Unit> ParseColumnWidthsFromJson(string JsonString)
        {
            List<Unit> columnWidths = new List<Unit>();
            if (!String.IsNullOrEmpty(JsonString))
            {

                RegexOptions options = (RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Regex rx = new Regex("ColumnWidths\\s*:\\s*[[](?:([0-9.]+) || [,\\s]*)*[]]", options);

                MatchCollection mc = rx.Matches(JsonString);
                if (mc.Count > 0 && mc[0].Groups.Count > 1)
                    foreach (Capture c in mc[0].Groups[1].Captures)
                    {
                        if (c.Value.EndsWith("%"))
                            columnWidths.Add(Unit.Parse(c.Value));
                        else
                            columnWidths.Add(Unit.Parse(c.Value + "px"));
                    }
            }

            return columnWidths;
        }
        #endregion

        //Constructor
        public CoolGridView()
        {
            _BoundaryStyle.BorderColor = Color.Gray;
            _BoundaryStyle.BorderWidth = new Unit(1, UnitType.Pixel);
            _BoundaryStyle.BorderStyle = BorderStyle.Solid;
        }

        public GridViewRow AddRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            return CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
        }

        protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            if (rowType == DataControlRowType.Header
                                || rowType == DataControlRowType.Footer
                                || rowType == DataControlRowType.Pager
                                )
            {
                CoolGridViewRow c = new CoolGridViewRow(rowIndex, dataSourceIndex, rowType, rowState);
                c.ParentCoolGridView = this;
                if (rowType == DataControlRowType.Pager) PagerRow = c;
                return c;
            }
            else if (rowIndex == 0)
            {
                CoolGridViewFirstRow c = new CoolGridViewFirstRow(rowIndex, dataSourceIndex, rowType, rowState);
                c.ParentCoolGridView = this;
                return c;
            }
            else
            {
                return base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
            }
        }

        private Unit _Width = Unit.Empty;
        [DefaultValue(typeof(Unit), ""), Category("Layout")]
        public override Unit Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private Unit _Height = Unit.Empty;
        [DefaultValue(typeof(Unit), ""), Category("Layout")]
        public override Unit Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        private new bool EnableSortingAndPagingCallbacks
        {
            get { return false; }
            set { }
        }

        private Style _BoundaryStyle = new Style();
        [PersistenceMode(PersistenceMode.InnerProperty), Description("Boundary Style"), NotifyParentProperty(true),
        Category("Styles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true)]
        public Style BoundaryStyle
        {
            get { return _BoundaryStyle; }
        }

        private string GridContainerID { get { return ClientID + Suffix + "_mainDiv"; } }
        private string HeaderContainerID { get { return ClientID + Suffix + "_headerDiv"; } }
        private string FooterContainerID { get { return ClientID + Suffix + "_footerDiv"; } }
        private string TableContainerID { get { return ClientID + Suffix + "_tableDiv"; } }
        private string PagerContainerID { get { return ClientID + Suffix + "_pagerDiv"; } }
        private string HiddenFieldDataID { get { return ClientID + Suffix + "_data"; } }

        protected override void OnPreRender(EventArgs e)
        {
            //            //TODO: Blog this
            //            //Updated for AjaxControlToolkit compatibility
            //            string jsInitialize = @"var var{3} = new CoolGridView({{GridContainerID: ""{0}"",HeaderContainerID: ""{1}"",TableContainerID: ""{2}"",GridID: ""{3}"",FooterContainerID: ""{4}"",PagerContainerID: ""{5}""}});
            ////this is for AjaxControlToolkit compatibility
            //try{{Sys.WebForms.PageRequestManager.getInstance().add_endRequest(var{3}.GetAjaxToolkitEndRequestHander());}} catch(e){{}}";

            //            jsInitialize = String.Format(jsInitialize, GridContainerID, HeaderContainerID, TableContainerID, ClientID, FooterContainerID, PagerContainerID);
            //            Page.ClientScript.RegisterStartupScript(this.GetType(), "Initialize" + ClientID, jsInitialize, true);


            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Registering our JavaScript File and embedding it into whatever page is using it.
            Type t = this.GetType();
            string url = Page.ClientScript.GetWebResourceUrl(t, WEBCONTROLS_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, WEBCONTROLS_JS))
                Page.ClientScript.RegisterClientScriptInclude(t, WEBCONTROLS_JS, url);

            //Register the DIV resize column vertical line
            if (!Page.ClientScript.IsStartupScriptRegistered(this.GetType(), "lLKAopspo28lOANcaju9182ia92u"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "lLKAopspo28lOANcaju9182ia92u", "<div id=\"lLKAopspo28lOANcaju9182ia92u\" style=\"display:none;border:solid 1px gray; background-color:#E5E5E5; width:100px; height:100px; top:0px; left:0px; position:absolute;\" ></div>", false);
            }

            //Register the DIV resize column vertical line
            if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "CoolGridView.Style"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CoolGridView.Style",
@"<style>
	.CoolGridViewTable TR TD, .CoolGridViewTable TR TH, .CoolGridViewTable TR TH SPAN, .CoolGridViewTable TR TD SPAN
	{
		overflow: hidden;
		text-overflow : ellipsis;
	}
	.CoolGridViewTable TR TH SPAN, .CoolGridViewTable TR TD SPAN
	{
		margin : 0 0 0 0;
		padding : 0 0 0 0;
	}
</style>", false);
            }

            //Check control state from hidden field
            _HiddenFieldDataValue = Page.Request.Form[HiddenFieldDataID];
            //Load column widths from current UI state
            //Initializes the column widths base on current UI state.
            List<Unit> columnWidths = ParseColumnWidthsFromJson(_HiddenFieldDataValue);
            int cI = 0;
            foreach (DataControlField fld in Columns)
            {
                if (fld.Visible && cI < columnWidths.Count)
                    fld.HeaderStyle.Width = columnWidths[cI++];
            }

            if (PagerPassThruField != null && PagerPassThruButton != null)
            {

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "PagerPassThru"))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<script type=\"text/javascript\"> function paging(page) {");
                    builder.Append(string.Format("var text = document.getElementById('{0}');", PagerPassThruField));
                    builder.Append("text.value = page;");
                    builder.Append(string.Format("var button = document.getElementById('{0}');", PagerPassThruButton));
                    builder.Append("button.click();} </");
                    builder.Append("script>");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PagerPassThru", builder.ToString());
                }
            }
        }

        private void AddPropertiesToRenderForGridContainer(HtmlTextWriter writer)
        {
            if (Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
            if (Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
            //TODO:Integrate this line with main trunk
            writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "hidden");
            BoundaryStyle.AddAttributesToRender(writer);
        }

        private string ToTableRulesValue(GridLines GridLines)
        {
            switch (GridLines)
            {
                case GridLines.Both: return "all";
                case GridLines.Horizontal: return "rows";
                case GridLines.Vertical: return "cols";
                case GridLines.None:
                default: return "none";
            }
        }

        private void RenderAttributesForTable(HtmlTextWriter writer)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, Style.Value);
            //writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, CellSpacing.ToString());
            if (GridLines != GridLines.None)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Border, (BorderWidth == Unit.Empty ? "1" : BorderWidth.Value.ToString()));
                //writer.AddAttribute(HtmlTextWriterAttribute.Rules, ToTableRulesValue(GridLines));
            }
            //if (BorderWidth != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, BorderWidth.ToString());
            ControlStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute("table-layout", "fixed");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-table");
        }

        private void RenderHeader(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                if (Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "30px");
                if (Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, HeaderContainerID);
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowX, "hidden");
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "visible");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, (GetCorrectedWidth().Value + 100).ToString() + "px");
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "none");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            RenderAttributesForTable(writer);
            //TODO: Have to emit header specific Style
            //HeaderStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            if (HeaderRow != null && HeaderRow is CoolGridViewRow)
            {
                ((CoolGridViewRow)HeaderRow).RenderColGroup(writer);
                ((CoolGridViewRow)HeaderRow).RenderRow(writer);
            }

            writer.RenderEndTag();//Table
            writer.RenderEndTag();//Div
            writer.RenderEndTag();//Div
        }

        private void RenderFooter(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                if (Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "30px");
                if (Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, FooterContainerID);
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowX, "hidden");
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "visible");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, (GetCorrectedWidth().Value + 100).ToString() + "px");
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "none");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            RenderAttributesForTable(writer);
            //TODO: Have to emit header specific Style
            //FooterStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            if (FooterRow != null && FooterRow is CoolGridViewRow)
            {
                ((CoolGridViewRow)FooterRow).RenderColGroup(writer);
                ((CoolGridViewRow)FooterRow).RenderRow(writer);
            }

            writer.RenderEndTag();//Table
            writer.RenderEndTag();//Div
            writer.RenderEndTag();//Div
        }

        private void RenderPager(HtmlTextWriter writer, string IDSufix)
        {
            if (DesignMode)
            {
                if (Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "30px");
                if (Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginTop, "0px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, "0px");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, PagerContainerID + IDSufix);
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowX, "hidden");
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "visible");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //			PagerStyle.AddAttributesToRender(writer);
            //			writer.RenderBeginTag(HtmlTextWriterTag.Table);

            if (AllowPaging && PagerRow != null && PageCount > 1)
            {
                if (PagerSettings.Mode == PagerButtons.NumericFirstLast || PagerSettings.Mode == PagerButtons.Numeric)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, PagerStyle.CssClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                    int currentPage = this.PageIndex;
                    currentPage++;

                    for (int j = 1; j <= PageCount; j++)
                    {
                        if (currentPage == j)
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "active");

                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("javascript:UpdateDaisyText('Loading Page {0}');", j.ToString()));
                        //                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:__doPostBack('{0}','Page${1}')", this.UniqueID, j.ToString()));
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:paging({0});", j.ToString()));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(j.ToString());
                        writer.RenderEndTag(); //A
                        writer.RenderEndTag(); //Li
                    }
                    writer.RenderEndTag(); //Ul
                }
                else
                    ((CoolGridViewRow)PagerRow).RenderRow(writer);
            }

            //			writer.RenderEndTag();//Table
            writer.RenderEndTag();//Div
        }

        protected override void Render(HtmlTextWriter writer)
        {
            String orig_CssClass = CssClass;
            try
            {
                CssClass = (String.IsNullOrEmpty(this.CssClass) ? "CoolGridViewTable" : "CoolGridViewTable " + CssClass);
                Style["table-layout"] = "fixed";
                Style[HtmlTextWriterStyle.Display] = "table";
                Style[HtmlTextWriterStyle.BorderCollapse] = "collapse";
                //base.Width = GetCorrectedWidth();
                base.Width = Unit.Empty;// new Unit(100, UnitType.Pixel);
                base.Height = Unit.Empty;

                //from base Render
                if (this.Page != null)
                    this.Page.VerifyRenderingInServerForm(this);
                //from base Render
                this.PrepareControlHierarchy();

                writer.AddAttribute(HtmlTextWriterAttribute.Id, GridContainerID);
                AddPropertiesToRenderForGridContainer(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div); //Div 1

                //Render the Pager on Top
                if (AllowPaging && (this.PagerSettings.Position == PagerPosition.Top || this.PagerSettings.Position == PagerPosition.TopAndBottom) && this.PagerRow != null && PageCount > 1)
                    RenderPager(writer, "Top");

                //Render the fixed header
                //FF: I had to comment this out because we are trying to use in Uniphy and we need it to be 3.5. 
                //if (ShowHeader && (Rows.Count > 0 | this.ShowHeaderWhenEmpty))
                //    RenderHeader(writer);

                if (ShowHeader && (Rows.Count > 0))
                    RenderHeader(writer);

                if (DesignMode)
                {
                    if (Height != Unit.Empty)
                    {
                        double _height = Height.Value;
                        if (ShowHeader) _height -= 30;
                        if (ShowFooter) _height -= 30;
                        if (AllowPaging) _height -= 30;
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Height, _height.ToString() + "px");
                    }
                    if (Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Id, TableContainerID);
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.Value.ToString() + "px");

                writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowX, "hidden");
                writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "auto");

                //Compatibility for IE 6.0
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); //Div 2
                base.RenderContents(writer);
                writer.RenderEndTag();//Div 2

                if (ShowFooter)
                    RenderFooter(writer);
                //Render the pager at bottom
                if (AllowPaging && (this.PagerSettings.Position == PagerPosition.Bottom || this.PagerSettings.Position == PagerPosition.TopAndBottom) && this.PagerRow != null && PageCount > 1)
                    RenderPager(writer, "Bottom");

                writer.RenderEndTag();//Div 1

                string jsInitialize = @"<input type=""hidden"" id=""{6}"" name=""{6}"" value=""{7}"" /><script type=""text/javascript"" language=""javascript"">
//<![CDATA[
if (typeof var{3} =='undefined' || var{3} == null)
	var var{3} = new CoolGridView({{GridContainerID: ""{0}"",HeaderContainerID: ""{1}"",TableContainerID: ""{2}"",GridID: ""{3}"",FooterContainerID: ""{4}"",PagerContainerID: ""{5}"",HiddenFieldDataID: ""{6}"",FormID:""{8}"", AllowResizeColumn : {9}}});
var{3}.Initialize();
//]]>
</script>";

                //Register JavaScript Initialization
                jsInitialize = String.Format(
                                    jsInitialize,
                                    GridContainerID,
                                    HeaderContainerID,
                                    TableContainerID,
                                    ClientID,
                                    FooterContainerID,
                                    PagerContainerID,
                                    HiddenFieldDataID,
                                    _HiddenFieldDataValue,
                                    this.Page.Form.ClientID,
                                    AllowResizeColumn.ToString().ToLower()
                                    );
                writer.Write(jsInitialize);
            }
            finally
            {
                CssClass = orig_CssClass;
            }
        }

        #region Private Methods

        // METHOD:: Add checked items and persist them to memory
        private void PersistItems(int item, object idValue)
        {
            if (!SelectedIndexes.Keys.Contains(item))
            {
                SelectedIndexes.Add(item, idValue);
            }
        }

        // METHOD:: Remove persisted items no longer needed
        private void RemoveItems(int item)
        {
            if (SelectedIndexes.ContainsKey(item))
                SelectedIndexes.Remove(item);
        }

        public void ClearAllCheckedItems()
        {
            SelectedIndexes.Clear();
        }

        // METHOD:: Get checkeditems from gridview
        public void ManagePageCheckedItems()
        {
            foreach (GridViewRow row in this.Rows)
            {
                // Retrieve the reference to the checkbox
                CheckBox checkBox = (CheckBox)row.FindControl(InputCheckBoxField.CheckBoxID);
                if (checkBox != null && checkBox.Checked)
                {
                    Label idLabel = (Label)row.FindControl(_ObjectIdControl);
                    PersistItems(row.DataItemIndex, idLabel != null ? idLabel.Text : string.Empty);
                }
                else
                {
                    RemoveItems(row.DataItemIndex);
                }
            }

        }

        // METHOD:: Repopulate checked items and store in gridview.
        public void RepopulateCheckedItems()
        {
            foreach (GridViewRow row in this.Rows)
            {
                // Retrieve the reference to the checkbox
                CheckBox checkBox = (CheckBox)row.FindControl(InputCheckBoxField.CheckBoxID);
                if (checkBox != null && checkBox.Enabled)
                {
                    if (SelectedIndexes != null)
                    {
                        if (SelectedIndexes.Keys.Contains(row.DataItemIndex))
                        {
                            checkBox.Checked = true;
                        }
                    }
                }
            }
        }

        public int SelectedItemCount
        {
            get { return SelectedIndexes.Count; }
        }

        public ICollection<object> GetSelectedItemIds()
        {
            return SelectedIndexes.Values;
        }

        #endregion

        #region Properties
        // PROPERTY:: AutoGenerateCheckBoxColumn
        [Category("Behavior")]
        [Description("Whether a checkbox column is generated automatically at runtime")]
        [DefaultValue(false)]
        public bool AutoGenerateCheckBoxColumn
        {
            get
            {
                object allowCheckBox = ViewState["AutoGenerateCheckBoxColumn"];
                if (allowCheckBox == null)
                    return false;
                return (bool)allowCheckBox;
            }
            set { ViewState["AutoGenerateCheckBoxColumn"] = value; }
        }

        // PROPERTY:: CheckBoxColumnIndex
        [Category("Behavior")]
        [Description("Indicates the 0-based position of the checkbox column")]
        [DefaultValue(0)]
        public int CheckBoxColumnIndex
        {
            get
            {
                object checkBoxColumnIndex = ViewState["CheckBoxColumnIndex"];
                if (checkBoxColumnIndex == null)
                    return 0;
                return (int)checkBoxColumnIndex;
            }
            set
            {
                ViewState["CheckBoxColumnIndex"] = (value < 0 ? 0 : value);
            }
        }

        // PROPERTY:: SelectedIndices
        private IDictionary<int, object> SelectedIndexes
        {
            get
            {
                if (ViewState[SELECTEDITEMS] == null)
                {
                    ViewState[SELECTEDITEMS] = new Dictionary<int, object>();
                }
                return (IDictionary<int, object>)ViewState[SELECTEDITEMS];
            }
        }

        #endregion

        #region Protected Overrides

        // METHOD:: CreateColumns
        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            // Let the GridView create the default set of columns
            ICollection columnList = base.CreateColumns(dataSource, useDataSource);
            if (!AutoGenerateCheckBoxColumn)
                return columnList;

            // Add a checkbox column if required
            ArrayList extendedColumnList = AddCheckBoxColumn(columnList);
            return extendedColumnList;
        }

        //METHOD:: OnPageIndexing
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            //Need to store which checkboxes where stored
            ManagePageCheckedItems();

            base.OnPageIndexChanging(e);

            if (e.Cancel)
                return;

            //Set page to new page and bind to datasource
            this.PageIndex = e.NewPageIndex;
            this.DataSource = this.DataSource;
            this.DataBind();

            //Now Repopulate Checked items
            RepopulateCheckedItems();
        }

        #endregion
    }

    /// <summary>
    /// The base CoolGridViewRow
    /// </summary>
    public class BaseCoolGridViewRow : GridViewRow
    {
        //REgular expression for inserting an inner span in <TD></TD>
        private Regex _reg;
        private CoolGridView _ParentCoolGridView = null;
        /// <summary>
        /// Gets the parent CoolGridView
        /// </summary>
        internal CoolGridView ParentCoolGridView
        {
            get
            {
                if (_ParentCoolGridView == null)
                {
                    Control c = this;
                    while ((c = c.Parent) != null)
                    {
                        if (c is CoolGridView)
                        {
                            _ParentCoolGridView = (CoolGridView)c;
                            break;
                        }
                    }
                }
                return _ParentCoolGridView;
            }
            set
            {
                _ParentCoolGridView = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the System.Web.UI.WebControls.GridViewRow class.
        /// </summary>
        /// <param name="rowIndex">The index of the System.Web.UI.WebControls.GridViewRow object in the System.Web.UI.WebControls.GridView.Rows
        ///     collection of a System.Web.UI.WebControls.GridView control.</param>
        /// <param name="dataItemIndex">The index of the System.Web.UI.WebControls.GridViewRow object in the System.Web.UI.WebControls.GridView.Rows
        ///     collection of a System.Web.UI.WebControls.GridView control.</param>
        /// <param name="rowType">One of the System.Web.UI.WebControls.DataControlRowType enumeration values.</param>
        /// <param name="rowState">A bitwise combination of the System.Web.UI.WebControls.DataControlRowState
        ///     enumeration values.</param>
        public BaseCoolGridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
            : base(rowIndex, dataItemIndex, rowType, rowState)
        {
            string regex = "(^\\s*<(?:td|th)[^>]*?>)(.*)(</(?:td|th)>\\s*$)";
            RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase;
            _reg = new Regex(regex, options);
        }

        /// <summary>
        /// Render a control and return the resulting HTML
        /// </summary>
        /// <param name="control">Control to render</param>
        /// <returns>String that contains the rendered HTML</returns>
        protected string GetRendering(Control control)
        {
            using (StringWriter sw = new StringWriter())
            using (HtmlTextWriter htmlw = new HtmlTextWriter(sw))
            {
                control.RenderControl(htmlw);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Place a SPAN just inside TD
        /// </summary>
        /// <param name="Html">The html of the table cell</param>
        /// <returns>returns the resulting string HTML with span inserted inside, enclosing the TD's content</returns>
        protected string PutInsideSpan(string Html)
        {
            if (String.IsNullOrEmpty(Html))
                return Html;

            Match m = _reg.Match(Html);
            if (m != null && m.Groups.Count >= 3)
                return m.Groups[1].Captures[0].Value + "<span colspan='99'>" + m.Groups[2].Captures[0].Value + "</span>" + m.Groups[3].Captures[0].Value;

            return Html;
        }

        internal virtual void RenderColGroup(HtmlTextWriter writer)
        {
            //foreach (DataControlFieldCell cell in Cells)
            //    cell.Width = GetCorrectedCellWidth(cell);

            writer.RenderBeginTag(HtmlTextWriterTag.Colgroup);
            foreach (DataControlFieldCell cell in Cells)
            {
                if (cell.ContainingField.Visible)
                {
                    writer.Write("<col width=\"{0}\" {1} />",
                                        GetCorrectedCellWidth(cell).Value.ToString(),
                                        (String.IsNullOrEmpty(cell.ContainingField.HeaderStyle.CssClass) ? String.Empty : "class=\"" + cell.ContainingField.HeaderStyle.CssClass + "\"")
                                        );
                }
            }
            writer.RenderEndTag();
        }

        protected virtual Unit GetCorrectedCellWidth(DataControlFieldCell cell)
        {
            switch (RowType)
            {
                case DataControlRowType.Header:
                case DataControlRowType.Footer:
                    if (cell.ContainingField.HeaderStyle.Width == Unit.Empty || cell.ContainingField.HeaderStyle.Width.Type == UnitType.Percentage)
                    {
                        return ParentCoolGridView.DefaultColumnWidth;
                    }
                    else
                    {
                        return cell.ContainingField.HeaderStyle.Width;
                    }
                default:
                    return Width;
            }
        }

        internal virtual void RenderRow(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }

    /// <summary>
    /// CoolGridViewRow represents Header, Footer or Pager of CoolGridView
    /// </summary>
    public class CoolGridViewRow : BaseCoolGridViewRow
    {
        public CoolGridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
            : base(rowIndex, dataItemIndex, rowType, rowState)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            switch (RowType)
            {
                case DataControlRowType.Header:
                case DataControlRowType.Footer:
                    break;
            }
        }

        ////There seems to be a bug in IE 6 and IE 7 when rendering TD with defined width.
        ////The style white-space nowrap is not taking effect.
        ////The fix is to put an inner span just inside <TD></TD> and put the "white-space:nospace" stye on the span.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="writer"></param>
        //protected override void RenderContents(HtmlTextWriter writer)
        //{
        //    if (RowType != DataControlRowType.DataRow)
        //        foreach (Control control in Controls)
        //            writer.Write(PutInsideSpan(GetRendering(control)));
        //    else
        //        base.RenderContents(writer);
        //}
    }

    /// <summary>
    /// CoolGridViewFirstRow is the first row of CoolGridView. It is the header if header is visible, or the first datarow if header is not visible.
    /// </summary>
    public class CoolGridViewFirstRow : BaseCoolGridViewRow
    {
        public CoolGridViewFirstRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
            : base(rowIndex, dataItemIndex, rowType, rowState)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderColGroup(writer);
            base.Render(writer);
        }

        ////There seems to be a bug in IE 6 and IE 7 when rendering TD with defined width.
        ////The style white-space nowrap is not taking effect.
        ////The fix is to put an inner span just inside <TD></TD> and put the "white-space:nospace" stye on the span.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="writer"></param>
        //protected override void RenderContents(HtmlTextWriter writer)
        //{
        //    foreach (Control control in Controls)
        //        writer.Write(PutInsideSpan(GetRendering(control)));
        //}

        protected override Unit GetCorrectedCellWidth(DataControlFieldCell cell)
        {
            if (cell.ContainingField.HeaderStyle.Width == Unit.Empty || cell.ContainingField.HeaderStyle.Width.Type == UnitType.Percentage)
            {
                return ParentCoolGridView.DefaultColumnWidth;
            }
            else
            {
                return cell.ContainingField.HeaderStyle.Width;
            }
        }
    }
}
