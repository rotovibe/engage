using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class PatientSystemManager : ManagerBase, IPatientSystemManager
    {
        public IPatientSystemEndpointUtil EndpointUtil { get; set; }

        public List<SystemSource> GetActiveSystemSources(GetActiveSystemSourcesRequest request)
        {
            try
            {
                List<SystemSource> result = new List<SystemSource>();
                List<SystemSourceData> ssData = EndpointUtil.GetSystemSources(request);
                if (ssData != null && ssData.Count > 0)
                {
                    ssData.ForEach(s => 
                    {
                        // Get only active system sources.
                        if (s.StatusId == (int)Status.Active)
                        {
                            result.Add(Mapper.Map<SystemSource>(s));
                        }
                    });
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
