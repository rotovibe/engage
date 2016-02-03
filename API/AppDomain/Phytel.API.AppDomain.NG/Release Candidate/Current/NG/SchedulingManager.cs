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
using System.Linq;
using System.Web;

namespace Phytel.API.AppDomain.NG
{
    public class SchedulingManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDSchedulingUrl = ConfigurationManager.AppSettings["DDSchedulingUrl"];
        #endregion
        
        public GetToDosResponse GetToDos(GetToDosRequest request)    
        {
            try
            {
                GetToDosResponse response = new GetToDosResponse();
                List<ToDo> toDosResult = null;
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
                        NotAssignedToId = request.NotAssignedToId,
                        CreatedById = request.CreatedById,
                        PatientId = request.PatientId,
                        StatusIds = request.StatusIds,
                        CategoryIds = request.CategoryIds,
                        PriorityIds = request.PriorityIds,
                        FromDate = request.FromDate,
                        Skip = request.Skip,
                        Take = request.Take,
                        Sort = request.Sort
                    } as object);

                if (ddResponse != null && ddResponse.ToDos != null)
                {
                    response.TotalCount = ddResponse.TotalCount;
                    toDosResult = new List<ToDo>();
                    List<ToDoData> dataList = ddResponse.ToDos;
                    var distintPatients = dataList.GroupBy(p => p.PatientId).Select(grp => grp.FirstOrDefault()).ToList();
                    List<string> patientIds = distintPatients.Select(p => p.PatientId).ToList();
                    // Call Patient DD to get patient details.
                    Dictionary<string, PatientData> patients = getPatients(request.Version, request.ContractNumber, request.UserId, client, patientIds);
                    
                    foreach (ToDoData n in dataList)
                    {
                        ToDo toDo = convertToToDo(n);
                        if (patients != null && !string.IsNullOrEmpty(n.PatientId))
                        {
                            PatientData pd;
                            if(patients.TryGetValue(n.PatientId, out pd))
                            {
                                toDo.PatientDetails = new PatientDetails
                                {
                                    Id = pd.Id,
                                    FirstName = pd.FirstName,
                                    LastName = pd.LastName,
                                    MiddleName = pd.MiddleName,
                                    PreferredName = pd.PreferredName,
                                    Suffix = pd.Suffix
                                };
                            }
                        }
                        toDosResult.Add(toDo);
                    }
                }
                response.ToDos = toDosResult;
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetToDos()::" + ex.Message, ex.InnerException);
            }
        }

        private static void getPatient(double version, string contractNumber, string userId, IRestClient client, ToDo toDo)
        {
            if (!string.IsNullOrEmpty(toDo.PatientId))
            {
                string patientUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                    DDPatientServiceURL,
                                                                                    "NG",
                                                                                    version,
                                                                                    contractNumber,
                                                                                    toDo.PatientId), userId);

                GetPatientDataResponse response = client.Get<GetPatientDataResponse>(patientUrl);

                if (response != null && response.Patient != null)
                {
                    toDo.PatientDetails = new PatientDetails
                    {
                        Id = response.Patient.Id,
                        FirstName = response.Patient.FirstName,
                        LastName = response.Patient.LastName,
                        MiddleName = response.Patient.MiddleName,
                        PreferredName = response.Patient.PreferredName,
                        Suffix = response.Patient.Suffix
                    };
                }
            }
        }

        private static Dictionary<string, PatientData> getPatients(double version, string contractNumber, string userId, IRestClient client, List<string> patientIds)
        {
            Dictionary<string, PatientData> data = null;
            //[Route("/{Context}/{Version}/{ContractNumber}/Patients/Ids", "POST")]
            string patientDDURL = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patients/Ids",
                                                                            DDPatientServiceURL,
                                                                            "NG",
                                                                            version,
                                                                            contractNumber), userId);

            GetPatientsDataResponse patientDDResponse =
                client.Post<GetPatientsDataResponse>(patientDDURL, new GetPatientsDataRequest
                {
                    Context = "NG",
                    ContractNumber = contractNumber,
                    Version = version,
                    UserId = userId,
                    PatientIds = patientIds
                } as object);

            if (patientDDResponse != null && patientDDResponse.Patients != null)
            {
                data = patientDDResponse.Patients;
            }
            return data;
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
                    StartTime = request.ToDo.StartTime,
                    Duration = request.ToDo.Duration,
                    PatientId = request.ToDo.PatientId,
                    PriorityId = request.ToDo.PriorityId,
                    ProgramIds = request.ToDo.ProgramIds,
                    Title = request.ToDo.Title,
                    StatusId = request.ToDo.StatusId
                };
                if (request.ToDo.StatusId == (int)Phytel.API.DataDomain.Scheduling.DTO.Status.Met || request.ToDo.StatusId == (int)Phytel.API.DataDomain.Scheduling.DTO.Status.Abandoned)
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
                    ToDo toDo = convertToToDo(dataDomainResponse.ToDoData);
                    // Call Patient DD to get patient details.
                    getPatient(request.Version, request.ContractNumber, request.UserId, client, toDo);
                    response.ToDo = toDo;
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
                    StartTime = request.ToDo.StartTime,
                    Duration = request.ToDo.Duration,
                    PatientId = request.ToDo.PatientId,
                    PriorityId = request.ToDo.PriorityId,
                    ProgramIds = request.ToDo.ProgramIds,
                    Title = request.ToDo.Title,
                    StatusId = request.ToDo.StatusId,
                    DeleteFlag = request.ToDo.DeleteFlag,
                    ClosedDate = request.ToDo.ClosedDate
                };
                
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
                    ToDo toDo = convertToToDo(dataDomainResponse.ToDoData);
                    // Call Patient DD to get patient details.
                    getPatient(request.Version, request.ContractNumber, request.UserId, client, toDo);
                    response.ToDo = toDo;
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
                    StartTime = toDoData.StartTime,
                    Duration = toDoData.Duration,
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
