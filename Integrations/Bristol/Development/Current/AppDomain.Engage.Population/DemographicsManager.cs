using AppDomain.Engage.Population.DTO.Context;

namespace AppDomain.Engage.Population
{
    public class DemographicsManager : IDemographicsManager
    {
        private IServiceContext _context;

        public DemographicsManager(IServiceContext context)
        {
            _context = context;
        }

        public string DoSomething()
        {
            return _context.Contract;
        }
    }
}
