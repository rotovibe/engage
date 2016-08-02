using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.Common.Audit
{
    public class DispatchEventArgs : EventArgs
    {
        public object payload { get; set; }
    }
}
