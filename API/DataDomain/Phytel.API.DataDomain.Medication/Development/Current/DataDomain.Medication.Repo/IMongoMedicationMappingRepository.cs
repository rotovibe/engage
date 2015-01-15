using System;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace DataDomain.Medication.Repo
{
    public interface IMongoMedicationMappingRepository : IRepository
    {
        object FindByPatientId(object request);
        object FindNDCCodes(object request);
        object Initialize(object newEntity);
    }
}