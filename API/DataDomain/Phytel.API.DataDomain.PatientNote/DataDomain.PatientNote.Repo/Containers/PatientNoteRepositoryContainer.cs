using AutoMapper;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientNote.DTO;
using System.Linq;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public static class PatientNoteRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            // automappings
            Mapper.CreateMap<PatientUtilizationData, MEPatientUtilization>()
                .ForMember(d => d.VisitType, opt => opt.MapFrom(src => ObjectId.Parse(src.VisitTypeId)))
                .ForMember(d => d.SourceId, opt => opt.MapFrom(src => ObjectId.Parse(src.SourceId)))
                .ForMember(d => d.PatientId, opt => opt.MapFrom(src => ObjectId.Parse(src.PatientId)))
                .ForMember(d => d.ProgramIds, opt => opt.MapFrom(src => src.ProgramIds.ConvertAll(id => ObjectId.Parse(id))));

            Mapper.CreateMap<MEPatientUtilization, PatientUtilizationData>()
                .ForMember(d => d.PatientId, opt => opt.MapFrom(src => src.PatientId.ToString()))
                .ForMember(d => d.VisitTypeId, opt => opt.MapFrom(src => src.VisitType.ToString()))
                .ForMember(d => d.SourceId, opt => opt.MapFrom(src => src.SourceId.ToString()))
                .ForMember(d => d.PatientId, opt => opt.MapFrom(src => src.PatientId.ToString()))
                .ForMember(d => d.ProgramIds, opt => opt.MapFrom(src => src.ProgramIds.ConvertAll(id => id.ToString())));

            //container.Register<IMongoPatientNoteRepository>(Constants.Domain, c =>
            //    new MongoPatientNoteRepository<PatientNoteMongoContext>(
            //        c.ResolveNamed<IUOWMongo<PatientNoteMongoContext>>(Constants.Domain)))
            //    .ReusedWithin(Funq.ReuseScope.Request);

            //container.Register<IMongoPatientNoteRepository>(Constants.Domain, c =>
            //    new MongoPatientUtilizationRepository<PatientNoteMongoContext>(
            //        c.ResolveNamed<IUOWMongo<PatientNoteMongoContext>>(Constants.Domain)))
            //    .ReusedWithin(Funq.ReuseScope.Request);

            return container;
        }
    }
}
