using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;

namespace NGDataImport
{
    enum ImportOperation
    {
        INSERT = 1,
        UPDATE = 2
    }

    public class ImportData
    {
        private ImportOperation importOperation { get; set; }
        public PatientData patientData { get; set; }        
        public bool failed { get; set; }
        public string failedMessage { get; set; }

    }
}
