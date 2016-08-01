using System.Net;

namespace Phytel.API.Common
{
    public class HttpObjectResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public T Body { get; set; }
        public string Message { get; set; }
    }
}
