using System;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace DataDomain.Medication.Repo
{
    public interface IMongoMedicationRepository : IRepository
    {
        object FindByPatientId(object request);
        object SearchMedications(object request);
    }
}