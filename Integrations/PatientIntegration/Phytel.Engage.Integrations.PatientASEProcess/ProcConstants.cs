using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Process
{
    public static class ProcConstants
    {
        public const string Contracts = "ORLANDOHEALTH001";
        public const int TakeCount = 1200;
        public const string DDPatientServiceUrl = "http://azurePhytel.cloudapp.net:59901/Patient";
        public const string DDPatientSystemUrl = "http://azurePhytel.cloudapp.net:59901/PatientSystem";
        public const string DDPatientNoteUrl = "http://azurePhytel.cloudapp.net:59901/PatientNote";
        public const string DDContactServiceUrl = "http://azurePhytel.cloudapp.net:59901/Contact";
    }
}
