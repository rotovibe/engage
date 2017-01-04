using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;

namespace NGDataImport
{
    public enum ImportOperation
    {
        INSERT = 1,
        UPDATE = 2,
        LOOKUP_PATIENT = 3,
        LOOKUP_USER_CONTACT =4
    }

    public class ImportData
    {
        public ImportOperation importOperation { get; set; }
        public PatientData patientData { get; set; }            
        public bool failed { get; set; }
        public string failedMessage { get; set; }

    }
}
