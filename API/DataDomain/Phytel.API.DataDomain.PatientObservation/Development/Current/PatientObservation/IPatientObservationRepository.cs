using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation
{
    public interface IPatientObservationRepository<T> : IRepository<T>
    {
        object Initialize(object newEntity);
        object InitializeProblem(object newEntity);
        object GetObservationsByType(object newEntity, bool? standard, bool? status);
        IEnumerable<object> GetActiveObservations();
        IEnumerable<object> FindObservationIdByPatientId(string Id);
        object FindRecentObservationValue(string observationTypeId, string patientId);
        List<ObservationStateData> GetAllowedObservationStates();
        object FindByObservationID(string entityId, string patientId);
    }

    public interface IPatientObservationRepository : IRepository
    {
        object Initialize(object newEntity);
        object InitializeProblem(object newEntity);
        object GetObservationsByType(object newEntity, bool? standard, bool? status);
        IEnumerable<object> GetActiveObservations();
        IEnumerable<object> FindObservationIdByPatientId(string Id);
        object FindRecentObservationValue(string observationTypeId, string patientId);
        List<ObservationStateData> GetAllowedObservationStates();
        object FindByObservationID(string entityId, string patientId);
        object FindByID(string entityID, bool includeDeletedObservations);
    }
}
