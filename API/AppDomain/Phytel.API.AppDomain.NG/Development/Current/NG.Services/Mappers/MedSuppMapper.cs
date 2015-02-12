using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Common;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class MedSuppMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<MedicationMapData, MedicationMap>();
            Mapper.CreateMap<MedicationMapData, DTO.MedicationMap>();
            Mapper.CreateMap<DTO.MedicationMap, MedicationMapData>();
            Mapper.CreateMap<PatientMedSuppData, PatientMedSupp>();
            Mapper.CreateMap<PatientMedSupp, PatientMedSuppData>();
        }
    }
}