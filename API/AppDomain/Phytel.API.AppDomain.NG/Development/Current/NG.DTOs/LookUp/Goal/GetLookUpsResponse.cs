﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetLookUpsResponse : IDomainResponse
    {
        public List<IdNamePair> LookUps { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
