using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;
using AutoMapper;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public class MedicationMappingDataManager : IMedicationMappingDataManager
    {
        //public List<DTO.MedicationMappingData> GetMedicationMappingList(GetAllMedicationMappingsRequest request)
        //{
        //    try
        //    {
        //        List<DTO.MedicationData> result = null;
        //        var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.Medication);
        //        result = repo.SelectAll().Cast<DTO.MedicationData>().ToList<DTO.MedicationData>();
        //        return result;
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        public DTO.MedicationMapData InsertMedicationMapping(PutInsertMedicationMappingRequest request, DTO.MedicationMapData mm)
        {
            try
            {
                DTO.MedicationMapData result;
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.MedicationMapping);

                result = repo.Insert(request as object) as DTO.MedicationMapData;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
