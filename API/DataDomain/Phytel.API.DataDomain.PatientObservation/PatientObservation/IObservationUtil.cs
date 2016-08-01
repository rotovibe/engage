using System.Collections.Generic;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.DataDomain.PatientObservation
{
    public interface IObservationUtil
    {
        void FindAndInsert(List<PatientObservationData> podl, string gid, ObservationValueData ovd);
        bool GroupExists(List<PatientObservationData> list, string gid);
        string GetPreviousValues(List<ObservationValueData> list);
        List<string> GetPatientObservationIds(List<PatientObservationData> pod);
    }
}