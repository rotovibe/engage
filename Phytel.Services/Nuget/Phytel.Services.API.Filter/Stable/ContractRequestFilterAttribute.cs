﻿using Phytel.Services.API.Provider;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;

namespace Phytel.Services.API.Filter
{
    public class ContractRequestFilterAttribute : RequestFilterAttribute
    {
        public IContractNumberProvider ContractNumberProvider {get;set;}
        public IHostContextProxy HostContextProxy { get; set; }
        
        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            if (HostContextProxy == null)
            {
                throw new Exception("IHostContextProxy was not initialized. Make sure IHostContextProxy has been registered with the IoC container.");
            }
            if (ContractNumberProvider == null)
            {
                throw new Exception("IContractNumberProvider was not initialized. Make sure IContractNumberProvider has been registered with the IoC container.");
            }

            string contractNumber = ContractNumberProvider.Get(requestDto);

            HostContextProxy.SetItem(Constants.HostContextKeyContractNumber, contractNumber);
        }
    }
}