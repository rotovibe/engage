using System;
using System.CodeDom;
using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using System.Collections.Generic;
using AppDomain.Engage.Population.DTO.Demographics;
using AppDomain.Engage.Population.DTO.Referrals;

namespace AppDomain.Engage.Population
{
    public class DemographicsManager : IDemographicsManager
    {
        private readonly IServiceContext _context;
        private readonly IPatientDataDomainClient _client;
        public UserContext UserContext { get; set; }

        

        public DemographicsManager(IServiceContext context, IPatientDataDomainClient client,UserContext userContext)
        {
            _context = context;
            _client = client;
            UserContext = userContext;
        }

        public PostReferralWithPatientsListResponse InsertBulkPatients(List<Patient> patientslist)
        {
            try
            {


                if (patientslist == null || patientslist.Count == 0)
                {
                    throw new ArgumentNullException("patientslist","Request cannot be null/empty");
                }
                else
                {

                    foreach (Patient patient in patientslist)
                    {
                        if (String.IsNullOrEmpty(patient.ExternalRecordId))
                            throw new ArgumentNullException("ExternalRecordId",
                                string.Concat(
                                    "Request parameter ExternalRecordId value  cannot be NULL/EMPTY for patient ",
                                    patient.ExternalRecordId, " - Request failed"
                                    ));
                        if (String.IsNullOrEmpty(patient.FirstName))
                            throw new ArgumentNullException(
                                string.Concat("Request parameter FirstName value cannot be NULL/EMPTY for patient ",
                                    patient.ExternalRecordId, " - Request failed"
                                    ));
                        if (String.IsNullOrEmpty(patient.LastName))
                            throw new ArgumentNullException(
                                string.Concat("Request parameter LastName value cannot be NULL/EMPTY for patient ",
                                    patient.ExternalRecordId, "- Request failed"
                                    ));
                        if (String.IsNullOrEmpty(patient.DOB))
                            throw new ArgumentNullException("DOB",
                                string.Concat("Request parameter DOB value cannot be NULL/EMPTY for patient ",
                                    patient.ExternalRecordId, " - Request failed"
                                    ));
                        
                        if (String.IsNullOrEmpty(patient.DataSource))
                            throw new ArgumentNullException("Datasource",
                                string.Concat("Request parameter Datasource value  cannot be NULL/EMPTY for patient ",
                                    patient.ExternalRecordId, " - Request failed"
                                    ));

                    }
                    return _client.PostPatientsListDetails(patientslist, UserContext);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        
        //example implementation

    }
}