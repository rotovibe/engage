
namespace Phytel.Services.API.Pagination
{
    public interface IPaginationRequest
    {
        int? Take { get; set; }
        int Skip { get; set; }
        bool InlineCount { get; set; }
    }
}
