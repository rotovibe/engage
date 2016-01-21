namespace Phytel.Services.API.Pagination
{
    public interface IPaginationResponse
    {
        long? TotalCount { get; set; }
        long? TotalPages { get; set; }
        int? Take { get; set; }
        int Skip { get; set; }
    }
}
