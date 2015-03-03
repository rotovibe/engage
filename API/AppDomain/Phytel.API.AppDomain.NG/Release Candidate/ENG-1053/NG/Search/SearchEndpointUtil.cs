using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using MedicationMap = Phytel.API.AppDomain.NG.DTO.MedicationMap;

namespace Phytel.API.AppDomain.NG.Search
{
    public class SearchEndpointUtil : ISearchEndpointUtil
    {
        static readonly string _ddMedicationServiceUrl = ConfigurationManager.AppSettings["DDMedicationUrl"];

        //GetTermSearchResults
        public List<TextValuePair> GetTermSearchResults(string term)
        {
            try
            {
                List<TextValuePair> result = new List<TextValuePair>();

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
