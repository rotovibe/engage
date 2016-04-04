using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AppDomain.Engage.Clinical.DTO.Medications
{
    public class PostPatientMedicationsResponse : IDomainResponse
    {
        public double Version { get; set; }
        public ResponseStatus Status { get; set; } // depricated
        public ResponseStatus ResponseStatus { get; set; }
    }
}
