using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public class MoveProgramAttributeStartDateValue : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_MoveProgramAttributeStartDateValue";
        public const string Description = "Procedure to move the programattribute startdate to the newly migrated attributestartdate on the patientprogram document. Updated by is set to system id '5368ff2ad4332316288f3e3e' ";

        public override void Implementation()
        {
            try
            {
                Results = new List<Result>();

                IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(Request, RepositoryType.PatientProgram);

                List<MEPatientProgram> programs = (List<MEPatientProgram>)repo.SelectAll();

                programs.ForEach(p =>
                {
                    Request.UserId = p.UpdatedBy.ToString();
                    repo.UserId = Constants.SystemContactId;  // system

                    IProgramRepository arp = new ProgramRepositoryFactory().GetRepository(Request, RepositoryType.PatientProgramAttribute);
                    MEProgramAttribute pAtt = (MEProgramAttribute)arp.FindByPlanElementID(p.Id.ToString());

                    if (p.AttributeStartDate == null && pAtt.StartDate != null)
                    {
                        p.AttributeStartDate = pAtt.StartDate;
                        ProgramDetail pd = new ProgramDetail
                        {
                            AttrStartDate = p.AttributeStartDate,
                            Id = p.Id.ToString(),
                            ProgramState = (int)p.State,
                            Order = p.Order,
                            Enabled = p.Enabled,
                            Completed = p.Completed
                        };
                        PutProgramActionProcessingRequest request = new PutProgramActionProcessingRequest { Program = pd, ProgramId = p.Id.ToString()};
                        repo.Update(request);
                        Results.Add(new Result { Message = "PlanElement [" + p.Id.ToString() + "] in PatientProgramAttributes collection startdate moved" });
                    }
                });
                Results.Add(new Result { Message = "Total records updated: " + Results.Count });
            }
            catch (Exception ex)
            {
                Results.Add(new Result { Message = ex.Message + " : "+ ex.StackTrace });
                throw ex;
            }
        }
    }
}
