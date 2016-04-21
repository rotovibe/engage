using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class PutBulkInsertMedicationsResponse : DomainResponse
   {
        public bool Status { get; set; }
   }

}
