using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Medication
    {
        public string Id { get; set; }
        public string NDC { get; set; }
        public string ProductId { get; set; }
        public string ProprietaryName { get; set; }
        public string ProprietaryNameSuffix { get; set; }
        public string SubstanceName { get; set; }
        public string RouteName { get; set; }
        public string DosageFormName { get; set; }
        public string Strength { get; set; }
    }
}
