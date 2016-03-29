using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AppDomain.Engage.Population.DTO.Demographics
{
    public class PostPatientMergeResponse : IDomainResponse
    {
        public double Version { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ResponseStatus Status { get; set; }
    }
}