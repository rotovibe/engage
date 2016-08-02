using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class CohortPatientViewData
    {
        public string Id { get; set; }
        public string PatientID { get; set; }
        public List<SearchFieldData> SearchFields { get; set; }
        public string LastName { get; set; }
        public double Version { get; set; }
        public List<string> AssignedToContactIds { get; set; }
    }

    public class SearchFieldData
    {
        public SearchFieldData()
        {
            Value = null;
        }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
    }
}
