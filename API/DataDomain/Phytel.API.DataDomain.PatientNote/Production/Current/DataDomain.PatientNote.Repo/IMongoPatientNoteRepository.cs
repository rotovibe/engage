using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public interface IMongoPatientNoteRepository : IRepository
    {
        IEnumerable<object> FindByPatientId(object request);
        void RemoveProgram(object entity, List<string> updatedProgramIds);
        IEnumerable<object> FindNotesWithAProgramId(string entityId);
        object FindByExternalRecordId(string externalRecordId);
        IEnumerable<object> Select(List<string> ids);
    }
}