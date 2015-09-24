namespace Phytel.API.DataDomain.Patient.DTO.Responses.Batch
{
    public class HttpObjectResponse<T>
    {
        public int ResponseCode { get; set; }
        public T Body { get; set; }
    }
}
