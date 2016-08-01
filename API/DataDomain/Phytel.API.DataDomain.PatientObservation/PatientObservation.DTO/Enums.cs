using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public enum RepositoryType
    {
        Observation,
        PatientObservation
    }

    public enum ObservationDisplay
    {
        None = -1,
        Primary = 1,
        Secondary = 2
    }


    public enum ObservationState
    {
        Complete = 1,
        Active = 2,
        Inactive = 3,
        Resolved = 4,
        Decline = 5
    }

    public enum Status
    {
        Active = 1,
        Inactive = 2,
        InReview = 3,
        Met = 4,
        NotMet = 5
    }  
}
