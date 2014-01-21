using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllLanguagesRespone : IDomainResponse
    {
        public List<LanguagesLookUp> States { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class LanguagesLookUp
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
