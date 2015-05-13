using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace C3.WebControls
{
    public partial class CoolGridView : System.Web.UI.WebControls.GridView
    {
        #region Constants
        private const string CheckBoxColumHeaderTemplate = "<input type='checkbox' hidefocus='true' id='{0}' name='{0}' {1} onclick='CheckAll(this)'>";
        private const string CheckBoxColumHeaderID = "{0}_HeaderButton";
        #endregion


        // METHOD:: add a brand new checkbox column
        protected virtual ArrayList AddCheckBoxColumn(ICollection columnList)
        {
            // Get a new container of type ArrayList that contains the given collection. 
            // This is required because ICollection doesn't include Add methods
            // For guidelines on when to use ICollection vs IList see Cwalina's blog
            ArrayList list = new ArrayList(columnList);

            // Determine the check state for the header checkbox
            string shouldCheck = "";
            string checkBoxID = String.Format(CheckBoxColumHeaderID, ClientID);
            if (!DesignMode)
            {
                object o = Page.Request[checkBoxID];
                if (o != null)
                {
                    shouldCheck = "checked=\"checked\"";
                }
            }


            // Create a new custom CheckBoxField object 
            InputCheckBoxField field = new InputCheckBoxField();
            field.HeaderText = String.Format(CheckBoxColumHeaderTemplate, checkBoxID, shouldCheck);
            field.ReadOnly = true;

            // Insert the checkbox field into the list at the specified position
            if (CheckBoxColumnIndex > list.Count)
            {
                // If the desired position exceeds the number of columns 
                // add the checkbox field to the right. Note that this check
                // can only be made here because only now we know exactly HOW 
                // MANY columns we're going to have. Checking Columns.Count in the 
                // property setter doesn't work if columns are auto-generated
                list.Add(field);
                CheckBoxColumnIndex = list.Count - 1;
            }
            else
                list.Insert(CheckBoxColumnIndex, field);

            // Return the new list
            return list;
        }


        // METHOD:: retrieve the style object based on the row state
        protected virtual TableItemStyle GetRowStyleFromState(DataControlRowState state)
        {
            switch (state)
            {
                case DataControlRowState.Alternate:
                    return AlternatingRowStyle;
                case DataControlRowState.Edit:
                    return EditRowStyle;
                case DataControlRowState.Selected:
                    return SelectedRowStyle;
                default:
                    return RowStyle;

                // DataControlRowState.Insert is not relevant here
            }
        }
    }
}
