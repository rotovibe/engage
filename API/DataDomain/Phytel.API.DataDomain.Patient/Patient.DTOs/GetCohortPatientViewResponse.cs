using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class GetCohortPatientViewResponse : IDomainResponse
   {
       public CohortPatientViewData CohortPatientView { get; set; }
       public double Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}
