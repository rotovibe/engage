using System.Collections.Generic;
using System.Linq;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;
using System;
using AutoMapper;
using Phytel.API.Interface;
using System.Configuration;

namespace Phytel.API.DataDomain.Medication
{
    public class PatientMedFrequencyDataManager : IPatientMedFrequencyDataManager
    {
        public List<PatientMedFrequencyData> GetPatientMedFrequencies(GetPatientMedFrequenciesDataRequest request)
        {
            try
            {
                List<PatientMedFrequencyData> result = null;
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedFrequency);
                result = repo.FindByPatientId(request) as List<PatientMedFrequencyData>;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public string InsertPatientMedFrequency(PostPatientMedFrequencyDataRequest request)
        {
            string id = null;
            try
            {
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedFrequency);
                if (request.PatientMedFrequencyData != null)
                {
                    id = repo.Insert(request as object) as string;
                }
                return id;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   
