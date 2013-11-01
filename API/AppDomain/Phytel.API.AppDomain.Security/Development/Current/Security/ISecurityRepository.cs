using Phytel.API.DataDomain.Security.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Security
{
    public interface ISecurityRepository<T> : IRepository<T>
    {
        APIUser GetUser(string userName, string apiKey, string product);
    }
}
