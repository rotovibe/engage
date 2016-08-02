using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Collections.Generic;
namespace Phytel.API.DataDomain.Patient.MongoDB.DTO
{
    public interface IDTOUtils
    {
        List<SearchField> CloneAppDomainCohortPatientViews(List<SearchFieldData> list);
    }
}
