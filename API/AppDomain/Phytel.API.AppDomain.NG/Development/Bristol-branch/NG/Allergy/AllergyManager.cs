using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class AllergyManager : ManagerBase, IAllergyManager
    {
        public IAllergyEndpointUtil EndpointUtil { get; set; }
        public ISearchManager SearchManager { get; set; }

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
            catch (Exception ex) { throw ex; }
        }
        #endregion

        public void IndexResultSet(List<DTO.Allergy> result)
        {
            try
            {
                var searchDocs = new List<IdNamePair>();
                result.ForEach(a => searchDocs.Add(Mapper.Map<IdNamePair>(a)));
                //new AllergyLuceneStrategy<IdNamePair,IdNamePair>().AddUpdateLuceneIndex(searchDocs);
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
            catch (Exception ex) { throw ex; }
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
            catch (Exception ex) { throw ex; }
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
                            PostAllergyRequest req = new PostAllergyRequest
                            {
                                Allergy = new DTO.Allergy { Id = p.AllergyId, TypeIds = p.AllergyTypeIds, Name = p.AllergyName },
                                ContractNumber = request.ContractNumber,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            AllergyData allergyData = EndpointUtil.UpdateAllergy(req);
                            DTO.Allergy newAllergy = Mapper.Map<DTO.Allergy>(allergyData);
                            // Register newly initialized allergies in search index.
                            SearchManager.RegisterAllergyDocumentInSearchIndex(newAllergy, req.ContractNumber, request);
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
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientAllergy - Delete
        public void DeletePatientAllergy(DeletePatientAllergyRequest request)
        {
            try
            {
                EndpointUtil.DeletePatientAllergy(request);

            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
