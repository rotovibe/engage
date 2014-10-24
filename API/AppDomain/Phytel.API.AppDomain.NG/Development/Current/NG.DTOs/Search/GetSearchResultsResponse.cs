using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    public class GetSearchResultsResponse : IDomainResponse
    {
        public List<SearchedItem> Allergies { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
