namespace Phytel.Services.ServiceStack.DTO
{
    public interface IDataDomainRequest : IContextRequest, IContractRequest, IVersionRequest
    {
        string Context { get; set; }

        string ContractNumber { get; set; }

        string UserId { get; set; }

        double Version { get; set; }
    }
}