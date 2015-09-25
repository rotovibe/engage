using System.Net;

namespace Phytel.API.DataDomain.Patient.Responses.Batch
{
    public class HttpObjectResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public T Body { get; set; }
        public string Message { get; set; }
    }
}
