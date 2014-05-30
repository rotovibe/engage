using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanElementStrategy
{
    public class SpawnElementStrategy
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
