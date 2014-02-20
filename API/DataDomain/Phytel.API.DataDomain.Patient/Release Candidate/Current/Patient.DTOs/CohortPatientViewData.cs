using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class CohortPatientViewData
    {
        public string Id { get; set; }
        public string PatientID { get; set; }
        public List<SearchFieldData> SearchFields { get; set; }
        public string LastName { get; set; }
        public string Version { get; set; }
    }

    public class SearchFieldData
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
    }
}

