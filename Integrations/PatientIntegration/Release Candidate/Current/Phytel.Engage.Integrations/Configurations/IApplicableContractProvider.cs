using System.Collections.Generic;

namespace Phytel.Engage.Integrations.Configurations
{
    public interface IApplicableContractProvider
    {
        bool Exists(string name);
    }
}