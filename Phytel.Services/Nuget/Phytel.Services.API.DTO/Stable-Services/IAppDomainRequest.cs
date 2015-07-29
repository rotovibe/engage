namespace Phytel.Services.API.DTO
{
    public interface IAppDomainRequest : IContractRequest, IVersionRequest
    {
        string ContractNumber { get; set; }

        string Token { get; set; }

        string UserId { get; set; }

        double Version { get; set; }
    }
}