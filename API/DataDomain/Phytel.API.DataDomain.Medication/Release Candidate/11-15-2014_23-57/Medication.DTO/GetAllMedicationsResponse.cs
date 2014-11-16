using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class GetAllMedicationsResponse : DomainResponse
   {
        public List<MedicationData> Medications { get; set; }
   }

}
