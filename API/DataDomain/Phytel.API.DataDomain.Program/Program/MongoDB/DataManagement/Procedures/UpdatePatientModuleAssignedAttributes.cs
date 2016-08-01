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
    public class UpdatePatientModuleAssignedAttributes : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdatePatientModuleAssignedAttributes";
        public const string Description = "Procedure to update Module's existing fields like AssignedBy(NIGHT-948), AssignedDate(NIGHT-949), AssignedTo(NIGHT-950) and State Update date (NIGHT-951) in PatientProgram collection.";
        

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
                    GetPrimaryCareManagerDataRequest careMemberDataRequest = new GetPrimaryCareManagerDataRequest { Context = Request.Context, ContractNumber = Request.ContractNumber, PatientId = mePP.PatientId.ToString(), UserId = Phytel.API.DataDomain.Program.DTO.Constants.SystemContactId, Version = 1 };
                    ObjectId primaryCareManagerId = Helper.GetPatientsPrimaryCareManager(careMemberDataRequest, client);

                    List<Module> modules = mePP.Modules;
                    if (modules != null & modules.Count > 0)
                    {
                        foreach (Module meM in modules)
                        {
                            #region NIGHT-948, NIGHT-949
                            if (meM.Enabled)
                            {
                                meM.AssignedBy = systemObjectId;
                                meM.AssignedOn = mePP.RecordCreatedOn;
                            }
                            
                            #endregion

                            #region NIGHT-950
                            if (primaryCareManagerId == ObjectId.Empty)
                            {
                                meM.AssignedTo = null;
                            }
                            else
                            {
                                meM.AssignedTo = primaryCareManagerId;
                            }
                            #endregion

                            #region NIGHT-951
                            switch (meM.State)
                            {
                                case ElementState.NotStarted:
                                    meM.StateUpdatedOn = mePP.RecordCreatedOn;
                                    break;
                                case ElementState.InProgress:
                                    meM.StateUpdatedOn = getActionsEarliestCompletedDate(meM);
                                    break;
                                case ElementState.Completed:
                                    meM.StateUpdatedOn = meM.DateCompleted;
                                    break;
                            }
                            #endregion
                        }
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

        private DateTime? getActionsEarliestCompletedDate(Module meM)
        {
            List<DateTime> completedDates = new List<DateTime>();       
            List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actions = meM.Actions;
            if (actions != null && actions.Count > 0)
            {
                foreach (Phytel.API.DataDomain.Program.MongoDB.DTO.Action meA in actions)
                {
                    if (meA.DateCompleted != null)
                        completedDates.Add((DateTime)meA.DateCompleted);
                }
            }
            completedDates.Sort();
            if (completedDates.Count > 0)
            {
                return completedDates[0];
            }
            else
            {
                return null;
            }
        }
    }
}
