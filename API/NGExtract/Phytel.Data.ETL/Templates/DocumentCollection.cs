﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phytel.Data.ETL.Templates
{
    public delegate void DocCollectionEventHandler(object sender, ETLEventArgs e);
    public abstract class DocumentCollection
    {
        public event DocCollectionEventHandler DocColEvent;

        protected virtual void OnDocColEvent(ETLEventArgs e)
        {
            if (DocColEvent != null)
            {
                DocColEvent(this, e);
            }
        }

        public string Contract { get; set; }
        public string ConnectionString { get; set; }

        public abstract void Execute();

        public void FormatError(Exception ex, SqlBulkCopy bcc)
        {
            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
            {
                var pattern = @"\d+";
                Match match = Regex.Match(ex.Message.ToString(), pattern);
                var index = Convert.ToInt32(match.Value) - 1;

                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                var sortedColumns = fi.GetValue(bcc);
                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                var metadata = itemdata.GetValue(items[index]);

                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);

                OnDocColEvent(new ETLEventArgs
                {
                    Message = "[" + Contract + "] PatientNotes():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " + ex.InnerException,
                    IsError = true
                });
            }
        }

        public void Export()
        {
            this.Execute();
        }
    }
}
