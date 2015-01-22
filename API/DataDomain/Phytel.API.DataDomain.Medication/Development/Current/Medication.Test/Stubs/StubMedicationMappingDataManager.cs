using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;
using AutoMapper;
using Phytel.API.Interface;
using System.Configuration;
using Phytel.API.DataDomain.Medication.Test;

namespace Phytel.API.DataDomain.Medication
{
    public class StubMedicationMappingDataManager : IMedicationMappingDataManager
    {

        public MedicationMapData InsertMedicationMapping(PutInsertMedicationMappingRequest request, MedicationMapData mm)
        {
            throw new NotImplementedException();
        }

        public MedicationMapData InitializeMedicationMap(PutInitializeMedicationMapDataRequest request)
        {
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);
            return (MedicationMapData)repo.Initialize(request);
        }

        public MedicationMapData UpdateMedicationMap(PutMedicationMapDataRequest request)
        {
            MedicationMapData result = null;
            var repo = StubRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);
            bool status = (bool)repo.Update(request);
            if (status)
            {
                result = (MedicationMapData)repo.FindByID(request.MedicationMapData.Id);
            }
            return result;
        }
    }
}   
