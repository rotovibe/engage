using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Diagnostics;

namespace Phytel.API.DataDomain.Patient.Services.Test
{
    [TestClass]
    public class CohortPatientView_Tests
    {
        [TestMethod]
        public void Get_CohortPatientView_Response_Test()
        {
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "52e26f11072ef7191c111c54";

            IRestClient client = new JsonServiceClient();
            GetCohortPatientViewResponse response =
                client.Get<GetCohortPatientViewResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/",
                "http://localhost:8888/Patient",
                context,
                version,
                contractNumber,
                patientId));
        }

        [TestMethod]
        public void Put_Problem_In_CohortPatientView_Response_Test()
        {
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "52e26f11072ef7191c111c54";
            CohortPatientViewData cohortPatientView = GetCohortPatientView();

            IRestClient client = new JsonServiceClient();
            PutProblemInCohortPatientViewResponse response =
                client.Put<PutProblemInCohortPatientViewResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/update",
                "http://localhost:8888/Patient",
                context,
                version,
                contractNumber,
                patientId), new PutProblemInCohortPatientViewRequest
                {
                    CohortPatientView = cohortPatientView,
                    ContractNumber = contractNumber,
                    PatientID = patientId
                } as object);
        }

        private CohortPatientViewData GetCohortPatientView()
        {
            CohortPatientViewData cpv = new CohortPatientViewData
            {
                Id = "52e26f11072ef7191c111c53",
                LastName = "Tomerlin",
                PatientID = "52e26f11072ef7191c111c54",
                SearchFields = searchFields()
            };

            return cpv;
        }

        private List<SearchFieldData> searchFields()
        {
            List<SearchFieldData> sfd = new List<SearchFieldData>();
            sfd.Add(new SearchFieldData
            {
                FieldName = "FN",
                Value = "Heather",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "LN",
                Value = "Tomerlin",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "G",
                Value = "F",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "DOB",
                Value = "",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "MN",
                Value = "Q",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "SFX",
                Value = "",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "PN",
                Value = "Heathero",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "Problem",
                Value = "528a670ed4332317acc50963",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "Problem",
                Value = "528a66e3d4332317acc5095e",
                Active = true
            });
            sfd.Add(new SearchFieldData
            {
                FieldName = "Problem",
                Value = "528a671ad4332317acc50965",
                Active = true
            });

            return sfd;
        }
    }
}
