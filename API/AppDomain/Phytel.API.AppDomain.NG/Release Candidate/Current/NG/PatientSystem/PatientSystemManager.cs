using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.AppDomain.NG.PatientSystem;
using Phytel.API.AppDomain.NG.PatientSystem.Modifiers;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Status = Phytel.API.DataDomain.PatientSystem.DTO.Status;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemManager : ManagerBase, IPatientSystemManager
    {
        public IPatientSystemEndpointUtil EndpointUtil { get; set; }

        public List<DTO.System> GetActiveSystems(IServiceContext context)
        {
            try
            {
                List<DTO.System> result = new List<DTO.System>();
                List<SystemData> ssData = EndpointUtil.GetSystems(context);
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

        public List<DTO.PatientSystem> GetPatientSystems(IServiceContext context, string patientId)
        {
            List<DTO.PatientSystem> list = null;
            try
            {
                List<SystemData> ssData = EndpointUtil.GetSystems(context);
                if (ssData != null && ssData.Count > 0)
                {
                    // Get only active system sources.
                    List<SystemData> activeSystemSources = ssData.FindAll(s => s.StatusId == (int)Status.Active);
                    if (activeSystemSources.Count > 0)
                    {
                        List<PatientSystemData> dataList = EndpointUtil.GetPatientSystems(context, patientId);
                        if (dataList != null && dataList.Count > 0)
                        {
                            list = new List<DTO.PatientSystem>();
                            dataList.ForEach(a =>
                                {
                                    if (activeSystemSources.Exists(x => x.Id == a.SystemId))
                                    {
                                        list.Add(Mapper.Map<DTO.PatientSystem>(a));
                                    }
                                });
                        }
                    }
                }
                return list;
            }   
            catch (Exception ex) { throw ex; }
        }

        public List<DTO.PatientSystem> InsertPatientSystems(IServiceContext context, string patientId)
        {
            List<DTO.PatientSystem> list = null;
            try
            {
                if (context.Tag == null) throw new Exception("Context tag is empty. There are no patientsystems to insert");
                var cList = (List<DTO.PatientSystem>)context.Tag;

                SetPrimaryPatientSystem(context, patientId, cList);

                List<PatientSystemData> dataList = EndpointUtil.InsertPatientSystems(context, patientId);

                if (dataList != null && dataList.Count > 0)
                {
                    list = new List<DTO.PatientSystem>();
                    dataList.ForEach(a => list.Add(Mapper.Map<DTO.PatientSystem>(a)));
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetPrimaryPatientSystem(IServiceContext context, string patientId, List<DTO.PatientSystem> cList)
        {
            if (new HasPrimaryDesignated().Modify(cList))
            {
                var dsList = EndpointUtil.GetPatientSystems(context, patientId);
                if (dsList != null)
                {
                    context.Tag = new SetSystemsAsNonPrimary().Modify(dsList);
                    EndpointUtil.UpdatePatientSystems(context, patientId);
                }
                context.Tag = new SetPrimaryFromList().Modify(cList);
            }
        }

        public List<DTO.PatientSystem> UpdatePatientSystems(IServiceContext context, string patientId)
        {
            List<DTO.PatientSystem> list = null;
            try
            {
                List<PatientSystemData> dataList = EndpointUtil.UpdatePatientSystems(context, patientId);

                if (dataList != null && dataList.Count > 0)
                {
                    list = new List<DTO.PatientSystem>();
                    dataList.ForEach(a => list.Add(Mapper.Map<DTO.PatientSystem>(a)));
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

        public string UpdatePatientAndSystemsData(IServiceContext context)
        {
            var result = string.Empty;
            var migrateCount = 0;
            var engageCount = 0;
            var migrate = (bool)context.Tag;
            List<string> patientsWithNoPrograms = new List<string>();

            if (migrate)
            {
                #region UpdateExistingPatientSystem
                if (string.Equals(context.Contract, "InHealth001", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Get all PatientSystems to update each record.
                    List<PatientSystemOldData> pSys = EndpointUtil.GetAllPatientSystems(context);
                    if (pSys != null && pSys.Count > 0)
                    {
                        // Remove all the newly added records.
                        pSys.RemoveAll(x => string.IsNullOrEmpty(x.OldSystemId));
                        if (pSys.Count > 0)
                        {
                            var bsdiSystem = EndpointUtil.GetSystems(context).FirstOrDefault(r => r.Name.Equals("BSDI", StringComparison.InvariantCultureIgnoreCase));
                            var hicSystem = EndpointUtil.GetSystems(context).FirstOrDefault(r => r.Name.Equals("HIC", StringComparison.InvariantCultureIgnoreCase));
                            List<DTO.PatientSystem> data = new List<DTO.PatientSystem>();
                            pSys.ForEach(p =>
                            {
                                var systemId = string.Empty;
                                ProgramStatus pStatus = hasHealthyWeightProgramAssigned(p.PatientId, context);

                                if (pStatus.HasProgramsAssigned)
                                {
                                    systemId = pStatus.HasHealthyWeightProgramsAssigned ? bsdiSystem.Id : hicSystem.Id;

                                    data.Add(new DTO.PatientSystem
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
                                ContractNumber = context.Contract,
                                PatientId = data[0].PatientId,
                                PatientSystems = data,
                                UserId = context.UserId,
                                Version = context.Version
                            };
                            context.Tag = updateRequest.PatientSystems;
                            List<PatientSystemData> dataList = EndpointUtil.UpdatePatientSystems(context, updateRequest.PatientId);
                            if (dataList != null)
                            {
                                migrateCount = dataList.Count;
                            }
                        }
                    }
                }
                result = string.Format("For {0} contract, migrated data for {1} existing PatientSystem Ids. Patients that were not migratred since they had no programs assigned: {2}", context.Contract, migrateCount, string.Join(", ", patientsWithNoPrograms));
                #endregion
            }
            else
            {
                #region InsertEngageSystemForEachPatient
                // Get All patients to add an Engage ID.
                List<PatientData> patients = EndpointUtil.GetAllPatients(context);
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
                        ContractNumber = context.Contract,
                        PatientId = insertData[0].PatientId,
                        UserId = Constants.SystemContactId,
                        Version = context.Version,
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
                result = string.Format("For {0} contract, Added engage ids for {1} patients", context.Contract, engageCount);
                #endregion
            }
            return result;
        }

        private ProgramStatus hasHealthyWeightProgramAssigned(string patientId, IServiceContext context)
        {
            return EndpointUtil.HasHealthyWeightProgramAssigned(patientId, context);
        }

        public class ProgramStatus
        {
            public bool HasProgramsAssigned { get; set; }
            public bool HasHealthyWeightProgramsAssigned { get; set; }
        }
    }
}
