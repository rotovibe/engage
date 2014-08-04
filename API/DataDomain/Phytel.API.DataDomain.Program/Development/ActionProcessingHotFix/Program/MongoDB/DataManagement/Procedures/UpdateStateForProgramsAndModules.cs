using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Phytel.API.DataDomain.CareMember.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Configuration;
using System.Text;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public class UpdateStateForProgramsAndModules : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdateStateForProgramsAndModules";
        public const string Description = "Procedure to update Program's and Module's state field(NIGHT-1091) in PatientProgram collection.";
        

        public override void Implementation()
        {
            try
            {
                Results = new List<Result>();
                ObjectId systemObjectId = ObjectId.Parse(Phytel.API.DataDomain.Program.DTO.Constants.SystemContactId);
                IRestClient client = new JsonServiceClient();
                IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(Request, Phytel.API.DataDomain.Program.DTO.RepositoryType.PatientProgram);

                List<MEPatientProgram> programs = (List<MEPatientProgram>)repo.SelectAll();

                foreach (MEPatientProgram mePP in programs)
                {
                    bool update = false;
                    List<Module> modules = mePP.Modules;
                    if (mePP.State == ElementState.NotStarted)
                    {
                        if (isAnyActionInProgressOrCompletedForAProgram(modules))
                        {
                            mePP.State = ElementState.InProgress;
                            update = true;
                        }
                    }
                    if (modules != null & modules.Count > 0)
                    {
                        foreach (Module meM in modules)
                        {
                            if (meM.State == ElementState.NotStarted)
                            {
                                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actions = meM.Actions;
                                if (isAnyActionInProgressOrCompletedForAModule(actions))
                                {
                                    meM.State = ElementState.InProgress;
                                    update = true;
                                }
                            }
                        }
                    }
                    if (update)
                    {
                        mePP.LastUpdatedOn = DateTime.UtcNow;
                        mePP.UpdatedBy = systemObjectId;
                        MEPatientProgram updatedProgram = mePP;
                        bool success = repo.Save(updatedProgram);
                        if (success)
                        {
                            Results.Add(new Result { Message = string.Format("Updated Program Id : '{0}' in PatientProgram collection.", updatedProgram.Id) });
                        }
                    }
                }
                Results.Add(new Result { Message = "Total records updated: " + Results.Count });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isAnyActionInProgressOrCompletedForAModule(List<DTO.Action> actions)
        {
            bool updateModuleState = false;
            if (actions != null && actions.Count > 0)
            {
                foreach (Phytel.API.DataDomain.Program.MongoDB.DTO.Action meA in actions)
                {
                    if (meA.State == ElementState.InProgress || meA.State == ElementState.Completed)
                    {
                        updateModuleState = true;
                        break;
                    }
                }
            }
            return updateModuleState;
        }

        private bool isAnyActionInProgressOrCompletedForAProgram(List<Module> modules)
        {
            bool updateProgramState = false;
            if (modules != null & modules.Count > 0)
            {
                foreach (Module meM in modules)
                {
                    List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actions = meM.Actions;
                    if (actions != null && actions.Count > 0)
                    {
                        foreach (Phytel.API.DataDomain.Program.MongoDB.DTO.Action meA in actions)
                        {
                            if (meA.State == ElementState.InProgress || meA.State == ElementState.Completed)
                            {
                                updateProgramState = true;
                                break;
                            }
                        }
                    }
                    if(updateProgramState)
                    {
                        break;
                    }
                }
            }
            return updateProgramState;
        }
    }
}
