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
    public class GetAllLanguagesResponse : IDomainResponse
    {
        public List<LanguagesLookUp> Languages { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class LanguagesLookUp : IdNamePair
    {
        public string Code { get; set; }
    }
}
