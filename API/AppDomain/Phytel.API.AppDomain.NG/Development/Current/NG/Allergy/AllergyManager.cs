using AutoMapper;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class AllergyManager : ManagerBase, IAllergyManager
    {
        public IAllergyEndpointUtil EndpointUtil { get; set; }
        public ISearchManager SearchManager { get; set; }

        public DTO.Allergy PutNewAllergy(PostInsertNewAllergyRequest request)
        {
            try
            {
                DTO.Allergy result = null;
                var algy = EndpointUtil.PutNewAllergy(request);
                result = Mapper.Map<DTO.Allergy>(algy);
                SearchManager.RegisterDocumentInSearchIndex(result, request.ContractNumber);
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }

        public List<DTO.Allergy> GetAllergies(GetAllergiesRequest request)
        {
            try
            {
                List<DTO.Allergy> result = new List<DTO.Allergy>();
                var algy = EndpointUtil.GetAllergies(request);
                algy.ForEach(a => result.Add(Mapper.Map<DTO.Allergy>(a)));
                IndexResultSet(result);
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }

        #region Allergy - Posts
        public DTO.Allergy InitializeAllergy(PostInitializeAllergyRequest request)
        {
            DTO.Allergy allergy = null;
            try
            {

                AllergyData data = EndpointUtil.InitializeAllergy(request);
                if (data != null)
                {
                    allergy = Mapper.Map<DTO.Allergy>(data);
                }
                return allergy;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:InitializeAllergy()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        public void IndexResultSet(List<DTO.Allergy> result)
        {
            try
            {
                var searchDocs = new List<IdNamePair>();
                result.ForEach(a => searchDocs.Add(Mapper.Map<IdNamePair>(a)));
                new AllergyLuceneStrategy<IdNamePair,IdNamePair>().AddUpdateLuceneIndex(searchDocs);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:IndexResultSet()::" + ex.Message, ex.InnerException);
            }
        }

        #region PatientAllergy - Gets
        public List<PatientAllergy> GetPatientAllergies(GetPatientAllergiesRequest request)
        {
            List<PatientAllergy> patientAllergies = null;
            try
            {
                List<PatientAllergyData> data = EndpointUtil.GetPatientAllergies(request);
                if (data != null && data.Count > 0)
                {
                    patientAllergies = new List<PatientAllergy>();
                    data.ForEach(a => patientAllergies.Add(Mapper.Map<PatientAllergy>(a)));
                }
                return patientAllergies;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientAllergies()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region PatientAllergy - Posts
        public PatientAllergy InitializePatientAllergy(PostInitializePatientAllergyRequest request)
        {
            PatientAllergy patientAllergy = null;
            try
            {

                PatientAllergyData data = EndpointUtil.InitializePatientAllergy(request);
                if (data != null)
                {
                    patientAllergy = Mapper.Map<PatientAllergy>(data);
                }
                return patientAllergy;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:InitializePatientAllergy()::" + ex.Message, ex.InnerException);
            }
        } 

        public List<PatientAllergy> UpdatePatientAllergies(PostPatientAllergiesRequest request)
        {
            List<PatientAllergy> patientAllergies = null;
            try
            {
                // Update Allergy collection for any newly initialized allergies & then register in search index.
                if(request.PatientAllergies != null && request.PatientAllergies.Count > 0)
                {
                    request.PatientAllergies.ForEach(p =>
                    { 
                        if(p.IsNewAllergy)
                        {
                            PostAllergyRequest req = new DTO.PostAllergyRequest
                            {
                                Allergy = new DTO.Allergy { Id = p.AllergyId, TypeIds = p.AllergyTypeIds, Name = p.AllergyName },
                                ContractNumber = request.ContractNumber,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            AllergyData allergyData = EndpointUtil.UpdateAllergy(req);
                            DTO.Allergy newAllergy = Mapper.Map<DTO.Allergy>(allergyData);
                            // Register newly initialized allergies in search index.
                            SearchManager.RegisterDocumentInSearchIndex(newAllergy, req.ContractNumber);
                        }
                    });
                }
                List<PatientAllergyData> data = EndpointUtil.UpdatePatientAllergies(request);
                if (data != null && data.Count > 0)
                {
                    patientAllergies = new List<PatientAllergy>();
                    data.ForEach(a => patientAllergies.Add(Mapper.Map<PatientAllergy>(a)));
                }
                return patientAllergies;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdatePatientAllergies()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion
    }
}
