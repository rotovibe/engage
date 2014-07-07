using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem.MongoDB.DataManagement.Procedures
{
    class UpdatePatientSystemId : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdatePatientSystemId";
        public const string Description = "Procedure to update sysid for the patient system record.";

        public override void Implementation()
        {
            try
            {
                Results = new List<Result>();

                ////IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(Request, RepositoryType.PatientProgram);
                //GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();
                //IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
                //IPatientSystemRepository<GetPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<GetPatientSystemDataResponse>.GetPatientSystemRepository("InHealth001", "NG", "5368ff2ad4332316288f3e3e");

                //PatientSystemData psd = new PatientSystemData { SystemID = "11580"};
                //// find patient by systemid
                
                ////List<MEPatientProgram> programs = (List<MEPatientProgram>)repo.SelectAll();
                //Results.Add(new Result { Message = "Total records updated: " + Results.Count });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}