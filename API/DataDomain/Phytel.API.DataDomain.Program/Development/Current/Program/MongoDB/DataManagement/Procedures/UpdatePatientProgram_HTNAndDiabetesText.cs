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
    class UpdatePatientProgram_HTNAndDiabetesText : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdatePatientProgram_HTNAndDiabetesText";

        public override void Implementation()
        {
            try
            {
                Results = new List<Result>();

                IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(Request, RepositoryType.PatientProgram);

                List<MEPatientProgram> programs = (List<MEPatientProgram>)repo.SelectAll();

                foreach (MEPatientProgram mePP in programs)
                {
                    if (string.Compare(mePP.Name, "BSHSI - Healthy Weight", true) == 0)
                    {
                        foreach (Module meM in mePP.Modules)
                        {
                            if (string.Compare(meM.Name, "BSHSI - Initial Assessment", true) == 0)
                            {
                                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actions = meM.Actions;
                                foreach(Phytel.API.DataDomain.Program.MongoDB.DTO.Action meA in actions)
                                {
                                    if (string.Compare(meA.Name, "Health History", true) == 0)
                                    {
                                        List<Step> steps = meA.Steps;
                                        foreach(Step meS in steps)
                                        {
                                            if (string.Compare(meS.Question, "Are there any health conditions that might impact your ability to achieve your health goals?", true) == 0)
                                            {
                                                List<MEPatientProgramResponse> responses = meS.Responses;
                                                foreach(MEPatientProgramResponse meR in responses)
                                                {
                                                    if(string.Compare(meR.Text, "Diabetes") == 0)
                                                    {
                                                        meR.Text = "Diabetes mellitus";
                                                        mePP.LastUpdatedOn = DateTime.UtcNow;
                                                        mePP.UpdatedBy = ObjectId.Empty;
                                                    }
                                                    else if (string.Compare(meR.Text, "HTN") == 0)
                                                    {
                                                        meR.Text = "Hypertension";
                                                        mePP.LastUpdatedOn = DateTime.UtcNow;
                                                        mePP.UpdatedBy = ObjectId.Empty;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        MEPatientProgram updatedProgram = mePP;
                        bool success = repo.Save(updatedProgram);
                        if (success)
                        {
                            Results.Add(new Result { Message = "Program Id [" + updatedProgram.Id.ToString() + "] updated." });
                        }
                        
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}