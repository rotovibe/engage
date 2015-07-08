using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientSystem;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class SystemSourceDataManager : ISystemSourceDataManager
    {
        IPatientSystemRepositoryFactory Factory { get; set; }

        public SystemSourceDataManager(IPatientSystemRepositoryFactory repo)
        {
            Factory = repo;
        }

        public List<SystemSourceData> GetSystemSources(GetSystemSourcesDataRequest request)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.SystemSource);
                List<SystemSourceData> list = repository.Find(request) as List<SystemSourceData>;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
