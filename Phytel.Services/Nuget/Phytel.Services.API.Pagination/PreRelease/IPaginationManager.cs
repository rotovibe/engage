namespace Phytel.Services.API.Pagination
{
    public interface IPaginationManager
    {
        int GetNormalizeSkip(object requestDto);
        int? GetNormalizeTake(object requestDto);
    }
}
