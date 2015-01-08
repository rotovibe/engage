using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using System;

namespace Phytel.API.AppDomain.NG.Medication
{
    public class MedicationEndpointUtil : IMedicationEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDMedicationUrl = ConfigurationManager.AppSettings["DDMedicationUrl"];
        #endregion

        #region Medication - Posts
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

                string strength = string.Empty;
                string unit = string.Empty;
                if (!string.IsNullOrEmpty(request.PatientMedSupp.Strength))
                {
                    string[] values = request.PatientMedSupp.Strength.Split(' ');
                    if(values.Length > 0)
                    { 
                        strength = values[0];
                        if (values.Length > 1)
                        {
                            unit = values[1].ToUpper();// Medication collection store all units in upper case.
                        }
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
                    Route = request.PatientMedSupp.Route,
                    Unit = unit
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

        public MedicationData InitializeMedSupp(PostInitializeMedSuppRequest request)
        {
            try
            {
                MedicationData result = null;
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/MedSupp/Initialize", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedSupp/Initialize",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                PutInitializeMedSuppDataResponse dataDomainResponse = client.Put<PutInitializeMedSuppDataResponse>(url, new PutInitializeMedSuppDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    MedSuppName = request.MedSuppName,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.MedicationData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public MedicationData UpdateMedication(PostMedicationRequest request)
        {
            try
            {
                MedicationData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Medication/Update", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Medication/Update",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                if (request.Medication != null)
                {
                    MedicationData data = new MedicationData();
                    data = Mapper.Map<MedicationData>(request.Medication);
                    PutMedicationDataResponse dataDomainResponse = client.Put<PutMedicationDataResponse>(url, new PutMedicationDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        MedicationData = data
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        result = dataDomainResponse.MedicationData;
                    }
                }
                return result;
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

        public PatientMedSuppData InitializePatientMedSupp(PostInitializePatientMedSuppRequest request)
        {
            try
            {
                PatientMedSuppData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Initialize", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Initialize",
                                    DDMedicationUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                PutInitializePatientMedSuppDataResponse dataDomainResponse = client.Put<PutInitializePatientMedSuppDataResponse>(url, new PutInitializePatientMedSuppDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    MedSuppId = request.MedSuppId,
                    CategoryId = request.CategoryId,
                    TypeId = request.TypeId,
                    SystemName = Constants.SystemName,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientMedSuppData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientMedSuppData UpdatePatientMedSupp(PostPatientMedSuppRequest request)
        {
            try
            {
                PatientMedSuppData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Update", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Update",
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
                    PatientMedSuppData = data
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
    }
}
