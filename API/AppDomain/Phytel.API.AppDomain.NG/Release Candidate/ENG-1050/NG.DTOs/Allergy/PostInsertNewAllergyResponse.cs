using Phytel.API.Common.CustomObject;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostInsertNewAllergyResponse : IDomainResponse
    {
        public bool Result { get; set; }
        public IdNamePair Allergy { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
