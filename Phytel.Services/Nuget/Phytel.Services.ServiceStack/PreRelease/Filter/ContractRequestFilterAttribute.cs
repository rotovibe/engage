using Phytel.Services.ServiceStack.Provider;
using Phytel.Services.ServiceStack.Proxy;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;

namespace Phytel.Services.ServiceStack.Filter
{
    public class ContractRequestFilterAttribute : RequestFilterAttribute
    {
        protected IContractNumberProvider _contractNumberProvider;
        protected IHostContextProxy _hostContextProxy;

        public ContractRequestFilterAttribute()
            : this(new ContractNumberProvider(), new HostContextProxy())
        {
        }

        public ContractRequestFilterAttribute(IContractNumberProvider contractNumberProvider, IHostContextProxy hostContextProxy)
        {
            _contractNumberProvider = contractNumberProvider;
            _hostContextProxy = hostContextProxy;
        }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            string contractNumber = _contractNumberProvider.Get(requestDto);

            if (string.IsNullOrEmpty(contractNumber))
            {
                throw new Exception("Contract number was not provided.");
            }
            else
            {
                _hostContextProxy.ContractNumber = contractNumber;
            }
        }
    }
}