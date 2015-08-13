﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemManager : ManagerBase, IPatientSystemManager
    {
        public IPatientSystemEndpointUtil EndpointUtil { get; set; }

        public List<DTO.System> GetActiveSystems(GetActiveSystemsRequest request)
        {
            try
            {
                List<DTO.System> result = new List<DTO.System>();
                List<SystemData> ssData = EndpointUtil.GetSystems(request);
                if (ssData != null && ssData.Count > 0)
                {
                    ssData.ForEach(s => 
                    {
                        // Get only active system sources.
                        if (s.StatusId == (int)Status.Active)
                        {
                            result.Add(Mapper.Map<DTO.System>(s));
                        }
                    });
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystem> GetPatientSystems(GetPatientSystemsRequest request)
        {
            List<PatientSystem> list = null;
            try
            {
                GetActiveSystemsRequest ssRequest = new GetActiveSystemsRequest { ContractNumber = request.ContractNumber, UserId = request.UserId, Version = request.Version };
                List<SystemData> ssData = EndpointUtil.GetSystems(ssRequest);
                if (ssData != null && ssData.Count > 0)
                {
                    // Get only active system sources.
                    List<SystemData> activeSystemSources = ssData.FindAll(s => s.StatusId == (int)Status.Active);
                    if (activeSystemSources.Count > 0)
                    {
                        List<PatientSystemData> dataList = EndpointUtil.GetPatientSystems(request);
                        if (dataList != null && dataList.Count > 0)
                        {
                            list = new List<PatientSystem>();
                            dataList.ForEach(a =>
                                {
                                    if (activeSystemSources.Exists(x => x.Id == a.SystemId))
                                    {
                                        list.Add(Mapper.Map<PatientSystem>(a));
                                    }
                                });
                        }
                    }
                }
                return list;
            }   
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystem> InsertPatientSystems(InsertPatientSystemsRequest request)
        {
            List<PatientSystem> list = null;
            try
            {
                List<PatientSystemData> dataList = EndpointUtil.InsertPatientSystems(request);
                if (dataList != null && dataList.Count > 0)
                {
                    list = new List<PatientSystem>();
                    dataList.ForEach(a => list.Add(Mapper.Map<PatientSystem>(a)));
                }
                return list;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystem> UpdatePatientSystems(UpdatePatientSystemsRequest request)
        {
            List<PatientSystem> list = null;
            try
            {
                List<PatientSystemData> dataList = EndpointUtil.UpdatePatientSystems(request);
                if (dataList != null && dataList.Count > 0)
                {
                    list = new List<PatientSystem>();
                    dataList.ForEach(a => list.Add(Mapper.Map<PatientSystem>(a)));
                }
                return list;
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeletePatientSystems(DeletePatientSystemsRequest request)
        {
            try
            {
                EndpointUtil.DeletePatientSystems(request);
            }
            catch (Exception ex) { throw ex; }
        }

        public string UpdatePatientAndSystemsData(UpdatePatientsAndSystemsRequest request)
        {
            string result = string.Empty;
            int migrateCount = 0;
            int engageCount = 0;
            List<string> patientsWithNoPrograms = new List<string>();
            if (request.Migrate)
            {
                #region UpdateExistingPatientSystem
                if (string.Equals(request.ContractNumber, "InHealth001", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Get all PatientSystems to update each record.
                    List<PatientSystemOldData> pSys = EndpointUtil.GetAllPatientSystems(request);
                    if (pSys != null && pSys.Count > 0)
                    {
                        // Remove all the newly added records.
                        pSys.RemoveAll(x => string.IsNullOrEmpty(x.OldSystemId));
                        if (pSys.Count > 0)
                        {
                            var bsdiSystem = EndpointUtil.GetSystems(Mapper.Map<GetActiveSystemsRequest>(request)).FirstOrDefault(r => r.Name.Equals("BSDI", StringComparison.InvariantCultureIgnoreCase));
                            var hicSystem = EndpointUtil.GetSystems(Mapper.Map<GetActiveSystemsRequest>(request)).FirstOrDefault(r => r.Name.Equals("HIC", StringComparison.InvariantCultureIgnoreCase));
                            List<PatientSystem> data = new List<PatientSystem>();
                            pSys.ForEach(p =>
                            {
                                string systemId = string.Empty;
                                ProgramStatus pStatus = hasHealthyWeightProgramAssigned(p.PatientId, request);
                                if (pStatus.HasProgramsAssigned)
                                {
                                    if (pStatus.HasHealthyWeightProgramsAssigned)
                                    {
                                        systemId = bsdiSystem.Id;
                                    }
                                    else
                                    {
                                        systemId = hicSystem.Id;
                                    }

                                    data.Add(new PatientSystem
                                    {
                                        Id = p.Id,
                                        PatientId = p.PatientId,
                                        Primary = false,
                                        StatusId = (int)Status.Active,
                                        SystemId = systemId,
                                        DataSource = "Import",
                                        Value = p.OldSystemId.Trim(),
                                    });
                                }
                                else
                                {
                                    patientsWithNoPrograms.Add(p.PatientId);
                                }

                            });
                            UpdatePatientSystemsRequest updateRequest = new UpdatePatientSystemsRequest
                            {
                                ContractNumber = request.ContractNumber,
                                PatientId = data[0].PatientId,
                                PatientSystems = data,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            List<PatientSystemData> dataList = EndpointUtil.UpdatePatientSystems(updateRequest);
                            if (dataList != null)
                            {
                                migrateCount = dataList.Count;
                            }
                        }
                    }
                }
                result = string.Format("For {0} contract, migrated data for {1} existing PatientSystem Ids. Patients that were not migratred since they had no programs assigned: {2}", request.ContractNumber, migrateCount, string.Join(", ", patientsWithNoPrograms));
                #endregion
            }
            else
            {
                #region InsertEngageSystemForEachPatient
                // Get All patients to add an Engage ID.
                List<Phytel.API.DataDomain.Patient.DTO.PatientData> patients = EndpointUtil.GetAllPatients(request);
                if (patients.Count > 0)
                {
                    List<PatientSystemData> insertData = new List<PatientSystemData>();
                    patients.ForEach(p =>
                    {
                        insertData.Add(new PatientSystemData
                        {
                            PatientId = p.Id,
                        });
                    });

                    InsertEngagePatientSystemsDataRequest insertRequest = new InsertEngagePatientSystemsDataRequest
                    {
                        ContractNumber = request.ContractNumber,
                        PatientId = insertData[0].PatientId,
                        UserId = Constants.SystemContactId,
                        Version = request.Version,
                        Context = "NG",
                        PatientSystemsData = insertData
                    };
                    insertRequest.UserId = Constants.SystemContactId; // the requirement says that the engage Id should have createdby user as 'system'.
                    List<string> engageList = EndpointUtil.InsertEngagePatientSystems(insertRequest);
                    if (engageList != null)
                    {
                        engageCount = engageList.Count;
                    }
                }
                result = string.Format("For {0} contract, Added engage ids for {1} patients", request.ContractNumber, engageCount);
                #endregion
            }
            return result;
        }

        private ProgramStatus hasHealthyWeightProgramAssigned(string patientId, UpdatePatientsAndSystemsRequest request)
        {
            return EndpointUtil.HasHealthyWeightProgramAssigned(patientId, request);
        }

        public class ProgramStatus
        {
            public bool HasProgramsAssigned { get; set; }
            public bool HasHealthyWeightProgramsAssigned { get; set; }
        }
    }
}
