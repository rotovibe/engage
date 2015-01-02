using System;
namespace Phytel.Services.ServiceStack.DTO
{
    [Obsolete("Use IContractRequest interface only. Do not inherit from this.", true)]
    public abstract class ContractRequest : IContractRequest
    {
        [Obsolete("Use Phytel.Services.ServiceStack.Constants.HostContextKeyContractNumber instead.", true)]
        public const string HostContextKeyContractNumber = "ContractNumber";

        public virtual string ContractNumber { get; set; }
    }
}