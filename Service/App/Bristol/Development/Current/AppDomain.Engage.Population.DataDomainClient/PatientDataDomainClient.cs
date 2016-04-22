using System;
using System.Collections.Generic;
using System.Net;
using AppDomain.Engage.Population.DTO.Context;

using AppDomain.Engage.Population.DTO.Demographics;
using AutoMapper;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;

using ServiceStack.Service;
using PostPatientReferralDefinitionRequest = Phytel.API.DataDomain.Cohort.DTO.Referrals.PostPatientReferralDefinitionRequest;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public class PatientDataDomainClient : IPatientDataDomainClient
    {
        private readonly string _domainUrl;
        private readonly IHelpers _urlHelper;
        private readonly IRestClient _client;
        private readonly IServiceContext _context;
        protected readonly IMappingEngine _mappingEngine;
        public UserContext UserContext { get; set; }

        public PatientDataDomainClient(IMappingEngine mappingEngine, string domainUrl, IHelpers urlHelper, IRestClient client, IServiceContext context)
        {
            _mappingEngine = mappingEngine;
            _domainUrl = domainUrl;
            _urlHelper = urlHelper;
            _client = client;
            _context = context;
        }

       

       
        public ProcessedPatientsList PostPatientsListDetails(List<Patient> patientDataList,UserContext usercontext)
        {
            List<PatientData> ddPatientData = new List<PatientData>();
            ProcessedPatientsList adPatientData = new ProcessedPatientsList();
            List<ProcessedData> addedPatients = new List<ProcessedData>();
            List<ProcessedData> erroredPatients = new List<ProcessedData>();

            try
            {

                

                foreach (Patient e in patientDataList)
                {
                    ddPatientData.Add(_mappingEngine.Map<PatientData>(e));
                }
                // List<PatientData> ddPatientData = _mappingEgine.Map<List<Patient>, List<PatientData>>(adPatientData);


                var url = _urlHelper.BuildURL(string.Format("{0}/{1}/{2}/{3}/{4}/Batch/Patients",
                    _domainUrl,
                    "api",
                    "NG",
                    _context.Version,
                    _context.Contract), usercontext.UserId);

                var response = _client.Post<InsertBatchPatientsDataResponse>(url, new InsertBatchPatientsDataRequest
                {
                    PatientsData = ddPatientData,
                    Context = "NG",
                    ContractNumber = _context.Contract,
                    Version = _context.Version,
                    UserId = usercontext.UserId
                } );
               
                //if (response.Status.ErrorCode == HttpStatusCode.OK.ToString())
                //{
                    foreach (HttpObjectResponse<PatientData> e in response.ErrorMessages)
                    {
                        if (e.Code == System.Net.HttpStatusCode.InternalServerError)
                        {

                        erroredPatients.Add(_mappingEngine.Map<ProcessedData>(e.Body));

                        }
                        
                    }

                foreach (AppData e in response.Responses)
                {
                    addedPatients.Add(_mappingEngine.Map<ProcessedData>(e));
                }

                adPatientData.InsertedPatients = addedPatients;
                    adPatientData.ErroredPatients = erroredPatients;
                //}

                //else
                //{
                //    throw new Exception();
                //}

                return adPatientData;


            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:PostPatientDetails()::" + ex.Message, ex.InnerException);
            }

            


        }


        
       
    }
}
