namespace Phytel.API.AppDomain.NG.DTO.Context
{
    public class ServiceContext : IServiceContext
    {
        public string Contract { get; set; }
        public double Version { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public object Tag { get; set; }
    }
}