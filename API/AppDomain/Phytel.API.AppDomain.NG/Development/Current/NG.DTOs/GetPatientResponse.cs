using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientResponse
    {
        public Patient Patient { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Patient
    {
        public string PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
    }
}
