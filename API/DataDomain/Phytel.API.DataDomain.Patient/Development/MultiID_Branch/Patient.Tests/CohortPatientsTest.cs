using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Patient.MongoDB.DTO;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestClass]
    public class CohortPatientsTest
    {
        [TestMethod]
        public void GetCohortPatientsByID_WithNoFilter()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "530f9cff072ef715f4b411cf",
                Version = 1,
                Context = "NG",
                SearchFilter = "",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            IPatientDataManager pm = new PatientDataManager();
            GetCohortPatientsDataResponse response = pm.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithSingleFilter()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = 1,
                Context = "NG",
                SearchFilter = "Jonell",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            IPatientDataManager pm = new PatientDataManager();
            GetCohortPatientsDataResponse response = pm.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithDoubleFilterSpace()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = 1,
                Context = "NG",
                SearchFilter = "Jonell Tigue",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            IPatientDataManager pm = new PatientDataManager();
            GetCohortPatientsDataResponse response = pm.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithDoubleFilterComma()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = 1,
                Context = "NG",
                SearchFilter = "Tigue, Jonell",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            IPatientDataManager pm = new PatientDataManager();
            GetCohortPatientsDataResponse response = pm.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithStartingComma()
        {
            IPatientDataManager pm = new PatientDataManager
            {
                Factory = new PatientRepositoryFactory(),
                Helpers = new Helpers()
            };
            
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "53237514072ef709d84efe9d",
                Version = 1,
                Context = "NG",
                SearchFilter = "barr",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100,
                UserId = "0000000000000000000000000"
            };

            GetCohortPatientsDataResponse response = pm.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void UpdatePatientView_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "52f55858072ef709f84e5dee";
            string userId = "DDTestHarness";
            CohortPatientViewData view = GetCohortPatientView();
            PutUpdateCohortPatientViewRequest request = new PutUpdateCohortPatientViewRequest {
                CohortPatientView = view,
                Context = context,
                ContractNumber = contractNumber,
                PatientID = patientId,
                UserId = userId,
                Version = version
            };

            IPatientDataManager pm = new PatientDataManager();
            PutUpdateCohortPatientViewResponse response = pm.UpdateCohortPatientViewProblem(request);
        }

        private CohortPatientViewData GetCohortPatientView()
        {
            CohortPatientViewData cpv = new CohortPatientViewData
            {
                Id = "52f55858072ef709f84e5ded",
                LastName = "Blad",
                PatientID = "52f55858072ef709f84e5dee",
                SearchFields = searchFields()
            };

            return cpv;
        }

        private List<SearchFieldData> searchFields()
        {
            List<SearchFieldData> sfd = new List<SearchFieldData>();
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.FN,
                Value = "Linsey",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.LN,
                Value = "Blad",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.G,
                Value = "M",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.DOB,
                Value = "07/10/1900",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.MN,
                Value = "WWWW",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.SFX,
                Value = "MR",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.PN,
                Value = "Linseyo",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.Problem,
                Value = "528a66e3d4332317acc5095e",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = Constants.Problem,
                Value = "528a672fd4332317acc50968",
                Active = true
            });

            return sfd;
        }
    }
}
