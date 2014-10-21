using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Allergy;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public interface IAllergyEndpointUtil
    {
        List<DdAllergy> GetAllergies(GetAllergiesRequest request);
    }
}