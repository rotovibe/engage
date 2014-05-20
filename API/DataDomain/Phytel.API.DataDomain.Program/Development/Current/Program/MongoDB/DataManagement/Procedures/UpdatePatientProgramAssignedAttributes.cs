using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Phytel.API.DataDomain.CareMember.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Configuration;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public class UpdatePatientProgramAssignedAttributes : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdatePatientProgramAssignedAttributes";
        public const string Description = "Procedure to update the existing fields like AssignedBy(NIGHT-832), AssignedDate(NIGHT-831), AssignedTo(NIGHT-833) and State Update date (NIGHT-868) in PatientProgram collection.";
        public string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];

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

                    #region NIGHT-832, NIGHT831
                    mePP.AssignedBy = systemObjectId;
                    mePP.AssignedOn = mePP.RecordCreatedOn;
                    #endregion
                     
                    #region NIGHT-833
                    GetPrimaryCareManagerDataRequest careMemberDataRequest = new GetPrimaryCareManagerDataRequest { Context = "NG", ContractNumber = "InHealth001", PatientId = mePP.PatientId.ToString(), UserId = Phytel.API.DataDomain.Program.DTO.Constants.SystemContactId, Version = 1 };
                    ObjectId primaryCareManagerId = getPatientsPrimaryCareManager(careMemberDataRequest, client);
                    if (primaryCareManagerId == ObjectId.Empty)
                    {
                        mePP.AssignedTo = null;
                    }
                    else
                    {
                        mePP.AssignedTo = primaryCareManagerId;
                    }
                    #endregion

                    #region NIGHT-868
                    DateTime? stateUpdatedOn = null;
		            switch(mePP.State)
                    {
                        case ElementState.NotStarted :
                            mePP.StateUpdatedOn = mePP.AssignedOn;
                            break;
                        case ElementState.InProgress :
                            stateUpdatedOn = getActionsEarliestCompletedDate(mePP);
                            mePP.StateUpdatedOn = stateUpdatedOn;
                            break;
                        case ElementState.Closed :
                            stateUpdatedOn = getDisenrollmentActionsCompletedDate(mePP);
                            mePP.StateUpdatedOn = stateUpdatedOn;
                            break;
                    }
	                #endregion

                    mePP.LastUpdatedOn = DateTime.UtcNow;
                    mePP.UpdatedBy = systemObjectId;
                    MEPatientProgram updatedProgram = mePP;
                    bool success = repo.Save(updatedProgram);
                    if (success)
                    {
                        Results.Add(new Result { Message = string.Format("Updated values are AssignedBy(aby) = '{0}', AssignedDate(aon) = '{1}', AssignedTo(ato) = '{2}', StateUpdatedOn(stuon) = '{3}' for Program Id = '{4}' in PatientProgram collection.", mePP.AssignedBy, mePP.AssignedOn, mePP.AssignedTo, mePP.StateUpdatedOn, updatedProgram.Id) });
                    }
                }
                Results.Add(new Result { Message = "Total records updated: " + Results.Count });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DateTime? getDisenrollmentActionsCompletedDate(MEPatientProgram mePP)
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
                            if (string.Compare(meA.Name, "Disenrollment", true) == 0)
                            {
                                if (meA.DateCompleted != null)
                                    return meA.DateCompleted;
                            }
                        }
                    }
                }
           }
           return null;
        }

        private DateTime? getActionsEarliestCompletedDate(MEPatientProgram mePP)
        {
            List<DateTime> completedDates = new List<DateTime>();       
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
                            if (meA.DateCompleted != null)
                                completedDates.Add((DateTime)meA.DateCompleted);
                        }
                    }
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

        private ObjectId getPatientsPrimaryCareManager(GetPrimaryCareManagerDataRequest request, IRestClient client)
        {
            try
            {

                ObjectId pcmObjectId = ObjectId.Empty;
                // [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PrimaryCareManager", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PrimaryCareManager",
                    DDCareMemberUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                GetCareMemberDataResponse ddResponse = client.Get<GetCareMemberDataResponse>(url);

                if (ddResponse != null && ddResponse.CareMember != null)
                {
                    pcmObjectId = ObjectId.Parse(ddResponse.CareMember.ContactId);
                }
                return pcmObjectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
