using System;
using System.Linq;
using System.Reflection;
using Phytel.Engage.Integrations.Repo.Bridge;
using Phytel.Engage.Integrations.Repo.Connections;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private const string Namespace = "Phytel.Engage.Integrations.Repo.Repositories.";
        private const string ImplementNameSpace = "Phytel.Engage.Integrations.Repo.Bridge.";
        private IRepository _repo = null;

        public IRepository GetRepository(string context, RepositoryType type)
        {
            var asb = Assembly.GetExecutingAssembly();
            var types = asb.GetTypes();
            var ob = types.ToList().Find(r => r.FullName.Equals(Namespace + type.ToString()));
            var imp = types.ToList().Find(r => r.FullName.Equals(ImplementNameSpace + context + "Implementation"));

            if (ob == null || imp == null) return _repo;
            var implementor = (IQueryImplementation) Activator.CreateInstance(imp);
            _repo = (IRepository) Activator.CreateInstance(ob, context, new SQLConnectionProvider(), implementor);

            return _repo;
        }
    }
}
