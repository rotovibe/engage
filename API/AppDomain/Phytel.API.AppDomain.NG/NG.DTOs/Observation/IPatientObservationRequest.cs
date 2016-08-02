using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public interface IPatientObservationsRequest
    {
        string PatientId { get; set; }
        string UserId { get; set; }
        string ContractNumber { get; set; }
        string Token { get; set; }
        double Version { get; set; }
        string Context { get; set; }
        string ObservationId { get; set; }
    }
}
