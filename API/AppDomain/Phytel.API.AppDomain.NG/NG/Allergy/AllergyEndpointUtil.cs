using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class AllergyEndpointUtil : IAllergyEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDAllergyUrl = ConfigurationManager.AppSettings["DDAllergyUrl"];
        #endregion

        #region Allergy
        public List<AllergyData> GetAllergies(GetAllergiesRequest request)
        {
            try
            {
                List<AllergyData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Allergy", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Allergy",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetAllAllergysResponse dataDomainResponse = client.Get<GetAllAllergysResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Allergys;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }

        public AllergyData InitializeAllergy(PostInitializeAllergyRequest request)
        {
            try
            {
                AllergyData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Version}/{ContractNumber}/Allergy/Initialize", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Allergy/Initialize",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                PutInitializeAllergyDataResponse dataDomainResponse = client.Put<PutInitializeAllergyDataResponse>(url, new PutInitializeAllergyDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    AllergyName = request.AllergyName,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.AllergyData;
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:InitializeAllergy()::" + ex.Message, ex.InnerException);
            }
        }

        public AllergyData UpdateAllergy(PostAllergyRequest request)
        {
            try
            {
                AllergyData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Allergy/Update", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Allergy/Update",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                if (request.Allergy != null)
                {
                    AllergyData data = new AllergyData();
                    data = Mapper.Map<AllergyData>(request.Allergy);
                    PutAllergyDataResponse dataDomainResponse = client.Put<PutAllergyDataResponse>(url, new PutAllergyDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        AllergyData = data
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        result = dataDomainResponse.AllergyData;
                    }
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdateAllergy()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion

        #region PatientAllergy - Gets
        public List<PatientAllergyData> GetPatientAllergies(GetPatientAllergiesRequest request)
        {
            try
            {
                List<PatientAllergyData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{PatientId}", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                GetPatientAllergiesDataResponse dataDomainResponse = client.Post<GetPatientAllergiesDataResponse>(url, new GetPatientAllergiesDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    StatusIds = request.StatusIds,
                    TypeIds = request.TypeIds,
                    PatientId = request.PatientId,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientAllergiesData;
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientAllergies()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region PatientAllergy - Post
        public PatientAllergyData InitializePatientAllergy(PostInitializePatientAllergyRequest request)
        {
            try
            {
                PatientAllergyData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Version}/{ContractNumber}/PatientAllergy/Initialize", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Initialize",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                PutInitializePatientAllergyDataResponse dataDomainResponse = client.Put<PutInitializePatientAllergyDataResponse>(url, new PutInitializePatientAllergyDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    AllergyId = request.AllergyId,
                    SystemName = Constants.SystemName,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientAllergyData;
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:InitializePatientAllergy()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientAllergyData> UpdatePatientAllergies(PostPatientAllergiesRequest request)
        {
            try
            {
                List<PatientAllergyData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Update/Bulk", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Update/Bulk",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                if (request.PatientAllergies != null && request.PatientAllergies.Count > 0)
                {
                    List<PatientAllergyData> data = new List<PatientAllergyData>();
                    request.PatientAllergies.ForEach(a => data.Add(Mapper.Map<PatientAllergyData>(a)));
                    PutPatientAllergiesDataResponse dataDomainResponse = client.Put<PutPatientAllergiesDataResponse>(url, new PutPatientAllergiesDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        PatientAllergiesData = data
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        result = dataDomainResponse.PatientAllergiesData;
                    }
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdatePatientAllergies()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region PatientAllergy - Delete
        public void DeletePatientAllergy(DeletePatientAllergyRequest request)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{Id}", "DELETE")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.Id), request.UserId);
                DeletePatientAllergyDataResponse dataDomainResponse = client.Delete<DeletePatientAllergyDataResponse>(url);
            }
            catch (WebServiceException ex) { throw ex; }
        } 
        #endregion
    }
}
