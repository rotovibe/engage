﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder
{
    public partial class ProgramsUserControl : UserControl
    {
        public string programNameText;

        public ProgramsUserControl()
        {
            InitializeComponent();
        }

        public ProgramsUserControl(string s)
        {
            programNameText = s;
            InitializeComponent();
            addItems();
        }


        private void athbyTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cliLabel_Click(object sender, EventArgs e)
        {

        }

        private void cliTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void nmTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void nmLabel_Click(object sender, EventArgs e)
        {

        }

        private void athbyLabel_Click(object sender, EventArgs e)
        {

        }

        private void snLabel_Click(object sender, EventArgs e)
        {

        }

        private void snTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void descLabel_Click(object sender, EventArgs e)
        {

        }

        private void descTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sdLabel_Click(object sender, EventArgs e)
        {

        }

        ////private void sdTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    esdTextBox.Text = 
        //}

        private void edLabel_Click(object sender, EventArgs e)
        {

        }

        //private void edTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    eedTextBox.Text = edTextBox.Text;
        //}

        private void stsLabel_Click(object sender, EventArgs e)
        {

        }

        private void stsNumericUpDwn_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void ProgramsUserControl_Load(object sender, EventArgs e)
        {
            //vTextBox.Text = "1.0";
            //uonTextBox.Text = System.DateTime.Now.ToString();
            //nmTextBox.Text = programNameText;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void addItems()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataColumn dc = dt.Columns.Add("PropertyName");
            DataColumn dc2 = dt.Columns.Add("PropertyValue");
            dr = dt.NewRow();
            dr["PropertyName"] = "Authored By:";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["PropertyName"] = "Client:";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["PropertyName"] = "Program Name:";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4["PropertyName"] = "Short Name:";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5["PropertyName"] = "Description:";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6["PropertyName"] = "Start Date:";
            dt.Rows.Add(dr6);
            DataRow dr7 = dt.NewRow();
            dr7["PropertyName"] = "End Date:";
            dt.Rows.Add(dr7);
            DataRow dr8 = dt.NewRow();
            dr8["PropertyName"] = "Status:";
            dt.Rows.Add(dr8);
            DataRow dr9 = dt.NewRow();
            dr9["PropertyName"] = "Order:";
            dt.Rows.Add(dr9);
            DataRow dr10 = dt.NewRow();
            dr10["PropertyName"] = "Eligibility Start Date:";
            dt.Rows.Add(dr10);
            DataRow dr11 = dt.NewRow();
            dr11["PropertyName"] = "Eligibility End Date:";
            dt.Rows.Add(dr11);
            DataRow dr12 = dt.NewRow();
            dr12["PropertyName"] = "Eligibility Requirements:";
            dt.Rows.Add(dr12);
            DataRow dr13 = dt.NewRow();
            dr13["PropertyName"] = "Population:";
            dt.Rows.Add(dr13);
            DataRow dr14 = dt.NewRow();
            dr14["PropertyName"] = "Completed:";
            dt.Rows.Add(dr14);
            DataRow dr15 = dt.NewRow();
            dr15["PropertyName"] = "Enabled:";
            dt.Rows.Add(dr15);
            DataRow dr16 = dt.NewRow();
            dr16["PropertyName"] = "Objectives:";
            dt.Rows.Add(dr16);
            DataRow dr17 = dt.NewRow();
            dr17["PropertyName"] = "Contract ID:";
            dt.Rows.Add(dr17);
            DataRow dr18 = dt.NewRow();
            dr18["PropertyName"] = "Program Template ID:";
            dt.Rows.Add(dr18);
            DataRow dr19 = dt.NewRow();
            dr19["PropertyName"] = "Source ID:";
            dt.Rows.Add(dr19);

            ds.Tables.Add(dt);
            dataGridView1.DataSource = dt.DefaultView;

            dataGridView1.Columns["PropertyName"].ReadOnly = true;

            //ListViewItem ttl = new ListViewItem("TTL:");
            //listView1.Items.Add(ttl);
            //ListViewItem uby = new ListViewItem("Updated By:");
            //listView1.Items.Add(uby);
            //ListViewItem uon = new ListViewItem("Updated On:");
            //listView1.Items.Add(uon);
            //ListViewItem v = new ListViewItem("Version:");
            //listView1.Items.Add(v);
        }

       


    }
}
