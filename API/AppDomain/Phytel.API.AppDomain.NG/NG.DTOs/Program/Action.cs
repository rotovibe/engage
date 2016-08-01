using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Serializable]
    public class Actions : PlanElement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ObjectiveInfo> Objectives { get; set; }
        public List<Step> Steps { get; set; }
        public int Status { get; set; }
        public string Text { get; set; }
        public string ModuleId { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
