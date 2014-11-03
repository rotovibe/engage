using Phytel.API.Common.CustomObject;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    public class GetMedFieldsResponse : IDomainResponse
    {
        public List<TextValuePair> Routes { get; set; }
        public List<TextValuePair> DosageForms { get; set; }
        public List<TextValuePair> Strengths { get; set; }
        public List<TextValuePair> Units { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
