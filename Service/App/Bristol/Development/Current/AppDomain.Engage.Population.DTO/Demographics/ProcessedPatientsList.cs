using System;
using System.Collections.Generic;

namespace AppDomain.Engage.Population.DTO.Demographics
{
    public class ProcessedData 
    {
        public string Id { get; set; }
        public string DataSource { get; set; }
        public string ExternalRecordId { get; set; }
        public string EngagePatientSystemValue { get; set; }

    }

    public class ProcessedPatientsList
    {
        public List<ProcessedData> InsertedPatients { get; set; }
        public List<ProcessedData> ErroredPatients { get; set; }
    }
}
