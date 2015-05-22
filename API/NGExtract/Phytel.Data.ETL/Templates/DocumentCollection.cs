using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public void Export()
        {
            this.Execute();
        }
    }
}
