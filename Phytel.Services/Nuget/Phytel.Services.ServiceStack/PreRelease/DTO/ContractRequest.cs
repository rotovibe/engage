namespace Phytel.Services.ServiceStack.DTO
{
    public abstract class ContractRequest : IContractRequest
    {
        public const string HostContextKeyContractNumber = "ContractNumber";

        public virtual string ContractNumber { get; set; }
    }
}