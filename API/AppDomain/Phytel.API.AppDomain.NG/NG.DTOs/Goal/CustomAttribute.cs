using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.AppDomain.NG.DTO.Goal;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class CustomAttribute
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int ControlType { get; set; }
        public List<string> Values { get; set; }
        public int Order { get; set; }
        public List<Option> Options { get; set; }
        public bool Required { get; set; }
    }
}
