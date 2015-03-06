using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Serializable]
    public class Step : PlanElement
    {
        public int StepTypeId { get; set; }
        public string Question { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Text { get; set; }
        public string Ex { get; set; }
        public int SelectType { get; set; }
        public bool IncludeTime { get; set; }
        public string ActionId { get; set; }
        public string Header { get; set; }
        public string SelectedResponseId { get; set; }
        public int ControlType { get; set; }
        public List<Response> Responses { get; set; }
        public int Status { get; set; }
    }
}
