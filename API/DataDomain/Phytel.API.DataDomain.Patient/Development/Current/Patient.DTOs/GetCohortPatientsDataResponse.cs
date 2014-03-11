using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class GetCohortPatientsDataResponse : IDomainResponse
    {
        public List<PatientData> CohortPatients { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
