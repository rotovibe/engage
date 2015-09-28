using System;
using System.Linq;
using System.Reflection;
using Phytel.Engage.Integrations.Repo.Connections;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private const string _namespace = "Phytel.Engage.Integrations.Repo.Repositories.";
        private IRepository _repo = null;

        public IRepository GetRepository(string context, RepositoryType type)
        {
            var asb = Assembly.GetExecutingAssembly();
            var types = asb.GetTypes();
            var ob = types.ToList().Find(r => r.FullName.Equals(_namespace + type.ToString()));

            if (ob != null)
                _repo = (IRepository)Activator.CreateInstance(ob, context, new SQLConnectionProvider());

            return _repo;
        }
    }
}
