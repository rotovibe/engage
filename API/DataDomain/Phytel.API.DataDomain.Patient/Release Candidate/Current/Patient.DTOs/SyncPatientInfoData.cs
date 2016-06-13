using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class SyncPatientInfoData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        //public int StatusId { get; set; }
        public int DeceasedId { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}
