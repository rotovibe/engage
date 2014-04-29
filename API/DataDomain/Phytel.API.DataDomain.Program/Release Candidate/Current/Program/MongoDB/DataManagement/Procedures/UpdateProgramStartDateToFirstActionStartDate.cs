using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public class UpdateProgramStartDateToFirstActionStartDate : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdateProgramStartDateToFirstActionStartDate";

        public override void Implementation()
        {
            try
            {
                Results = new List<Result>();
                IProgramRepository<PostMongoProceduresResponse> repo = ProgramRepositoryFactory<PostMongoProceduresResponse>
                    .GetPatientProgramRepository(Request.ContractNumber, Request.Context, Request.UserId);

                List<MEPatientProgram> programs = (List<MEPatientProgram>)repo.SelectAll();

                DateTime rco;
                programs.ForEach(p =>
                {
                    List<DTO.Action> acts = new List<DTO.Action>();
                    rco = p.RecordCreatedOn;
                    p.Modules.ForEach(m =>
                    {
                        m.Actions.ToList().ForEach(i => { acts.Add(i); });
                    });
                    DateTime? date = acts.Where(a => a.Completed == true).Select(a => a.DateCompleted).Min();
                    if (date != null)
                    {
                        rco = (DateTime)date;

                        IProgramRepository<PostMongoProceduresResponse> arp = ProgramRepositoryFactory<PostMongoProceduresResponse>
                            .GetProgramAttributesRepository(Request.ContractNumber, Request.Context, p.UpdatedBy.ToString());
                        ProgramAttribute pa = new ProgramAttribute
                        {
                            PlanElementId = p.Id.ToString(),
                            StartDate = rco,
                             OptOut = false
                        };

                        arp.Update(pa);
                        Results.Add(new Result { Message = "PlanElement [" + p.Id.ToString() + "] in PatientProgramAttributes collection startdate modified to " + date });
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
