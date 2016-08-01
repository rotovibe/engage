
using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
namespace Phytel.API.DataDomain.Medication.DTO
{
    public class GetMedicationNDCsDataResponse : DomainResponse
    {
        public List<string> NDCcodes { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
