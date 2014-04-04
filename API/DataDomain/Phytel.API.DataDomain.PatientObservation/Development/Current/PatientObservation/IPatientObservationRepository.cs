using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation
{
    public interface IPatientObservationRepository<T> : IRepository<T>
    {
        object Initialize(object newEntity);
        object GetObservationsByType(object newEntity, bool standard);
        IEnumerable<object> FindObservationIdByPatientId(string Id);
        object FindRecentObservationValue(string observationTypeId, string patientId);
        List<int> GetAllowedObservationStates(string description);
    }
}
