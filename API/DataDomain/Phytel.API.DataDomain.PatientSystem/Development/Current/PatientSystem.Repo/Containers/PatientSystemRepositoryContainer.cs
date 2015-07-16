using AutoMapper;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Repo
{
    public static class PatientSystemRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            //container.Register<IMongoPatientSystemRepository>(Constants.Domain, c =>
            //    new MongoPatientSystemRepository<PatientSystemMongoContext>(
            //        c.ResolveNamed<IUOWMongo<PatientSystemMongoContext>>(Constants.Domain)))
            //    .ReusedWithin(Funq.ReuseScope.Request);

            //container.Register<IMongoPatientSystemRepository>(Constants.Domain, c =>
            //    new MongoSystemSourceRepository<PatientSystemMongoContext>(
            //        c.ResolveNamed<IUOWMongo<PatientSystemMongoContext>>(Constants.Domain)))
            //    .ReusedWithin(Funq.ReuseScope.Request);
            
            Mapper.CreateMap<MEPatientSystem, PatientSystemData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            return container;
        }
    }
}
