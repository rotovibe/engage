using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientSystem
{
    public interface IMongoPatientSystemRepository : IRepository
    {
        IEnumerable<object> FindByPatientId(string patientId);
        IEnumerable<object> Find(object entity);
        object FindByExternalRecordId(string externalRecordId);
        List<PatientSystemData> Select(List<string> Ids);
    }
}
