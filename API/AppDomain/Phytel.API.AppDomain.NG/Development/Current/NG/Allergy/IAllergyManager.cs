using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Allergy;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public interface IAllergyManager
    {
        List<DTO.Allergy.Allergy> GetAllergies(GetAllergiesRequest request);
        void LogException(Exception ex);
    }
}