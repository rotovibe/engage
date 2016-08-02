using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.UOW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Common;

namespace Phytel.Engage.Integrations.UOW.Tests
{
    [TestClass()]
    public class PatientsImportUowTest
    {
        [TestMethod()]
        public void GetPatientSystemsToLoad()
        {
            var uow = new PatientsImportUow();
            var psys = new List<PatientSystemData>
            {
                new PatientSystemData{ ExternalRecordId = "12345", Value="testing"  },
                new PatientSystemData{ ExternalRecordId = "11111", Value="testing2"  },
                new PatientSystemData{ ExternalRecordId = "12345", Value="testing3"  },
                new PatientSystemData{ ExternalRecordId = "55555", Value="testing4"  },
                new PatientSystemData{ ExternalRecordId = "44444", Value="testing5"  },
                new PatientSystemData{ ExternalRecordId = "44444", Value="testing6"  },
                new PatientSystemData{ ExternalRecordId = "44444", Value="testing7"  },
                new PatientSystemData{ ExternalRecordId = "44444", Value="testing8"  },
                new PatientSystemData{ ExternalRecordId = "44444", Value="testing9"  }
            };

            var httpReponse = new HttpObjectResponse<PatientData>();

            httpReponse.Body = new PatientData { ExternalRecordId = "12345", FirstName = "Mel" };
            httpReponse.Code = System.Net.HttpStatusCode.InternalServerError;

            var httpReponse2 = new HttpObjectResponse<PatientData>();
            httpReponse2.Body = new PatientData { ExternalRecordId = "11111", FirstName = "Jenny" };
            httpReponse2.Code = System.Net.HttpStatusCode.InternalServerError;

            var httpReponse3 = new HttpObjectResponse<PatientData>();
            httpReponse3.Body = new PatientData { ExternalRecordId = "44444", FirstName = "Elise" };
            httpReponse3.Code = System.Net.HttpStatusCode.OK;

            var prsl = new List<HttpObjectResponse<PatientData>>
            {
                httpReponse, httpReponse2, httpReponse3
            };

            var psdata = uow.GetPatientSystemsToLoad(psys, prsl);
        }
    }
}