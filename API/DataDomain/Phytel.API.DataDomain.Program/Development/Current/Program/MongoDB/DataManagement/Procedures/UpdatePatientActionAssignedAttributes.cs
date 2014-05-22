using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Phytel.API.DataDomain.CareMember.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public class UpdatePatientActionAssignedAttributes : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdatePatientActionAssignedAttributes";
        public const string Description = "Procedure to update Action's existing fields like AssignedBy(NIGHT-876), AssignedDate(NIGHT-835), AssignedTo(NIGHT-877) and State Update date (NIGHT-952) in PatientProgram collection.";
        

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
                    List<Module> modules = mePP.Modules;
                    if (modules != null & modules.Count > 0)
                    {
                        foreach (Module meM in modules)
                        {
                            List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actions = meM.Actions;
                            if (actions != null && actions.Count > 0)
                            {
                                foreach (Phytel.API.DataDomain.Program.MongoDB.DTO.Action meA in actions)
                                {

                                    #region NIGHT-876, NIGHT-835
                                    if (meA.Enabled)
                                    {
                                        meA.AssignedBy = systemObjectId;
                                        meA.AssignedOn = mePP.RecordCreatedOn;
                                    }

                                    #endregion

                                    #region NIGHT-877
                                    GetPrimaryCareManagerDataRequest careMemberDataRequest = new GetPrimaryCareManagerDataRequest { Context = "NG", ContractNumber = "InHealth001", PatientId = mePP.PatientId.ToString(), UserId = Phytel.API.DataDomain.Program.DTO.Constants.SystemContactId, Version = 1 };
                                    ObjectId primaryCareManagerId = Helper.GetPatientsPrimaryCareManager(careMemberDataRequest, client);
                                    if (primaryCareManagerId == ObjectId.Empty)
                                    {
                                        meA.AssignedTo = null;
                                    }
                                    else
                                    {
                                        meA.AssignedTo = primaryCareManagerId;
                                    }
                                    #endregion

                                    #region NIGHT-952
                                    switch (meA.State)
                                    {
                                        case ElementState.NotStarted:
                                            meA.StateUpdatedOn = mePP.RecordCreatedOn;
                                            break;
                                        case ElementState.InProgress:
                                            meA.StateUpdatedOn = getStepResponsesEarliestCompletedDate(meA);
                                            break;
                                        case ElementState.Completed:
                                            meA.StateUpdatedOn = meA.DateCompleted;
                                            break;
                                    }
                                    #endregion
                                }
                            }
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

        private DateTime? getStepResponsesEarliestCompletedDate(Phytel.API.DataDomain.Program.MongoDB.DTO.Action meA)
        {
            List<DateTime> completedDates = new List<DateTime>();
            List<Step> steps = meA.Steps;
            if (steps != null && steps.Count > 0)
            {
                foreach (Step meS in steps)
                {
                   // meS.Responses 
                    //if (meA.DateCompleted != null)
                      //  completedDates.Add((DateTime)meA.DateCompleted);
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
