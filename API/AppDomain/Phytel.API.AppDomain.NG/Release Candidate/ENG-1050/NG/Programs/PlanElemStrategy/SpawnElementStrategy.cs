using Phytel.API.AppDomain.NG.ElementActivation;

namespace Phytel.API.AppDomain.NG.PlanElementStrategy
{
    public class SpawnElementStrategy : IPlanElementStrategy
    {
        private readonly ISpawn _spawn;

        public SpawnElementStrategy(ISpawn spawn)
        {
            _spawn = spawn;
        }

        public void Evoke()
        {
            _spawn.Execute();
        }
    }
}
