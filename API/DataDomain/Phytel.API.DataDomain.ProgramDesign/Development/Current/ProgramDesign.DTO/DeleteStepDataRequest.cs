﻿using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Step/{StepId}/Delete", "DELETE")]
    public class DeleteStepDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "StepId", Description = "Id of the Program that is beind deleted.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string StepId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Progam", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
