using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    public class MedNameSearchDoc
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductNDC { get; set; }
        public string CompositeName { get; set; }
        public string ProprietaryName { get; set; }
        public string ProprietaryNameSuffix { get; set; }
        public string SubstanceName { get; set; }

        public string RouteName { get; set; }
        public string DosageFormname { get; set; }
        public string Strength { get; set; }
        public string Unit { get; set; }
    }
}
