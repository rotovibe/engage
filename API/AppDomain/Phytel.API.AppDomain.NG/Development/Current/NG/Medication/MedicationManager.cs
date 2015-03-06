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
                    string name = string.IsNullOrEmpty(request.PatientMedSupp.Name) ? string.Empty : request.PatientMedSupp.Name.ToUpper();
                    string form = string.IsNullOrEmpty(request.PatientMedSupp.Form) ? string.Empty : request.PatientMedSupp.Form.ToUpper();
                    string route = string.IsNullOrEmpty(request.PatientMedSupp.Route) ? string.Empty : request.PatientMedSupp.Route.ToUpper();
                    string strength = string.IsNullOrEmpty(request.PatientMedSupp.Strength) ? string.Empty : request.PatientMedSupp.Strength;
                    
                    #region Search MedicationMap
                    // Search if any record exists with the given combination of name, strength, route and form.
                    GetMedicationMapsRequest mmRequest = new GetMedicationMapsRequest
                    {
                        Name = name,
                        Route = route,
                        Form = form,
                        Strength = strength,
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version
                    };
                    List<MedicationMapData> list = EndpointUtil.SearchMedicationMap(mmRequest); 
                    #endregion
                    if(list == null)
                    {
                        MedicationMapData medData = null;
                        if (string.IsNullOrEmpty(request.PatientMedSupp.FamilyId))
                        {
                            #region Insert MedicationMap
                            PostMedicationMapRequest insertReq = new PostMedicationMapRequest
                            {
                                MedicationMap = new DTO.MedicationMap
                                {
                                    FullName = name,
                                    SubstanceName = string.Empty,
                                    Strength = strength,
                                    Route = route,
                                    Form = form,
                                    Custom = true,
                                    Verified = false
                                },
                                ContractNumber = request.ContractNumber,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            medData = EndpointUtil.InsertMedicationMap(insertReq);
                            #endregion
                        }
                        else 
                        {
                            #region Update MedicationMap
                            // This saves the initialized medicine map
                            PutMedicationMapRequest req = new PutMedicationMapRequest
                            {
                                MedicationMap = new DTO.MedicationMap
                                {
                                    Id = request.PatientMedSupp.FamilyId,
                                    FullName = name,
                                    SubstanceName = string.Empty,
                                    Strength = strength,
                                    Route = route,
                                    Form = form,
                                    Custom = true,
                                    Verified = false
                                },
                                ContractNumber = request.ContractNumber,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            medData = EndpointUtil.UpdateMedicationMap(req);
                            #endregion
                        }
                        RegisterMedication(request.ContractNumber, medData);
                    }
                    #region Calculate NDC codes.
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
                    #endregion
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

        /// <summary>
        /// Register new medication name in Lucene indexes.
        /// </summary>
        /// <param name="contractNumber">contract Number sent in the request.</param>
        /// <param name="medData">MedicationMapData object.</param>
        private void RegisterMedication(string contractNumber, MedicationMapData medData)
        {
            DTO.Medication newMed = new DTO.Medication
            {
                Id = medData.Id,
                ProprietaryName = medData.FullName,
                Strength = medData.Strength,
                DosageFormName = medData.Form,
                RouteName = medData.Route,
                SubstanceName = string.Empty,
                NDC = string.Empty,
                ProductId = string.Empty,
                ProprietaryNameSuffix = string.Empty
            };
            SearchManager.RegisterMedDocumentInSearchIndex(newMed, contractNumber);
        }
        #endregion

        #region PatientMedSupp - Delete
        public void DeletePatientMedSupp(DeletePatientMedSuppRequest request)
        {
            try
            {
                EndpointUtil.DeletePatientMedSupp(request);

            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientMedFrequency
        public List<PatientMedFrequency> GetPatientMedFrequencies(GetPatientMedFrequenciesRequest request)
        {
            List<PatientMedFrequency> patientMedFreqs = null;
            try
            {
                List<PatientMedFrequencyData> data = EndpointUtil.GetPatientMedFrequencies(request);
                if (data != null && data.Count > 0)
                {
                    patientMedFreqs = new List<PatientMedFrequency>();
                    data.ForEach(a =>
                        patientMedFreqs.Add(new PatientMedFrequency { Id = a.Id, Name = a.PatientId } ));
                }
                return patientMedFreqs;
            }
            catch (Exception ex) { throw ex; }
        }

        public string InsertPatientMedFrequency(PostPatientMedFrequencyRequest request)
        {
            string id = null;
            try
            {
                id = EndpointUtil.InsertPatientMedFrequency(request);
                return id;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
