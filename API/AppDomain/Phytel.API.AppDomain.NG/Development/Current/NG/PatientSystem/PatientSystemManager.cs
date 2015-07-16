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
            var pSys = EndpointUtil.GetAllPatientSystems(request);
            var sSys = EndpointUtil.GetSystems(Mapper.Map<GetActiveSystemsRequest>(request));



            return "success";
        }
    }
}
