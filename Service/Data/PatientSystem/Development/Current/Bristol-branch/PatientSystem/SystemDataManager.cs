using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientSystem;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class SystemDataManager : ISystemDataManager
    {
        IPatientSystemRepositoryFactory Factory { get; set; }

        public SystemDataManager(IPatientSystemRepositoryFactory repo)
        {
            Factory = repo;
        }

        public List<SystemData> GetSystems(GetSystemsDataRequest request)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.System);
                List<SystemData> list = repository.Find(request) as List<SystemData>;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
