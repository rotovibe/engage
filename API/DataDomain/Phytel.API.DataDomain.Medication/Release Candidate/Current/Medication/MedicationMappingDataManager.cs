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

        public List<MedicationMapData> DeleteMedicationMaps(PutDeleteMedMapDataRequest request)
        {
            try
            {
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);

                if (request.MedicationMaps != null)
                {
                    request.MedicationMaps.ForEach(m =>
                    {
                        string id = (string)repo.Find(m);
                        if (!string.IsNullOrEmpty(id))
                        {
                            m.Id = id;
                            repo.Delete(id);
                        }
                    });
                }
                return request.MedicationMaps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMedicationMaps(DeleteMedicationMapsDataRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Ids))
                {
                    var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);
                    string[] Ids = request.Ids.Split(',');
                    foreach (string id in Ids)
                    {
                        DeleteMedicationMapsDataRequest deleteReq = new DeleteMedicationMapsDataRequest
                        {
                            Id = id.Trim(),
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        repo.Delete(deleteReq);
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   
