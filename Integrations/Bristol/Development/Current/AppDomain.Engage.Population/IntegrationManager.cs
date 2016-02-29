using AppDomain.Engage.Population.DTO.Context;

namespace AppDomain.Engage.Population
{
    public class IntegrationManager : IIntegrationManager
    {
        private IServiceContext _context;

        public IntegrationManager(IServiceContext context)
        {
            _context = context;
        }

        public string DoSomething()
        {
            return _context.Contract;
        }
    }
}
