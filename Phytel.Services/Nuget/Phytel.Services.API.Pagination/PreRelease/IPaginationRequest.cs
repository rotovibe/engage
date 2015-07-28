
namespace Phytel.Services.Pagination
{
    public interface IPaginationRequest
    {
        int? Take { get; set; }
        int Skip { get; set; }
    }
}
