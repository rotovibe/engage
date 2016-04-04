using AppDomain.Engage.Clinical.DTO.Medications;
using Phytel.Services.API.Platform.Filter.Attributes;

namespace AppDomain.Engage.Clinical.Service
{
    [IsAuthenticatedFilter("br")]
    public class MedicationsService : ServiceBase
    {
        public PostPatientMedicationsResponse Post(PostPatientMedicationsRequest request)
        {
            return new PostPatientMedicationsResponse();
        }
    }
}