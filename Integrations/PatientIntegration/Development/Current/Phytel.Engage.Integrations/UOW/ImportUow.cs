using System;
using System.Linq;
using Phytel.Engage.Integrations.Extensions;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientsImportUow<T> : UowBase<T>, IImportUOW<T>
    {
        public void Commit()
        {
            var take = Convert.ToInt32(Math.Round(Pocos.Count * .25, 0, MidpointRounding.AwayFromZero));
            var count = 0;
            var pages = Pocos.Pages(take);
            if (Pocos.Count <= take) return;

            for (var i = 0; i <= pages; i++)
            {
                if (count == Pocos.Count) break;
                var savePatients = Pocos.Batch(take).ToList()[i];
                count = count + savePatients.Count();
                // send to bulk DD to batch.
            }
        }
    }
}
