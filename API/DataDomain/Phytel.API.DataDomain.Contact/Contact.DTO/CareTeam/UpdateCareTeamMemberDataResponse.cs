﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    public class UpdateCareTeamMemberDataResponse : IDomainResponse
    {
        public double Version { get; set; }

        public ResponseStatus Status { get; set; }
    }
}
