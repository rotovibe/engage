using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemEndpointUtil : IPatientSystemEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        protected readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        #endregion

        public List<SystemData> GetSystems(GetActiveSystemsRequest request)
        {
            try
            {
                List<SystemData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/System", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/System",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetSystemsDataResponse dataDomainResponse = client.Get<GetSystemsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.SystemsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<Phytel.API.DataDomain.Patient.DTO.PatientData> GetAllPatients(UpdatePatientsAndSystemsRequest request)
        {
            try
            {
                List<Phytel.API.DataDomain.Patient.DTO.PatientData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patients", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patients",
                                    DDPatientServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                Phytel.API.DataDomain.Patient.DTO.GetAllPatientsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.Patient.DTO.GetAllPatientsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        public List<PatientSystemData> GetPatientSystems(GetPatientSystemsRequest request)
        {
            List<PatientSystemData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                GetPatientSystemsDataResponse dataDomainResponse = client.Get<GetPatientSystemsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystemData> InsertPatientSystems(InsertPatientSystemsRequest request)
        {
            List<PatientSystemData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                List<PatientSystemData> data = new List<PatientSystemData>();
                request.PatientSystems.ForEach(a => data.Add(Mapper.Map<PatientSystemData>(a)));

                InsertPatientSystemsDataResponse dataDomainResponse = client.Post<InsertPatientSystemsDataResponse>(url, new InsertPatientSystemsDataRequest 
                    { 
                        PatientId  = request.PatientId,
                        PatientSystemsData = data,
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsRequest request)
        {
            List<PatientSystemData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                List<PatientSystemData> data = new List<PatientSystemData>();
                request.PatientSystems.ForEach(a => data.Add(Mapper.Map<PatientSystemData>(a)));

                UpdatePatientSystemsDataResponse dataDomainResponse = client.Put<UpdatePatientSystemsDataResponse>(url, new UpdatePatientSystemsDataRequest 
                    { 
                        PatientId  = request.PatientId,
                        PatientSystemsData = data,
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeletePatientSystems(DeletePatientSystemsRequest request)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems/{Ids}", "DELETE")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems/{5}",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId,
                                    request.Ids), request.UserId);

                DeletePatientSystemsDataResponse dataDomainResponse = client.Delete<DeletePatientSystemsDataResponse>(url);
            }
            catch (Exception ex) { throw ex; }
        }


        public List<PatientSystemOldData> GetAllPatientSystems(UpdatePatientsAndSystemsRequest request)
        {
            List<PatientSystemOldData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetAllPatientSystemDataResponse dataDomainResponse = client.Get<GetAllPatientSystemDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsOldData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<string> InsertEngagePatientSystems(InsertEngagePatientSystemsDataRequest request)
        {
            List<string> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/EngagePatientSystems", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/EngagePatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                InsertEngagePatientSystemsDataResponse dataDomainResponse = client.Post<InsertEngagePatientSystemsDataResponse>(url, request as object);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Ids;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
