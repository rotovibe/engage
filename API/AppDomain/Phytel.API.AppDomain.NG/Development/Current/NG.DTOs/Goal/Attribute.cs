using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Attribute
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }
        public string ControlType { get; set; }
        public Dictionary<int, string> Options { get; set; }
        public int Order { get; set; }
    }
}
