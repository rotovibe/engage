using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Repo.DTOs
{
    public class PCPPhone
    {
        public int PatientID { get; set; }
        public int PCPId { get; set; }
        public string PCP_Name { get; set; }
        public string Facility { get; set; }
        public string desc { get; set; }
        public string Phone { get; set; }
    }
}
