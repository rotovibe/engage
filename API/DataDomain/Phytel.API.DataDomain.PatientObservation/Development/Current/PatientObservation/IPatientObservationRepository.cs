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
        
    }
}
