using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.DataDomain.Medication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Tests
{
    [TestClass()]
    public class MedicationMappingDataManagerTests
    {
        [TestClass()]
        public class InsertMedicationMappingTest
        {
            [TestMethod()]
            public void InsertMedicationMapping()
            {
                Mapper.CreateMap<MEMedicationMapping, DTO.MedicationMapData>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                    .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                    .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                    .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                    .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

                Mapper.CreateMap<DTO.MedicationMapData, MEMedicationMapping>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
                    .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                    .ForMember(dest => dest.RecordCreatedBy,
                        opt => opt.MapFrom(src => ObjectId.Parse(src.RecordCreatedBy)))
                    .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                    .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.UpdatedBy)));

                var mgr = new MedicationMappingDataManager();

                var mmap = new DTO.MedicationMapData
                {
                    Form = "Tab",
                    FullName = "SassyPants",
                    Route = "Oral",
                    Strength = "22 mg",
                    SubstanceName = "Sassy Substance"
                };

                var request = new PostMedicationMapDataRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "5325c81f072ef705080d347e",
                    Version = 1,
                    MedicationMapData = mmap
                };

                var result = mgr.InsertMedicationMap(request);

                Assert.IsTrue(result != null);
            }
        }
    }
}
