using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Search.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using MedicationMap = Phytel.API.AppDomain.NG.DTO.MedicationMap;
using TextValuePair = Phytel.API.AppDomain.NG.DTO.Search.TextValuePair;

namespace Phytel.API.AppDomain.NG.Search
{
    public class SearchEndpointUtil : ISearchEndpointUtil
    {
        static readonly string _ddMedicationServiceUrl = ConfigurationManager.AppSettings["DDMedicationUrl"];
        static readonly string _ddSearchServiceUrl = ConfigurationManager.AppSettings["DDSearchUrl"];


        public void RegisterMedDocument(IAppDomainRequest e, MedNameSearchDoc nfp)
        {
            try
            {
                Mapper.CreateMap<MedNameSearchDoc, MedNameSearchDocData>();
                var data = Mapper.Map<MedNameSearchDocData>(nfp);

                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Search/MedicationIndex/Insert", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Search/MedicationIndex/Insert",
                                   _ddSearchServiceUrl,
                                   "NG",
                                   e.Version,
                                   e.ContractNumber), e.UserId);

                client.Put<PutMedRegistrationResponse>(url,
                    new PutMedRegistrationRequest
                    {
                        Context = "NG",
                        ContractNumber = e.ContractNumber,
                        MedDocument = data,
                        UserId = e.UserId,
                        Version = e.Version
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SearchEndpointUtil:RegisterMedDocument()::" + ex.Message, ex.InnerException);
            }
        }

        public void RegisterAllergyDocument(IAppDomainRequest e, IdNamePair allergy)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Search/AllergyIndex/Insert", "PUT")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Search/AllergyIndex/Insert",
                                   _ddSearchServiceUrl,
                                   "NG",
                                   e.Version,
                                   e.ContractNumber), e.UserId);

                client.Put<PutAllergyRegistrationResponse>(url,
                    new PutAllergyRegistrationRequest
                    {
                        Context = "NG",
                        ContractNumber = e.ContractNumber,
                        AllergyDocument = allergy,
                        UserId = e.UserId,
                        Version = e.Version
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SearchEndpointUtil:RegisterMedDocument()::" + ex.Message, ex.InnerException);
            }
        }

        public List<object> GetTermSearchResults(IAppDomainRequest e, SearchEnum type, string term)
        {
            string urlTest = null;

            try
            {
                List<object> result = new List<object>();

                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Search/{Type}/{Term}", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Search/{4}/",
                                   _ddSearchServiceUrl,
                                   "NG",
                                   e.Version,
                                   e.ContractNumber,
                                   type), e.UserId);

                url = url + "&Term=" + term;

                GetSearchResponse dataDomainResponse = client.Get<GetSearchResponse>(url);

                switch (type)
                {
                    case SearchEnum.Medication:
                    {
                        if (dataDomainResponse.MedResults != null)
                        {
                            result = dataDomainResponse.MedResults.ConvertAll(s =>
                                new TextValuePair {Text = s.Text, Value = s.Value}).ToList<object>();
                        }
                        break;
                    }
                    case SearchEnum.Allergy:
                    {
                        if (dataDomainResponse.AllergyResults != null)
                        {
                            try
                            {
                                result = dataDomainResponse.AllergyResults.ToList<object>();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("AllergyResults tolist() failed.", ex.InnerException);
                            }
                        }
                        break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SearchEndpointUtil:GetTermSearchResults():: url:" + urlTest+ " message:" + ex.Message + " trace:" + ex.StackTrace, ex.InnerException);
            }
        }

        public List<MedicationMap> GetMedicationMapsByName(GetMedFieldsRequest e, string userId)
        {
            try
            {
                List<MedicationMap> result = new List<MedicationMap>();

                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/MedicationMap", "POST")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/MedicationMap",
                                   _ddMedicationServiceUrl,
                                   "NG",
                                   e.Version,
                                   e.ContractNumber,
                                   e.Name), userId);

                GetMedicationMapDataResponse dataDomainResponse = client.Post<GetMedicationMapDataResponse>(url, new GetMedicationMapDataRequest
                {
                    Context = "NG",
                    ContractNumber = e.ContractNumber,
                    Name = e.Name,
                    UserId = e.UserId,
                    Version = e.Version
                } as object);

                if (dataDomainResponse.MedicationMapsData != null)
                {
                    result.AddRange(dataDomainResponse.MedicationMapsData.Select(AutoMapper.Mapper.Map<MedicationMap>));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SearchEndpointUtil:GetMedicationMapsByName()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
