using Phytel.Services.ServiceStack.Provider;
using Phytel.Services.ServiceStack.Proxy;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;

namespace Phytel.Services.ServiceStack.Filter
{
    public class ContractRequestFilterAttribute : RequestFilterAttribute
    {
        public IContractNumberProvider ContractNumberProvider {get;set;}
        public IHostContextProxy HostContextProxy { get; set; }
        
        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            string contractNumber = ContractNumberProvider.Get(requestDto);

            if (string.IsNullOrEmpty(contractNumber))
            {
                throw new Exception("Contract number was not provided.");
            }
            else
            {
                HostContextProxy.ContractNumber = contractNumber;
            }
        }
    }
}