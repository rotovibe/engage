using System;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemEndpointUtil : IPatientSystemEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        #endregion

        public List<SystemSourceData> GetSystemSources(GetActiveSystemSourcesRequest request)
        {
            try
            {
                List<SystemSourceData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/SystemSource", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/SystemSource",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetSystemSourcesDataResponse dataDomainResponse = client.Get<GetSystemSourcesDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.SystemSourcesData;
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
    }
}
