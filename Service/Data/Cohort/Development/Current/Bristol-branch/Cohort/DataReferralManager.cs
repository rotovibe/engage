using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace Phytel.API.DataDomain.Cohort
{
    public class DataReferralManager : IDataReferralManager
    {
        private readonly IServiceContext _context;
        private readonly IReferralRepository<IDataDomainRequest> _repository;
        
        public DataReferralManager(IServiceContext context, IReferralRepository<IDataDomainRequest> repository)
        {
            _context = context;
            _repository = repository;
        }

        public string InsertReferral(ReferralData request)
        {
            try
            {
         
                if ((request == null))
                    throw new ArgumentNullException("Request parameter referral cannot be NULL");
                if (string.IsNullOrEmpty(request.CohortId ))
                    throw new ArgumentNullException("Request parameter referral.cohortId cannot be NULL/EMPTY");
                if (string.IsNullOrEmpty(request.Name))
                    throw new ArgumentNullException("Request parameter referral.name cannot be NULL/EMPTY");
                if (string.IsNullOrEmpty(request.DataSource))
                    throw new ArgumentNullException("Request parameter referral.datasource cannot be NULL/EMPTY");

                return _repository.Insert(request, _context.Version, _context.UserId) as string;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public PostPatientsListReferralDefinitionResponse InsertReferralsAll(PostPatientsListReferralDefinitionRequest request)
        {
            PostPatientsListReferralDefinitionResponse patientListReferralResp = new PostPatientsListReferralDefinitionResponse();
            patientListReferralResp.ExistingPatientIds = new List<string>();
            patientListReferralResp.NewPatientIds = new List<string>();
            patientListReferralResp.ResponseStatus = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus();
           ReferralData patientRefDataRqst = null;
            PostPatientReferralDefinitionResponse patientRefDefResp = null;

            try
            {
                using (CohortMongoContext ctx = new CohortMongoContext(request.ContractNumber))
                {
                    int indx = -1;
                    foreach (PatientReferralsListEntityData pDE in request.PatientsReferralsList)
                    {
                        // Attempt to locate patient in Patient collection using 
                        // inbound datasource and externalId
                        // by associating using the externalId <-> Referral 'cid'
                        // and by associating the datasource <-> Referral 'src'
                        bool found = false;
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        indx += 1;
                        queries.Add(Query.EQ(MEReferral.CohortIdProperty, request.PatientsReferralsList[indx].ExternalId));
                        queries.Add(Query.EQ(MEReferral.DataSourceProperty, request.PatientsReferralsList[indx].DataSource));
                        IMongoQuery mQuery = Query.And(queries);
                        MEReferral meReferral = ctx.Referrals.Collection.Find(mQuery).FirstOrDefault();

                        if(meReferral != null)
                        {
                            if(meReferral.CohortId == request.PatientsReferralsList[indx].ExternalId)
                            {
                                found = true;    
                            }
                        }
                      
                        if (found)
                        {
                            // if found, retrieve the existing patient id + assign to 'existingPatientIds'
                            patientListReferralResp.ExistingPatientIds.Add(meReferral.CohortId);
                            continue;
                        }
                        else
                        {
                            // Create a new  ReferralData request object 
                              patientRefDataRqst = new ReferralData();
                              patientRefDataRqst.CohortId = pDE.ExternalId;
                              patientRefDataRqst.DataSource = pDE.DataSource;
                              patientRefDataRqst.CreatedBy = pDE.CreatedBy;
                              patientRefDataRqst.Name = pDE.Name;
                              patientRefDataRqst.Description = request.Description;
                              patientRefDataRqst.Reason = request.Reason;

                            // Call InsertReferral
                            string  newReferral = this.InsertReferral(patientRefDataRqst);
                            patientListReferralResp.NewPatientIds.Add(newReferral);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Combine the list by adding the patientId that were inserted to the list of the existing
            // Assign the final list to the list in the response object ("patientListReferralResp");
            return patientListReferralResp;
        }            // end method definition
        public ReferralData GetReferralById(string referralId)
        {
            try
            {
                if (string.IsNullOrEmpty(referralId))
                    throw new ArgumentNullException("Request parameter ReferralID cannot be NULL/EMPTY");
                return _repository.FindByID(referralId) as ReferralData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ReferralData> GetAllReferrals()
        {
            try
            {
                return _repository.SelectAll() as List<ReferralData>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
  
