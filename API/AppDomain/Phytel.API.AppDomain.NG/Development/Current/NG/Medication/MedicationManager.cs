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


        #region Medication - Posts
        public DTO.Medication InitializeMedSupp(PostInitializeMedSuppRequest request)
        {
            DTO.Medication medication = null;
            try
            {

                MedicationData data = EndpointUtil.InitializeMedSupp(request);
                if (data != null)
                {
                    medication = Mapper.Map<DTO.Medication>(data);
                }
                return medication;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientMedSupp - Posts
        public PatientMedSupp InitializePatientMedSupp(PostInitializePatientMedSuppRequest request)
        {
            PatientMedSupp patientMedSupp = null;
            try
            {

                PatientMedSuppData data = EndpointUtil.InitializePatientMedSupp(request);
                if (data != null)
                {
                    patientMedSupp = Mapper.Map<PatientMedSupp>(data);
                }
                return patientMedSupp;
            }
            catch (Exception ex) { throw ex; }
        } 

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

        public PatientMedSupp UpdatePatientMedSupp(PostPatientMedSuppRequest request)
        {
            PatientMedSupp patientMedSupp = null;
            try
            {
                if (request.PatientMedSupp != null)
                {
                    //Update Medication collection to add any newly initialized medication and then register in search index.
                    if (request.PatientMedSupp.IsNewAllergy)
                    {
                        PostMedicationRequest req = new DTO.PostMedicationRequest
                        {
                            Medication = new DTO.Medication {
                                Id = request.PatientMedSupp.MedSuppId,
                                NDC = string.Empty,
                                ProductId = string.Empty,
                                ProprietaryName = string.Empty,
                                ProprietaryNameSuffix = string.Empty,
                                SubstanceName = string.Empty,
                                RouteName = string.Empty,
                                DosageFormName = string.Empty,
                                Strength = string.Empty
                            },
                            ContractNumber = request.ContractNumber,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        MedicationData medData = EndpointUtil.UpdateMedication(req);
                        DTO.Medication newMed = Mapper.Map<DTO.Medication>(medData);
                        // Register newly initialized medication in search index.
                        SearchManager.RegisterMedDocumentInSearchIndex(newMed, req.ContractNumber);
                        // For newly initialized medication, calculate NDC codes.
                        request.RecalculateNDC = true;
                    }
                    // Populate calculated NDC codes and Pharm classes in the request object before save.
                    if (request.RecalculateNDC)
                    {
                        request.PatientMedSupp.NDCs = EndpointUtil.GetMedicationNDCs(request);
                    }
                    PatientMedSuppData data = EndpointUtil.UpdatePatientMedSupp(request);
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
