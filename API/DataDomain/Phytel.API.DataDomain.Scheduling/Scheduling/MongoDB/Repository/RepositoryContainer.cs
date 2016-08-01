using System;

namespace Phytel.API.DataDomain.ToDo.MongoDB.Repository
{
    public static class RepositoryContainer
    {
        public const string NamedUnitOfWorkToDo = "ToDoRepo";

        public static void Configure(Funq.Container container)
        {

            Func<Funq.Container, IToDoRepository> repositoryMongoToDo = c =>
            {
                IMongoUOW<ToDoMongoContext> uow =
                    c.ResolveNamed<IMongoUOW<ToDoMongoContext>>(NamedUnitOfWorkToDo);

                return new MongoToDoRepository<ToDoMongoContext>(uow);
            };

            container.Register<IToDoRepository>(NamedUnitOfWorkToDo, repositoryMongoToDo).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}
