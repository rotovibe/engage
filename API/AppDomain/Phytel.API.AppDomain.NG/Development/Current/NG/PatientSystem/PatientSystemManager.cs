using System;
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
            int bsdiCount = 0;
            int engageCount = 0;
            var pSys = EndpointUtil.GetAllPatientSystems(request);
            // Remove all the newly added records.
            pSys.RemoveAll(x => string.IsNullOrEmpty(x.SystemID));
            if (pSys.Count > 0)
            {
                var bsdiSystem = EndpointUtil.GetSystems(Mapper.Map<GetActiveSystemsRequest>(request)).FirstOrDefault(r => r.Name.Equals("BSDI", StringComparison.InvariantCultureIgnoreCase));
                var engageSystem = EndpointUtil.GetSystems(Mapper.Map<GetActiveSystemsRequest>(request)).FirstOrDefault(r => r.Name.Equals("Engage", StringComparison.InvariantCultureIgnoreCase));

                #region UpdateExistingPatientSystem
                List<PatientSystem> data = new List<PatientSystem>();
                pSys.ForEach(p =>
                    {
                        data.Add(new PatientSystem 
                        {
                            Id = p.Id,
                            PatientId = p.PatientId,
                            Primary = false,
                            StatusId = (int)Status.Active,
                            SystemId = bsdiSystem.Id,
                            SystemSource = "Import",
                            Value  = p.SystemID.Trim(),
                        });
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
                    bsdiCount = dataList.Count;
                }
                #endregion

                #region InsertEngageSystemForEachPatient

                List<PatientSystem> insertData = new List<PatientSystem>();
                pSys.ForEach(p =>
                    {
                        data.Add(new PatientSystem 
                        {
                            PatientId = p.PatientId,
                            SystemId = engageSystem.Id,
                        });
                    });

                InsertPatientSystemsRequest insertRequest = new InsertPatientSystemsRequest
                {
                    ContractNumber = request.ContractNumber,
                    PatientId = insertData[0].PatientId,
                    UserId = request.UserId,
                    Version = request.Version,
                    PatientSystems = insertData
                };

                List<PatientSystemData> engageList = EndpointUtil.InsertPatientSystems(insertRequest);
                if (engageList != null)
                {
                    engageCount = engageList.Count;
                }
                #endregion
            }

            return string.Format("For {0} contract, migrated data for {1} BSDI Ids and added data for {2} Engage Ids.",request.ContractNumber, bsdiCount, engageCount);
        }
    }
}
