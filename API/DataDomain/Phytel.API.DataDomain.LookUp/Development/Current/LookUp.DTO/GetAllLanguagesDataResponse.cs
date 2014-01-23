using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllLanguagesDataResponse : IDomainResponse
    {
        public List<LanguageData> Languages { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class LanguageData : LookUpData
    {
        public string Code { get; set; }
    }
}
