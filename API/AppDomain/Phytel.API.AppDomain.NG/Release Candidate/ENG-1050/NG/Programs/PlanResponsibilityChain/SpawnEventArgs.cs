using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class SpawnEventArgs : EventArgs
    {
        public string Name { get; set; }
        public List<object> Tags { get; set; }

        public SpawnEventArgs()
        {
            Tags = new List<object>();
        }
    }
}
