using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public class MedicationManager : ManagerBase, IMedicationManager
    {
        public IMedicationEndpointUtil EndpointUtil { get; set; }
        public ISearchManager SearchManager { get; set; }


        #region MedicationMap - Posts
        public DTO.MedicationMap InitializeMedicationMap(PostInitializeMedicationMapRequest request)
        {
            DTO.MedicationMap med = null;
            try
            {
               MedicationMapData data = EndpointUtil.InitializeMedicationMap(request);
                if (data != null)
                {
                    med = Mapper.Map<DTO.MedicationMap>(data);
                }
                return med;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientMedSupp - Posts
        public List<PatientMedSupp> GetPatientMedSupps(GetPatientMedSuppsRequest request)
        {
            List<PatientMedSupp> patientMedSupps = null;
            try
            {
                List<PatientMedSuppData> data = EndpointUtil.GetPatientMedSupps(request);
                if (data != null && data.Count > 0)
                {
                    patientMedSupps = new List<PatientMedSupp>();
                    data.ForEach(a => patientMedSupps.Add(Mapper.Map<PatientMedSupp>(a)));
                }
                return patientMedSupps;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientMedSupp SavePatientMedSupp(PostPatientMedSuppRequest request)
        {
            PatientMedSupp patientMedSupp = null;
            try
            {
                if (request.PatientMedSupp != null)
                {
                    //Update Medication collection to add any newly initialized medication and then register in search index.
                    if (request.PatientMedSupp.IsNew)
                    {
                        PostMedicationMapRequest req = new DTO.PostMedicationMapRequest
                        {
                            MedicationMap = new DTO.MedicationMap
                            {
                                Id = request.PatientMedSupp.FamilyId,
                                FullName  = request.PatientMedSupp.Name,
                                SubstanceName = string.Empty,
                                Strength = string.IsNullOrEmpty(request.PatientMedSupp.Strength) ? string.Empty : request.PatientMedSupp.Strength,
                                Route = string.IsNullOrEmpty(request.PatientMedSupp.Route) ? string.Empty : request.PatientMedSupp.Route,
                                Form = string.IsNullOrEmpty(request.PatientMedSupp.Form) ? string.Empty : request.PatientMedSupp.Form,
                                Custom = true,
                                Verified = false
                            },
                            ContractNumber = request.ContractNumber,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        MedicationMapData medData = EndpointUtil.UpdateMedicationMap(req);
                        //DTO.MedicationMap newMed = Mapper.Map<DTO.MedicationMap>(medData);
                        DTO.Medication newMed = new DTO.Medication { Id = medData.Id, ProprietaryName = medData.FullName};
                        // Register newly initialized medicationMap in search index.
                        SearchManager.RegisterMedDocumentInSearchIndex(newMed, req.ContractNumber);
                    }
                    // Populate calculated NDC codes and Pharm classes in the request object before save.
                    bool calculateNDC = false;
                    if (request.Insert)
                    {
                        calculateNDC = true;
                        request.PatientMedSupp.SystemName = Constants.SystemName;
                    }
                    else
                    {
                        // On update, check for ReCalculateNDC flag.
                        if (request.RecalculateNDC)
                        {
                            calculateNDC = true;
                        }
                    }
                    if (calculateNDC)
                    {
                        request.PatientMedSupp.NDCs = EndpointUtil.GetMedicationNDCs(request);
                    }
                    PatientMedSuppData data = EndpointUtil.SavePatientMedSupp(request);
                    if (data != null)
                    {
                        patientMedSupp = Mapper.Map<PatientMedSupp>(data);
                    }
                }
                return patientMedSupp;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
