using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;

namespace Phytel.API.DataDomain.Medication
{
    public class MedicationMappingDataManager : IMedicationMappingDataManager
    {
        public List<MedicationMapData> GetMedicationMap(GetMedicationMapDataRequest request)
        {
            try
            {
                List<MedicationMapData> result = null;
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);
                result = repo.Search(request) as List<MedicationMapData>;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        
        public MedicationMapData InitializeMedicationMap(PutInitializeMedicationMapDataRequest request)
        {
            try
            {
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);
                return (MedicationMapData)repo.Initialize(request);
            }
            catch (Exception ex) { throw ex; }
        }

        public DTO.MedicationMapData InsertMedicationMap(PostMedicationMapDataRequest request)
        {
            MedicationMapData result = null;
            try
            {
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);
                if (request.MedicationMapData != null)
                {
                    result = repo.Insert(request as object) as MedicationMapData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public MedicationMapData UpdateMedicationMap(PutMedicationMapDataRequest request)
        {
            try
            {
                MedicationMapData result = null;
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);

                if (request.MedicationMapData != null)
                {
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        result = (MedicationMapData)repo.FindByID(request.MedicationMapData.Id);
                    }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool DeleteMedicationMaps(PutDeleteMedMapDataRequest request)
        {
            try
            {
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);

                if (request.MedicationMaps != null)
                {
                    repo.DeleteAll(request.MedicationMaps.Cast<object>().ToList());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
