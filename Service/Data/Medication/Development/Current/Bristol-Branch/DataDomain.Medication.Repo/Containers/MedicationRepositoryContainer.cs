using AutoMapper;
using MongoDB.Bson;
using Phytel.API.DataDomain.Medication.DTO;

namespace DataDomain.Medication.Repo.Containers
{
    public static class MedicationRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            Mapper.CreateMap<MEMedication, MedicationData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            Mapper.CreateMap<MedicationData, MEMedication>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.RecordCreatedBy)))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.UpdatedBy)));


            Mapper.CreateMap<MEMedicationMapping, MedicationMapData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            Mapper.CreateMap<MedicationMapData, MEMedicationMapping>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.RecordCreatedBy)))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.UpdatedBy)));

            Mapper.CreateMap<MEPatientMedSupp, PatientMedSuppData>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn));

            container.Register<IUOWMongo<MedicationMongoContext>>(Constants.Domain, c =>
                new UOWMongo<MedicationMongoContext>(
                    c.ResolveNamed<string>(Constants.NamedString)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);


            container.Register<IMongoMedicationRepository>(Constants.Domain, c =>
                new MongoMedicationRepository<MedicationMongoContext>(
                    c.ResolveNamed<IUOWMongo<MedicationMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMongoPatientMedSuppRepository>(Constants.Domain, c =>
                new MongoPatientMedSuppRepository<MedicationMongoContext>(
                    c.ResolveNamed<IUOWMongo<MedicationMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            Mapper.CreateMap<PatientMedFrequencyData, MEPatientMedFrequency>();
            Mapper.CreateMap<MEPatientMedFrequency, PatientMedFrequencyData>();

            return container;
        }
    }
}
