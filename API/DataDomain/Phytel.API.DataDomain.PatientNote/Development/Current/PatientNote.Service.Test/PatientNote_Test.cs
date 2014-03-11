using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Service;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientNote.Services.Test
{
    [TestClass]
    public class PatientNote_Test
    {
        [TestMethod]
        public void InsertPatientNote_Test()
        {
            string url = "http://localhost:8888/PatientNote";
            PatientNoteData note = new PatientNoteData { Text = "DD_Service test note 2", CreatedById = "53043e53d433231f48de8a7a", PatientId = "52f55877072ef709f84e69b0" };
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/Insert", "PUT")]
            PutPatientNoteDataResponse response = client.Put<PutPatientNoteDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/Insert", url, context, version, contractNumber, note.PatientId),
                new PutPatientNoteDataRequest { Context = context, ContractNumber = contractNumber, PatientId = "52f55877072ef709f84e69b0", PatientNote = note, UserId = "53043e53d433231f48de8a7a", Version = version } as object);

            Assert.IsNotNull(response.Id);
        }
    }
}
