using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Serializable]
    public class SpawnElement
    {
        public int ElementType { get; set; }
        public string ElementId { get; set; }
        public string Tag { get; set; }
    }
}
