﻿using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public interface IAllergyManager
    {
        List<DTO.Allergy> GetAllergies(GetAllergiesRequest request);
        void IndexResultSet(List<DTO.Allergy> result);
        void LogException(Exception ex);
    }
}