using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace C3.WebControls
{
    public partial class GridView : System.Web.UI.WebControls.GridView//, IExtenderControl
    {
        #region Private Variables

        private const string GRIDVIEW_JS = "C3.WebControls.GridView.js";
        private const string SELECTEDITEMS = "gridSelectedItems";

        #endregion       

        #region Public Methods
        public GridView()
        {
            
              
        }
        #endregion

        #region Private Methods

        // METHOD:: Add checked items and persist them to memory
        private void PersistItems(int item)
        {
            if (!SelectedIndexes.Exists(i => i == item))
            {
                SelectedIndexes.Add(item);
            }
        }

        // METHOD:: Remove persisted items no longer needed
        private void RemoveItems(int item)
        {
            SelectedIndexes.Remove(item);
        }

        // METHOD:: Get checkeditems from gridview
        private void GetPageCheckedItems()
        {
            foreach (GridViewRow row in this.Rows)
            {
                // Retrieve the reference to the checkbox
                CheckBox checkBox = (CheckBox)row.FindControl(InputCheckBoxField.CheckBoxID);

                if (checkBox != null && checkBox.Checked)
                {
                    PersistItems(row.DataItemIndex);
                }
                else
                {
                    RemoveItems(row.DataItemIndex);
                }
            }
        }

        // METHOD:: Repopulate checked items and store in gridview.
        private void RepopulateCheckedItems()
        {
            foreach (GridViewRow row in this.Rows)
            {
                // Retrieve the reference to the checkbox
                CheckBox checkBox = (CheckBox)row.FindControl(InputCheckBoxField.CheckBoxID);

                if (SelectedIndexes != null)
                {
                    if (SelectedIndexes.Exists(i => i == row.DataItemIndex))
                    {
                        checkBox.Checked = true;
                    }
                }
            }
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
        
        
        //Override the datasource so we can store it and have access to it for paging or anything else we might want to do like filtering
        public override object DataSource
        {
            get
            {
                return (object)ViewState["DataSourceSpecial"];
            }
            set
            {                
                ViewState["DataSourceSpecial"] = value;
                base.DataSource = value;
            }
        }

        // PROPERTY:: SelectedIndices
        private List<int> SelectedIndexes
        {
            get
            {
                if (ViewState[SELECTEDITEMS] == null)
                {
                    ViewState[SELECTEDITEMS] = new List<int>();
                }
                return (List<int>)ViewState[SELECTEDITEMS];                
            }
        }

        // METHOD:: GetSelectedIndices
        public virtual int[] GetSelectedIndexes()
        {
            //Get Selected CheckBoxes from memory and return as a integer array
            return (int[])((ArrayList)ViewState[SELECTEDITEMS]).ToArray(typeof(int));
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

        // METHOD:: OnLoad
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Registering our JavaScript File and embedding it into whatever page is using it.
            Type t = this.GetType();            
            string url = Page.ClientScript.GetWebResourceUrl(t, GRIDVIEW_JS);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, GRIDVIEW_JS))
                Page.ClientScript.RegisterClientScriptInclude(t, GRIDVIEW_JS, url);
        }

        // METHOD:: OnPreRender
        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);                

            // Adjust each data row
            foreach (GridViewRow r in Rows)
            {
                // Get the appropriate style object for the row
                TableItemStyle style = GetRowStyleFromState(r.RowState);

                // Retrieve the reference to the checkbox
                CheckBox checkBox = (CheckBox)r.FindControl(InputCheckBoxField.CheckBoxID);

                // Build the ID of the checkbox in the header
                string headerCheckBoxID = String.Format(CheckBoxColumHeaderID, ClientID);

            }
        }

        // METHOD:: OnPageIndexing
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            //Need to store which checkboxes where stored
            GetPageCheckedItems();

            //Set page to new page and bind to datasource
            this.PageIndex = e.NewPageIndex;
            this.DataSource = this.DataSource;
            this.DataBind();

            //Now Repopulate Checked items
            RepopulateCheckedItems();
        }
        #endregion    
    }    
}
