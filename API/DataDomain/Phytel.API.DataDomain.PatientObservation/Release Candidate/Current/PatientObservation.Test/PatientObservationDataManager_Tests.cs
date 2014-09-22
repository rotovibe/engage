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
    public class PatientObservationDataManager_Tests
    {
        [TestMethod()]
        public void GetPatientObservationList_Test()
        {
            GetPatientProblemsSummaryRequest request = new GetPatientProblemsSummaryRequest();
            request.ContractNumber = "InHealth001";
            request.Context = "NG";
            request.PatientId = "5325db4cd6a4850adcbba8da";
            request.Version = 1.0;
            new PatientObservationDataManager().GetPatientProblemList(request);
        }

        [TestMethod()]
        public void GetPatientObservationByID_Test()
        {
            GetPatientObservationRequest request = new GetPatientObservationRequest
            {
                Context = "NG",
                ContractNumber = "InHealth001",
                ObservationID = "533ed16dd4332307bc592baf",
                PatientId = "5325db5ed6a4850adcbba912",
                UserId = "000000000000000000000000",
                Version = 1.0
            };
            GetPatientObservationResponse response = new PatientObservationDataManager().GetPatientObservationByID(request);
            Assert.IsNotNull(response);
        }
    }
}
