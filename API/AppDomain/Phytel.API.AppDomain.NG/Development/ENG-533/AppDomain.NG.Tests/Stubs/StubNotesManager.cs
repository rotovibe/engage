using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.Notes;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubNotesManager : INotesManager
    {
        public IUtilizationManager UtilManager
        {
            get { return new StubUtilsManager(); }
            set
            {
            }
        }

        public DTO.PatientNote GetPatientNote(DTO.GetPatientNoteRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.PatientNote> GetAllPatientNotes(IServiceContext context)
        {
            throw new NotImplementedException();
        }

        public DTO.PostPatientNoteResponse InsertPatientNote(DTO.PostPatientNoteRequest request)
        {
            var response = new DTO.PostPatientNoteResponse
            {
                Id = "111111111111111111112224",
                Version = 1
            };

            return response;
        }

        public DTO.PostDeletePatientNoteResponse DeletePatientNote(DTO.PostDeletePatientNoteRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.UpdatePatientNoteResponse UpdatePatientNote(DTO.UpdatePatientNoteRequest request)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
