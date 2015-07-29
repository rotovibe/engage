using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.Services.API.DTO
{
    public interface IDomainResponse
    {
        ResponseStatus ResponseStatus { get; set; }
    }
}