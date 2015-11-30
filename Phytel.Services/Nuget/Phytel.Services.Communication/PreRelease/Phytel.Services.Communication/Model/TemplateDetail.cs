using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Communication
{
    public class TemplateDetail
    {
        public int TemplateID { get; set; }
        public string Description { get; set; }
        public string TemplateXMLBody { get; set; }
        public string TemplateXSLBody { get; set; }
    }
}
