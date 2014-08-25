using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;
using Phytel.API.DataDomain.Scheduling.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class SchedulingManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDSchedulingUrl = ConfigurationManager.AppSettings["DDSchedulingUrl"];
        #endregion

        public List<ToDo> GetToDos(GetToDosRequest request)
        {
            try
            {
                List<ToDo> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDos", "POST")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Scheduling/ToDos",
                    DDSchedulingUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber), request.UserId);

                GetToDosDataResponse ddResponse =
                    client.Post<GetToDosDataResponse>(url, new GetToDosDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        Version = request.Version,
                        UserId = request.UserId,
                        AssignedToId = request.AssignedToId,
                        PatientId = request.PatientId,
                        StatusIds = request.StatusIds
                        
                    } as object);

                if (ddResponse != null && ddResponse.ToDos != null)
                {
                    result = new List<ToDo>();
                    List<ToDoData> dataList = ddResponse.ToDos;
                    foreach (ToDoData n in dataList)
                    {
                        ToDo toDo = new ToDo
                        {
                            AssignedToId = n.AssignedToId,
                            CategoryId = n.CategoryId,
                            ClosedDate = n.ClosedDate,
                            CreatedById = n.CreatedById,
                            CreatedOn = n.CreatedOn,
                            Description = n.Description,
                            DueDate = n.DueDate,
                            Id = n.Id,
                            PatientId = n.PatientId,
                            PriorityId = n.PriorityId,
                            ProgramIds = n.ProgramIds,
                            StatusId = n.StatusId,
                            Title = n.Title,
                            UpdatedOn = n.UpdatedOn
                        };

                        // Call Patient DD to get patient details.
                        if(!string.IsNullOrEmpty(toDo.PatientId))
                        {
                            string patientUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                                DDPatientServiceURL,
                                                                                                "NG",
                                                                                                request.Version,
                                                                                                request.ContractNumber,
                                                                                                toDo.PatientId), request.UserId);

                            GetPatientDataResponse response = client.Get<GetPatientDataResponse>(patientUrl);

                            if (response != null && response.Patient != null)
                            {
                                toDo.PatientDetails = new PatientDetails 
                                {
                                    FirstName = response.Patient.FirstName,
                                    LastName = response.Patient.LastName,
                                    MiddleName = response.Patient.MiddleName,
                                    PreferredName = response.Patient.PreferredName,
                                    Suffix = response.Patient.Suffix 
                                };
                            }
                        }
                        result.Add(toDo);
                    }
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetToDos()::" + ex.Message, ex.InnerException);
            }
        }

        public PostInsertToDoResponse InsertToDo(PostInsertToDoRequest request)
        {
            try
            {
                if (request.ToDo == null)
                    throw new Exception("The ToDo property cannot be null in the request.");

                PostInsertToDoResponse response = new PostInsertToDoResponse();
                //[Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/Insert", "PUT")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Scheduling/ToDo/Insert",
                                                                                DDSchedulingUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber), request.UserId);

                ToDoData data = new ToDoData
                {
                    AssignedToId = request.ToDo.AssignedToId,
                    CategoryId = request.ToDo.CategoryId,
                    Description = request.ToDo.Description,
                    DueDate = request.ToDo.DueDate,
                    PatientId = request.ToDo.PatientId,
                    PriorityId = request.ToDo.PriorityId,
                    ProgramIds = request.ToDo.ProgramIds,
                    Title = request.ToDo.Title,
                    StatusId = (int)Status.Open
                };
                PutInsertToDoDataResponse dataDomainResponse =
                    client.Put<PutInsertToDoDataResponse>(url, new PutInsertToDoDataRequest
                                                                                {
                                                                                    Context = "NG",
                                                                                    ContractNumber = request.ContractNumber,
                                                                                    Version = request.Version,
                                                                                    UserId = request.UserId,
                                                                                    ToDoData = data
                                                                                } as object);
                if (dataDomainResponse != null && !(string.IsNullOrEmpty(dataDomainResponse.Id)))
                {
                    response.Id = dataDomainResponse.Id;
                    response.Version = dataDomainResponse.Version;
                }

                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:InsertToDo()::" + ex.Message, ex.InnerException);
            }
        }

        public PostUpdateToDoResponse UpdateToDo(PostUpdateToDoRequest request)
        {
            try
            {
                if (request.ToDo == null)
                    throw new Exception("The ToDo property cannot be null in the request.");

                PostUpdateToDoResponse response = new PostUpdateToDoResponse();
                // [Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/Update", "PUT")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Scheduling/ToDo/Update",
                                                                                DDSchedulingUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber), request.UserId);

                ToDoData data = new ToDoData
                {
                    Id = request.ToDo.Id,
                    AssignedToId = request.ToDo.AssignedToId,
                    CategoryId = request.ToDo.CategoryId,
                    Description = request.ToDo.Description,
                    DueDate = request.ToDo.DueDate,
                    PatientId = request.ToDo.PatientId,
                    PriorityId = request.ToDo.PriorityId,
                    ProgramIds = request.ToDo.ProgramIds,
                    Title = request.ToDo.Title,
                    StatusId = request.ToDo.StatusId
                };
                if (request.ToDo.StatusId == (int)Status.Met || request.ToDo.StatusId == (int)Status.Abandoned)
                {
                    data.ClosedDate = DateTime.UtcNow;
                }

                PutUpdateToDoDataResponse dataDomainResponse =
                    client.Put<PutUpdateToDoDataResponse>(url, new PutUpdateToDoDataRequest
                                                                                {
                                                                                    ToDoData = data,
                                                                                    Context = "NG",
                                                                                    ContractNumber = request.ContractNumber,
                                                                                    Version = request.Version,
                                                                                    UserId = request.UserId,
                                                                                    
                                                                                } as object);
                if (dataDomainResponse != null && dataDomainResponse.Success)
                {
                    response.Version = dataDomainResponse.Version;
                }
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdateToDo()::" + ex.Message, ex.InnerException);
            }
        }

    }

}
