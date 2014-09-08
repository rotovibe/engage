using Phytel.API.DataDomain.Template.DTO;
using Phytel.Repository;

namespace DataDomain.Template.Repo.Containers
{
    public static class TemplateRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container.Register<IUOWMongo<TemplateMongoContext>>(Constants.Domain, c =>
                new UOWMongo<TemplateMongoContext>(
                    c.ResolveNamed<string>(Constants.NamedString)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMongoTemplateRepository>(Constants.Domain, c =>
                new MongoTemplateRepository<TemplateMongoContext>(
                        c.ResolveNamed<IUOWMongo<TemplateMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);


            return container;
        }
    }
}
