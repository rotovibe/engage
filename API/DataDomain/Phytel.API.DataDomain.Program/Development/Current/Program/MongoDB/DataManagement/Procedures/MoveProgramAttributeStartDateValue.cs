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
    public class MoveProgramAttributeStartDateValue : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_MoveProgramAttributeStartDateValue";
        public const string Description = "Procedure to move the programattribute startdate to the newly migrated attributestartdate on the patientprogram document.";

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

                    IProgramRepository arp = new ProgramRepositoryFactory().GetRepository(Request, RepositoryType.PatientProgramAttribute);
                    MEProgramAttribute pAtt = (MEProgramAttribute)arp.FindByPlanElementID(p.Id.ToString());

                    if (p.AttributeStartDate == null && pAtt.StartDate != null)
                    {
                        p.AttributeStartDate = pAtt.StartDate;
                        repo.Update(p);
                        Results.Add(new Result { Message = "PlanElement [" + p.Id.ToString() + "] in PatientProgramAttributes collection startdate moved" });
                    }
                });
            }
            catch (Exception ex)
            {
                Results.Add(new Result { Message = ex.Message + " : "+ ex.StackTrace });
                throw ex;
            }
        }
    }
}
