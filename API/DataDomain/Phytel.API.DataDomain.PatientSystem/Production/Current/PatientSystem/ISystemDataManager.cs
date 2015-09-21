using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem
{
    public interface ISystemDataManager
    {
        List<SystemData> GetSystems(GetSystemsDataRequest request);
    }
}
