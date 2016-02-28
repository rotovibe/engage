using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllLanguagesDataResponse : IDomainResponse
    {
        public List<LanguageData> Languages { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class LanguageData : IdNamePair
    {
        public string Code { get; set; }
        public bool Active { get; set; }
    }
}
