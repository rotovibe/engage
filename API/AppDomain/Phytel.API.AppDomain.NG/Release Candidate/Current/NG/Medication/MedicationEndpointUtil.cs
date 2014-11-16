﻿using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;

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
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientMedSupps()::" + ex.Message, ex.InnerException);
            }
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
                if (request.Insert)
                {
                    data.SystemName = Constants.SystemName;
                }
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
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:SavePatientMedSupp()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion
    }
}
