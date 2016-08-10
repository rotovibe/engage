using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using interop = Microsoft.Office.Interop.Excel;


namespace NGMedSuppImport
{
    public partial class Main : Form
    {
        private interop.Sheets excelSheets;
        private string fileName ;

        public Main()
        {
            InitializeComponent();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Search for source file.";
            openFileDialog1.DefaultExt = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            fileName = openFileDialog1.FileName;

            interop.Application excelApp = new interop.Application();

            // open an existing workbook
            string workbookPath = fileName;
            interop.Workbook excelWorkbook = excelApp.Workbooks.Open(workbookPath, 0, true, 5,
                "", "", false, interop.XlPlatform.xlWindows, "", true, false, 0, true, false,
                false);

            // get all sheets in workbook
            excelSheets = excelWorkbook.Worksheets;

            InitializeSheetComboBox(excelSheets);
        }

        private void InitializeSheetComboBox(interop.Sheets excelSheets)
        {
            SheetComboBox.Enabled = WorksheetLabel.Enabled = PreviewButton.Enabled = false;
            SheetComboBox.Items.Clear();
            foreach (var sheet in excelSheets)
            {
                SheetComboBox.Items.Add(((interop._Worksheet) sheet).Name);
            }
            SheetComboBox.Enabled = WorksheetLabel.Enabled = PreviewButton.Enabled = true;
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            if (SheetComboBox.SelectedItem == null) return;

            //// get some sheet
            var sheet = SheetComboBox.SelectedItem;
            Microsoft.Office.Interop.Excel.Worksheet excelWorksheet =
                (Microsoft.Office.Interop.Excel.Worksheet) excelSheets.get_Item(sheet);

            var cols = excelWorksheet.Columns.Count;

            object[,] data = excelWorksheet.Range[null, null].Value2;

            // Create new Column in DataTable
            for (int cCnt = 1; cCnt <= excelWorksheet.Range[null, null].Columns.Count; cCnt++)
            {
                //Column = new DataColumn();
                //Column.DataType = System.Type.GetType("System.String");
                //Column.ColumnName = cCnt.ToString();
                //DT.Columns.Add(Column);

                //// Create row for Data Table
                //for (rCnt = 0; rCnt <= Range.Rows.Count; rCnt++)
                //{
                //    textBox2.Text = rCnt.ToString();

                //    try
                //    {
                //        cellVal = (string)(data[rCnt, cCnt]);
                //    }
                //    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                //    {
                //        ConvertVal = (double)(data[rCnt, cCnt]);
                //        cellVal = ConvertVal.ToString();
                //    }

                //    // Add to the DataTable
                //    if (cCnt == 1)
                //    {

                //        Row = DT.NewRow();
                //        Row[cCnt.ToString()] = cellVal;
                //        DT.Rows.Add(Row);
                //    }
                //    else
                //    {

                //        Row = DT.Rows[rCnt];
                //        Row[cCnt.ToString()] = cellVal;

                //    }
                //}
            }

            //// access cell within sheet
            //Microsoft.Office.Interop.Excel.Range excelCell =
            //    (Microsoft.Office.Interop.Excel.Range) excelWorksheet.get_Range("A1", "A1");



            //// Populate a new data table and bind it to the BindingSource.
            //DataTable table = new DataTable();
            //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            //bindingSource1.DataSource = table;

            //// Resize the DataGridView columns to fit the newly loaded content.
            //PreviewDataGridView.AutoResizeColumns(
            //    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }
    }
}
