using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemEndpointUtil : IPatientSystemEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        protected readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];
        #endregion

        public List<SystemData> GetSystems(IServiceContext context)
        {
            try
            {
                List<SystemData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/System", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/System",
                                    DDPatientSystemUrl,
                                    "NG",
                                    context.Version,
                                    context.Contract), context.UserId);

                GetSystemsDataResponse dataDomainResponse = client.Get<GetSystemsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.SystemsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientData> GetAllPatients(IServiceContext request)
        {
            try
            {
                List<PatientData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patients", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patients",
                                    DDPatientServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.Contract), request.UserId);

                GetAllPatientsDataResponse dataDomainResponse = client.Get<GetAllPatientsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        public List<PatientSystemData> GetPatientSystems(IServiceContext request, string patientId)
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
                                    request.Contract,
                                    patientId), request.UserId);

                GetPatientSystemsDataResponse dataDomainResponse = client.Get<GetPatientSystemsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystemData> InsertPatientSystems(IServiceContext context, string patientId)
        {
            List<PatientSystemData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems",
                    DDPatientSystemUrl,
                    "NG",
                    context.Version,
                    context.Contract,
                    patientId), context.UserId);

                List<PatientSystemData> data = new List<PatientSystemData>();

                var systemList = (List<DTO.PatientSystem>) context.Tag;
                if (systemList != null)
                    systemList.ForEach(a => data.Add(Mapper.Map<PatientSystemData>(a)));

                InsertPatientSystemsDataResponse dataDomainResponse = client.Post<InsertPatientSystemsDataResponse>(
                    url, new InsertPatientSystemsDataRequest
                    {
                        PatientId = patientId,
                        PatientSystemsData = data,
                        Context = "NG",
                        ContractNumber = context.Contract,
                        UserId = context.UserId,
                        Version = context.Version
                    } as object);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsData;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PatientSystemData> UpdatePatientSystems(IServiceContext context, string patientId)
        {
            List<PatientSystemData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    context.Version,
                                    context.Contract,
                                    patientId), context.UserId);

                List<PatientSystemData> data = new List<PatientSystemData>();

                // unpack patientsystems
                var systemList = (List<DTO.PatientSystem>)context.Tag;
                if (systemList != null)
                    systemList.ForEach(a => data.Add(Mapper.Map<PatientSystemData>(a)));

                UpdatePatientSystemsDataResponse dataDomainResponse = client.Put<UpdatePatientSystemsDataResponse>(url, new UpdatePatientSystemsDataRequest
                    {
                        PatientId = patientId,
                        PatientSystemsData = data,
                        Context = "NG",
                        ContractNumber = context.Contract,
                        UserId = context.UserId,
                        Version = context.Version
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


        public List<PatientSystemOldData> GetAllPatientSystems(IServiceContext context)
        {
            List<PatientSystemOldData> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystems",
                                    DDPatientSystemUrl,
                                    "NG",
                                    context.Version,
                                    context.Contract), context.UserId);

                GetAllPatientSystemDataResponse dataDomainResponse = client.Get<GetAllPatientSystemDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientSystemsOldData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
