using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class PatientNoteVisitor : VisitorBase
    {
        public override List<PatientNote> Visit(ref List<PatientNote> result)
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/{Count}", "GET")]
            IRestClient client = new JsonServiceClient();
            var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/{5}",
                DDPatientNoteUrl,
                "NG",
                Version,
                ContractNumber,
                PatientId,
                Count), UserId);

            GetAllPatientNotesDataResponse ddResponse = client.Get<GetAllPatientNotesDataResponse>(url);

            if (ddResponse == null || ddResponse.PatientNotes == null || ddResponse.PatientNotes.Count <= 0)
                return result;
            List<PatientNoteData> dataList = ddResponse.PatientNotes;

            result = dataList.Select(n => new PatientNote
            {
                Id = n.Id,
                PatientId = n.PatientId,
                Text = n.Text,
                ProgramIds = n.ProgramIds,
                CreatedOn = n.CreatedOn,
                CreatedById = n.CreatedById,
                TypeId = n.TypeId,
                MethodId = n.MethodId,
                OutcomeId = n.OutcomeId,
                WhoId = n.WhoId,
                SourceId = n.SourceId,
                Duration = n.Duration,
                ValidatedIdentity = n.ValidatedIdentity,
                ContactedOn = n.ContactedOn,
                UpdatedById = n.UpdatedById,
                UpdatedOn = n.UpdatedOn,
                DataSource = n.DataSource
            }).ToList();

            return result;
        }
    }
}
