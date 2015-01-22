using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Medication.Test.Stubs
{
    public class StubPatientMedSupp_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        IPatientMedSuppDataManager cm = new StubPatientMedSuppDataManager();
    }
}
