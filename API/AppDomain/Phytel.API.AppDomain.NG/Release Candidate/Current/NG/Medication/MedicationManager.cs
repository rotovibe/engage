using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.LookUp.DTO;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.Medication
{
    public class MedicationManager : ManagerBase, IMedicationManager
    {
        public IMedicationEndpointUtil EndpointUtil { get; set; }
        public ISearchManager SearchManager { get; set; }

        #region MedicationMap - Gets
        public List<MedicationMap> GetMedicationMaps(GetMedicationMapsRequest request)
        {
            List<MedicationMap> medMaps = null;
            try
            {
                List<MedicationMapData> data = EndpointUtil.SearchMedicationMap(request);
                if (data != null)
                {
                    medMaps = new List<MedicationMap>();
                    data.ForEach(a => medMaps.Add(Mapper.Map<MedicationMap>(a)));
                }
                return medMaps;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

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

        #region MedicationMap - Delete
        public void DeleteMedicationMaps(DeleteMedicationMapsRequest request)
        {
            try
            {
                // Call MedicationMap endpoint to delete medicationmaps.
                EndpointUtil.DeleteMedicationMaps(request);
                // Call the Search endpoint to delete Lucene indexes.
                SearchManager.DeleteMedDocuments(request);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region PatientMedSupp - Gets
        public int GetPatientMedSuppsCount(GetPatientMedSuppsCountRequest request)
        {
            try
            {
                return EndpointUtil.GetPatientMedSuppsCount(request);
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
                        RegisterMedication(request, medData);
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

                    string sigCode = CalculateSigCode( request );
                    if (!string.IsNullOrEmpty(sigCode))
                    {
                        request.PatientMedSupp.SigCode = sigCode;
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

        public string CalculateSigCode(PostPatientMedSuppRequest request)            
        {
            DateTime? startDate =  request.PatientMedSupp.StartDate;
            DateTime? endDate = request.PatientMedSupp.EndDate;
            string quantity = request.PatientMedSupp.FreqQuantity;
            string strength =        request.PatientMedSupp.Strength;   
            string form =            request.PatientMedSupp.Form;
            string route =           request.PatientMedSupp.Route;
            string frequencyId =     request.PatientMedSupp.FrequencyId;
            string patientId =       request.PatientMedSupp.PatientId;
            string contractNumber =  request.ContractNumber;
            string token =       request.Token;
            string userId =      request.UserId;
            double version = request.Version;

            string sigCode = "";
            string dateRange = "";
            if (startDate != null && endDate != null)
            {
                startDate = (DateTime)startDate.Value.ToUniversalTime();
                endDate = (DateTime)endDate.Value.ToUniversalTime();                                
                TimeSpan ts = endDate.Value - startDate.Value;
                int days = (int)Math.Round(ts.TotalDays);   // daylight savings adjustments: just round it
                if (days > 0)
                {
                    dateRange = "for " + days.ToString() + (days == 1 ? " day" : " days");
                }
            }
            if (quantity == null) quantity = "";
            quantity = quantity.Trim();
            if (strength == null) strength = "";
            strength = strength.Trim();
            if (form == null) form = "";
            form = form.Trim();
            if (route == null) route = "";
            route = route.Trim();

            if (string.IsNullOrEmpty(quantity.Trim()) && string.IsNullOrEmpty(strength.Trim()) && string.IsNullOrEmpty(form.Trim())
                    && string.IsNullOrEmpty(route.Trim()) && string.IsNullOrEmpty(frequencyId) && string.IsNullOrEmpty(dateRange))
            {
                sigCode = "-";
            }
            else
            {
                string howOften = "";
                if (!string.IsNullOrEmpty(frequencyId))
                {
                    //get the frequency name (how often) from frequencyId:
                   

                    NGManager ngManager = new NGManager();
                    GetLookUpsRequest getLookupsRequest = new GetLookUpsRequest
                    {
                        ContractNumber = contractNumber,
                        Token = token,
                        TypeName = "Frequency",
                        UserId = userId,
                        Version = version
                    };
                    List<IdNamePair> frequencies = ngManager.GetLookUps(getLookupsRequest);
                    
                    //GetLookUpDetailsRequest lookupRequest = new GetLookUpDetailsRequest
                    //{
                    //    ContractNumber = contractNumber,
                    //    Token = token,
                    //    TypeName = "Frequency",
                    //    UserId = userId,
                    //    Version = version
                    //};
                    //List<LookUpDetails> lookups = ngManager.GetLookUpDetails(lookupRequest); //this has error in the implementation code!
                    if (frequencies != null && frequencies.Count() > 0)
                    {
                        IdNamePair aLookup = frequencies.Where(l => l.Id.Equals(frequencyId)).FirstOrDefault();
                        if (aLookup != null)
                        {
                            howOften = aLookup.Name;
                        }
                        else
                        {
                            GetPatientMedFrequenciesRequest freqRequest = new GetPatientMedFrequenciesRequest();
                            freqRequest.PatientId = request.PatientMedSupp.PatientId;
                            freqRequest.ContractNumber = contractNumber;
                            freqRequest.UserId = userId;
                            freqRequest.Token = token;
                            freqRequest.Version = version;
                            List<PatientMedFrequency> patientFrequencies = GetPatientMedFrequencies(freqRequest);
                            if (frequencies != null && (frequencies.Count() > 0))
                            {
                                PatientMedFrequency theFrequency = patientFrequencies.Where(f => f.Id.Equals(frequencyId)).FirstOrDefault();
                                if (theFrequency != null)
                                {
                                    howOften = theFrequency.Name;
                                    howOften = howOften.Trim();
                                }
                            }
                        }
                    }                    
                }
                sigCode = AddSigSubCode(sigCode, quantity);
                sigCode = AddSigSubCode(sigCode, strength);
                sigCode = AddSigSubCode(sigCode, form);
                sigCode = AddSigSubCode(sigCode, route);
                sigCode = AddSigSubCode(sigCode, howOften);
                sigCode = AddSigSubCode(sigCode, dateRange);                
            }
            return sigCode;
        }

        private string AddSigSubCode(string sigCode, string subCode)
        {            
            if (subCode.Length > 0)
            {
                if (sigCode.Length > 0)
                {
                    sigCode += " ";
                }
                sigCode = sigCode + subCode;
            }
            return sigCode;
        }

        public void DeleteMedicationMap(PutDeleteMedMapRequest request)
        {
            throw new NotImplementedException();
            //try
            //{
            //    if (request.MedicationMaps != null)
            //    {
            //        List<MedicationMapData> list = EndpointUtil.DeleteMedicationMap(request);
            //        if (list != null)
            //        {
            //            List<MedicationMap> newMaps = new List<MedicationMap>();
            //            list.ForEach(x =>
            //            {
            //                MedicationMap m = new MedicationMap
            //                {
            //                    Id = x.Id,
            //                    FullName = x.FullName,
            //                    Form = x.Form,
            //                    Route = x.Route,
            //                    Strength = x.Strength
            //                };
            //                newMaps.Add(m);
            //            });
            //            request.MedicationMaps = newMaps;
            //            // call the search endpoint with new request. New Request has the Id populated.
            //            SearchManager.DeleteMedDocuments(request);
            //        }
            //    }
            //}
            //catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Register new medication name in Lucene indexes.
        /// </summary>
        /// <param name="contractNumber">contract Number sent in the request.</param>
        /// <param name="medData">MedicationMapData object.</param>
        private void RegisterMedication(IAppDomainRequest request, MedicationMapData medData)
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
            SearchManager.RegisterMedDocumentInSearchIndex(newMed, request);
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
                        patientMedFreqs.Add(new PatientMedFrequency { Id = a.Id, Name = a.Name } ));
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
                // Before inserting a new one, check if the Frequency lookup already contains that name.
                if (request.PatientMedFrequency != null && !string.IsNullOrEmpty(request.PatientMedFrequency.Name))
                {
                    NGManager ngManager = new NGManager();
                    GetLookUpsRequest lookUpRequest = new GetLookUpsRequest
                    {
                        ContractNumber = request.ContractNumber,
                        TypeName = LookUpType.Frequency.ToString(),
                        UserId = request.UserId,
                        Version = request.Version,
                    };
                    List<IdNamePair> lookups = ngManager.GetLookUps(lookUpRequest);
                    var freq = lookups.Find(x => x.Name.ToLower() == request.PatientMedFrequency.Name.ToLower());
                    if (freq != null)
                    {
                        id = freq.Id;
                    }
                }
                if (string.IsNullOrEmpty(id))
                {
                    id = EndpointUtil.InsertPatientMedFrequency(request);
                }
                return id;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
