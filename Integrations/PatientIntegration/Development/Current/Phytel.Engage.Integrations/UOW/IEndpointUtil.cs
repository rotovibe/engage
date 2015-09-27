using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientSystem.DTO;


namespace Phytel.Engage.Integrations.UOW
{
    public interface IEndpointUtil
    {
        List<HttpObjectResponse<PatientData>> SavePatientInfo<T>(T patients, string contract);
        List<HttpObjectResponse<PatientSystemData>> SaveSystemPatientInfo(List<PatientSystemData> patientSystems);
    }
}
