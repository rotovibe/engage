using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTO;

namespace Phytel.Engage.Integrations.UOW
{
    public class XpidMyEqualityComparer : IEqualityComparer<PatientXref>
    {
        public bool Equals(PatientXref x, PatientXref y)
        {
            return x.SendingApplication == y.SendingApplication && x.ExternalPatientID == y.ExternalPatientID && x.PhytelPatientID == y.PhytelPatientID;
        }

        public int GetHashCode(PatientXref obj)
        {
            return (obj.SendingApplication + obj.ExternalPatientID + obj.PhytelPatientID).GetHashCode();
        }
    }

    public class XdpidMyEqualityComparer : IEqualityComparer<PatientXref>
    {
        public bool Equals(PatientXref x, PatientXref y)
        {
            return x.SendingApplication == y.SendingApplication && x.ExternalDisplayPatientId == y.ExternalDisplayPatientId && x.PhytelPatientID == y.PhytelPatientID;
        }

        public int GetHashCode(PatientXref obj)
        {
            return (obj.SendingApplication + obj.ExternalDisplayPatientId + obj.PhytelPatientID).GetHashCode();
        }
    }

    public class PsDMyEqualityComparer : IEqualityComparer<PatientSystemData>
    {
        public bool Equals(PatientSystemData x, PatientSystemData y)
        {
            return x.PatientId == y.PatientId && x.SystemId == y.SystemId && x.Value == y.Value;
        }

        public int GetHashCode(PatientSystemData obj)
        {
            return (obj.PatientId + obj.SystemId + obj.Value).GetHashCode();
        }
    }
}
