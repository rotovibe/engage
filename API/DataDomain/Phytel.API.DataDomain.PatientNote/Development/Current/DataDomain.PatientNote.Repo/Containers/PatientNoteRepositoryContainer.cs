using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public static class PatientNoteRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {

            container.Register<IMongoPatientNoteRepository>(Constants.Domain, c =>
                new MongoPatientNoteRepository<PatientNoteMongoContext>(
                    c.ResolveNamed<IUOWMongo<PatientNoteMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            return container;
        }
    }
}
