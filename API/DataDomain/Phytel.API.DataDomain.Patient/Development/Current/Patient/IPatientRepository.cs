using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Patient
{
    public interface IPatientRepository<T> : IRepository<T>
    {
        GetPatientsDataResponse Select(string[] patientIds);
        PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request);
        PutPatientFlaggedResponse UpdateFlagged(PutPatientFlaggedRequest request);
        object FindByID(string patientId, string userId);
    }
}
