﻿using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostAllergyResponse : IDomainResponse
    {
        public Allergy Allergy { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
