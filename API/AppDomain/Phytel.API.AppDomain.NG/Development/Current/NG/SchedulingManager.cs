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
                        CreatedById = request.CreatedById,
                        PatientId = request.PatientId,
                        StatusIds = request.StatusIds,
                        //FromDate = (request.FromDate != null) ? TimeZoneInfo.ConvertTimeFromUtc((DateTime)request.FromDate, ): request.FromDate
                        FromDate = request.FromDate
                        
                    } as object);

                if (ddResponse != null && ddResponse.ToDos != null)
                {
                    result = new List<ToDo>();
                    List<ToDoData> dataList = ddResponse.ToDos;
                    foreach (ToDoData n in dataList)
                    {
                        ToDo toDo = convertToToDo(n);
                        // Call Patient DD to get patient details.
                        getPatientDetails(request, client, toDo);
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

        private static void getPatientDetails(GetToDosRequest request, IRestClient client, ToDo toDo)
        {
            if (!string.IsNullOrEmpty(toDo.PatientId))
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
                    StatusId = request.ToDo.StatusId
                };
                if (request.ToDo.StatusId == (int)Status.Met || request.ToDo.StatusId == (int)Status.Abandoned)
                {
                    data.ClosedDate = DateTime.UtcNow;
                }
                PutInsertToDoDataResponse dataDomainResponse =
                    client.Put<PutInsertToDoDataResponse>(url, new PutInsertToDoDataRequest
                                                                                {
                                                                                    Context = "NG",
                                                                                    ContractNumber = request.ContractNumber,
                                                                                    Version = request.Version,
                                                                                    UserId = request.UserId,
                                                                                    ToDoData = data
                                                                                } as object);
                if (dataDomainResponse != null && dataDomainResponse.ToDoData != null)
                {
                    response.ToDo = convertToToDo(dataDomainResponse.ToDoData);
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
                    StatusId = request.ToDo.StatusId,
                    DeleteFlag = request.ToDo.DeleteFlag
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
                if (dataDomainResponse != null && dataDomainResponse.ToDoData != null)
                {
                    response.ToDo = convertToToDo(dataDomainResponse.ToDoData);
                    response.Version = dataDomainResponse.Version;
                }
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdateToDo()::" + ex.Message, ex.InnerException);
            }
        }

        private ToDo convertToToDo(ToDoData toDoData)
        {
            ToDo data = null;
            if (toDoData != null)
            {
                data = new ToDo
                {
                    AssignedToId = toDoData.AssignedToId,
                    CategoryId = toDoData.CategoryId,
                    ClosedDate = toDoData.ClosedDate,
                    CreatedById = toDoData.CreatedById,
                    CreatedOn = toDoData.CreatedOn,
                    Description = toDoData.Description,
                    DueDate = toDoData.DueDate,
                    Id = toDoData.Id,
                    PatientId = toDoData.PatientId,
                    PriorityId = toDoData.PriorityId,
                    ProgramIds = toDoData.ProgramIds,
                    StatusId = toDoData.StatusId,
                    Title = toDoData.Title,
                    UpdatedOn = toDoData.UpdatedOn,
                    DeleteFlag = toDoData.DeleteFlag
                };
            }
            return data;
        }

    }

}
