using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientSystem.DTO;


namespace Phytel.Engage.Integrations.UOW
{
    public interface IDataDomain
    {
        object Save<T>(T patients, string contract);
        object Update<T>(T patients, string contract, string ddServiceUrl);
    }
}
