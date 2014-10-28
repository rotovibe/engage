﻿using Phytel.API.AppDomain.NG.DTO;
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

        public DdAllergy PutNewAllergy(PostInsertNewAllergyRequest request)
        {
            try
            {
                DdAllergy result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Allergy", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Allergy",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                PostNewAllergyResponse dataDomainResponse = client.Put<PostNewAllergyResponse>(url,
                    new PostNewAllergyRequest
                    {
                        Context = request.Context,
                        Description = request.Name,
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version
                    });

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Allergy;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }

        public List<DdAllergy> GetAllergies(GetAllergiesRequest request)
        {
            try
            {
                List<DdAllergy> result = null;
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

                GetPatientAllergiesDataResponse dataDomainResponse = client.Get<GetPatientAllergiesDataResponse>(url);

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

        public PatientAllergyData InitializePatientAllergy(GetInitializePatientAllergyRequest request)
        {
            try
            {
                PatientAllergyData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{PatientId}/Initialize", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}/Initialize",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                PutInitializePatientAllergyDataResponse dataDomainResponse = client.Put<PutInitializePatientAllergyDataResponse>(url, new PutInitializePatientAllergyDataRequest 
                { 
                    Context  = "NG",
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
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
        #endregion

        #region PatientAllergy - Post
        public List<PatientAllergyData> BulkUpdatePatientAllergies(PostPatientAllergiesRequest request)
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
                throw new WebServiceException("AD:BulkUpdatePatientAllergies()::" + ex.Message, ex.InnerException);
            }
        }


        public PatientAllergyData SingleUpdatePatientAllergy(PostPatientAllergyRequest request)
        {
            try
            {
                PatientAllergyData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Update/Single", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Update/Single",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                if (request.PatientAllergy != null)
                {
                    PatientAllergyData data = new PatientAllergyData();
                    data = Mapper.Map<PatientAllergyData>(request.PatientAllergy);
                    PutPatientAllergyDataResponse dataDomainResponse = client.Put<PutPatientAllergyDataResponse>(url, new PutPatientAllergyDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version,
                        PatientAllergyData  = data
                    } as object);

                    if (dataDomainResponse != null)
                    {
                        result = dataDomainResponse.PatientAllergyData;
                    }
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:SingleUpdatePatientAllergy()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion
    }
}
