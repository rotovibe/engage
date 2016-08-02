namespace Phytel.API.AppDomain.NG.DTO.Context
{
    public interface IServiceContext
    {
        string Contract { get; set; }
        double Version { get; set; }
        string UserId { get; set; }
        string Token { get; set; }
        object Tag { get; set; }
    }
}