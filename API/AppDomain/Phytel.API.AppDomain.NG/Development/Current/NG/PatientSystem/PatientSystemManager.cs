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
    }
}
