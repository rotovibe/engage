using System;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace DataDomain.Medication.Repo
{
    public interface IMongoMedicationRepository : IRepository
    {
        object Search(object request);
        object FindByPatientId(object request);
        object FindNDCCodes(object request);
        object Initialize(object newEntity);
        object FindByName(object request);
        object Find(object request);
    }
}