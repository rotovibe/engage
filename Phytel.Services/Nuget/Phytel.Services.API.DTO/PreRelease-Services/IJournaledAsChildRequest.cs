namespace Phytel.Services.API.DTO
{
    public interface IJournaledAsChildRequest : IJournaledRequest
    {
        string ParentActionId { get; set; }
    }
}