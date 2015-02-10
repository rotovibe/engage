using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.Services.ServiceStack.DTO
{
    public interface IDomainResponse
    {
        ResponseStatus Status { get; set; }
    }
}