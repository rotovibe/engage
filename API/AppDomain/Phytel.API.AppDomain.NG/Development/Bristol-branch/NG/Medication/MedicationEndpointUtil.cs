using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using System;
using System.Linq;

namespace Phytel.API.AppDomain.NG.Medication
{
    public class MedicationEndpointUtil : IMedicationEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDMedicationUrl = ConfigurationManager.AppSettings["DDMedicationUrl"];
        #endregion

        #region MedicationMap
        public List<string> GetMedicationNDCs(PostPatientMedSuppRequest request)
        {
            try
            {
                List<string> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Medication/Search", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Medication/Search",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                string strength = null;
                List<string> strList = null;
                if (!string.IsNullOrEmpty(request.PatientMedSupp.Strength))
                {
                    string[] combined = request.PatientMedSupp.Strength.Split(';');
                    if (combined.Length > 0)
                    {
                        strList = new List<string>();
                        foreach (string s in combined)
                        {
                            string[] str = s.Trim().Split(' ');
                            if (str.Length > 0)
                            {
                                strList.Add(str[0]);
                            }
                        };
                        strength = string.Join("; ", strList);
                    }
                }

                GetMedicationNDCsDataResponse dataDomainResponse = client.Post<GetMedicationNDCsDataResponse>(url, new GetMedicationNDCsDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version,
                    Name = request.PatientMedSupp.Name,
                    Strength = strength,
                    Form = request.PatientMedSupp.Form,
                    Route = request.PatientMedSupp.Route
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.NDCcodes;
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetMedicationDetails()::" + ex.Message, ex.InnerException);
            }
        }

        public MedicationMapData InitializeMedicationMap(PostInitializeMedicationMapRequest request)
        {
            try
            {
                MedicationMapData result = null;
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/Initialize", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap/Initialize",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                MedicationMapData data = new MedicationMapData();
                data = Mapper.Map<MedicationMapData>(request.MedicationMap);
                PutInitializeMedicationMapDataResponse dataDomainResponse = client.Put<PutInitializeMedicationMapDataResponse>(url, new PutInitializeMedicationMapDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    MedicationMapData = data,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.MedicationMappingData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public MedicationMapData UpdateMedicationMap(PutMedicationMapRequest request)
        {
            try
            {
                MedicationMapData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/Update", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap/Update",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                if (request.MedicationMap != null)
                {
                    MedicationMapData data = new MedicationMapData();
                    data = Mapper.Map<MedicationMapData>(request.MedicationMap);
                    PutMedicationMapDataResponse dataDomainResponse = client.Put<PutMedicationMapDataResponse>(url, new PutMedicationMapDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        MedicationMapData = data
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        result = dataDomainResponse.MedicationMappingData;
                    }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }


        public MedicationMapData InsertMedicationMap(PostMedicationMapRequest request)
        {
            try
            {
                MedicationMapData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/Insert", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap/Insert",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                if (request.MedicationMap != null)
                {
                    MedicationMapData data = new MedicationMapData();
                    data = Mapper.Map<MedicationMapData>(request.MedicationMap);
                    PostMedicationMapDataResponse dataDomainResponse = client.Post<PostMedicationMapDataResponse>(url, new PostMedicationMapDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        MedicationMapData = data
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        result = dataDomainResponse.MedMapData;
                    }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<MedicationMapData> DeleteMedicationMap(PutDeleteMedMapRequest request)
        {
            try
            {
                List<MedicationMapData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/Delete", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap/Delete",
                    DDMedicationUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber), request.UserId);

                PutDeleteMedMapDataResponse response = client.Put<PutDeleteMedMapDataResponse>(url,
                    new PutDeleteMedMapDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        MedicationMaps = request.MedicationMaps.Select(
                            map => new MedicationMapData
                            {
                                FullName = map.FullName,
                                Route = map.Route,
                                SubstanceName = map.SubstanceName,
                                Strength = map.Strength,
                                Form = map.Form
                            }).ToList(),
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);

                if (response != null)
                {
                    result = response.MedicationMapsData;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicationMapData> SearchMedicationMap(GetMedicationMapsRequest request)
        {
            try
            {
                List<MedicationMapData> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap", "POST")]
                IRestClient client = new JsonServiceClient();
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap",
                                   DDMedicationUrl,
                                   "NG",
                                   request.Version,
                                   request.ContractNumber), request.UserId);

                GetMedicationMapDataResponse dataDomainResponse = client.Post<GetMedicationMapDataResponse>(url, new GetMedicationMapDataRequest { 
                     Context = "NG",
                     ContractNumber = request.ContractNumber,
                     Form = request.Form,
                     Name = request.Name,
                     Route = request.Route,
                     Strength = request.Strength,
                     Type  = request.Type,
                     Skip = request.Skip,
                     Take = request.Take,
                     UserId = request.UserId,
                     Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.MedicationMapsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteMedicationMaps(DeleteMedicationMapsRequest request)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/{Ids}", "DELETE")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap/{4}",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.Ids), request.UserId);

                DeleteMedicationMapsDataResponse dataDomainResponse = client.Delete<DeleteMedicationMapsDataResponse>(url);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientMedSupps - Gets
        public int GetPatientMedSuppsCount(GetPatientMedSuppsCountRequest request)
        {
            int count = 0;
            try
            {
                IRestClient client = new JsonServiceClient();
                string name = string.IsNullOrEmpty(request.Name) ? null : request.Name.ToUpper();
                string form = string.IsNullOrEmpty(request.Form) ? null : request.Form.ToUpper();
                string route = string.IsNullOrEmpty(request.Route) ? null : request.Route.ToUpper();;
                string strength = request.Strength;
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp?Name={4}&Form={5}&Route={6}&Strength={7}",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    name,
                                    form,
                                    route,
                                    strength), request.UserId);

                GetPatientMedSuppsCountDataResponse dataDomainResponse = client.Get<GetPatientMedSuppsCountDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    count = dataDomainResponse.PatientCount;
                }
                return count;
            }
            catch (Exception ex) { throw ex; }
        } 
        #endregion

        #region PatientMedSupps - Posts
        public List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsRequest request)
        {
            try
            {
                List<PatientMedSuppData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/{PatientId}", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/{4}",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                GetPatientMedSuppsDataResponse dataDomainResponse = client.Post<GetPatientMedSuppsDataResponse>(url, new GetPatientMedSuppsDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    StatusIds = request.StatusIds,
                    CategoryIds = request.CategoryIds,
                    PatientId = request.PatientId,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientMedSuppsData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientMedSuppData SavePatientMedSupp(PostPatientMedSuppRequest request)
        {
            try
            {
                PatientMedSuppData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Save", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Save",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);
                PatientMedSuppData data = Mapper.Map<PatientMedSuppData>(request.PatientMedSupp);
                PutPatientMedSuppDataResponse dataDomainResponse = client.Put<PutPatientMedSuppDataResponse>(url, new PutPatientMedSuppDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version,
                    PatientMedSuppData = data,
                    Insert = request.Insert
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientMedSuppData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientMedSupp - Delete
        public void DeletePatientMedSupp(DeletePatientMedSuppRequest request)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/{Id}", "DELETE")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/{4}",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.Id), request.UserId);
                DeletePatientMedSuppDataResponse dataDomainResponse = client.Delete<DeletePatientMedSuppDataResponse>(url);
            }
            catch (WebServiceException ex) { throw ex; }
        }
        #endregion

        #region PatientMedFrequency - Posts
        public List<PatientMedFrequencyData> GetPatientMedFrequencies(GetPatientMedFrequenciesRequest request)
        {
            try
            {
                List<PatientMedFrequencyData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Frequency/{PatientId}", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Frequency/{4}",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);
    
                GetPatientMedFrequenciesDataResponse dataDomainResponse = client.Get<GetPatientMedFrequenciesDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientMedFrequenciesData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public string InsertPatientMedFrequency(PostPatientMedFrequencyRequest request)
        {
            try
            {
                string id = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Frequency/Insert", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Frequency/Insert",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);
                if (request.PatientMedFrequency != null)
                {


                    PatientMedFrequencyData data = new PatientMedFrequencyData
                    {
                        Name = request.PatientMedFrequency.Name,
                        PatientId = request.PatientMedFrequency.PatientId
                    };
                    PostPatientMedFrequencyDataResponse dataDomainResponse = client.Post<PostPatientMedFrequencyDataResponse>(url, new PostPatientMedFrequencyDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        PatientMedFrequencyData = data,
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        id = dataDomainResponse.Id;
                    }
                }
                return id;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
