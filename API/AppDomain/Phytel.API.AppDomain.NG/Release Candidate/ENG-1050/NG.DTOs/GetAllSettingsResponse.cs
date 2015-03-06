using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllSettingsResponse : IDomainResponse
    {
        public List<Setting> Settings { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }

    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
