using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientObservation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;
namespace Phytel.API.DataDomain.PatientObservation.Tests
{
    [TestClass()]
    public class MongoPatientObservationRepository_Tests
    {
        [TestMethod()]
        public void FindByObservationID_Test()
        {

            MongoPatientObservationRepository repo = new MongoPatientObservationRepository("InHealth001");

            object obj = repo.FindByObservationID("533ed16dd4332307bc592baf", "5325db5ed6a4850adcbba912");
        }
    }
}
