using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO.Allergy
{
    public class PostInsertPatientAllergyResponse : IDomainResponse
    {
        public bool Result { get; set; }
        //public List<Allergy> PatientAllergies { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
