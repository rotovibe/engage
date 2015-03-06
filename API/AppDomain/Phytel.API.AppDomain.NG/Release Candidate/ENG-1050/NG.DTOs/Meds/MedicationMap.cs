using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class MedicationMap
    {
        public string Id { get; set; }
        public string SubstanceName { get; set; }
        public string FullName { get; set; }
        public string Strength { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public bool Custom { get; set; }
        public bool Verified { get; set; }
    }
}
