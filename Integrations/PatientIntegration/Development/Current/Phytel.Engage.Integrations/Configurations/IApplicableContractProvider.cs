using System.Collections.Generic;

namespace Phytel.Engage.Integrations.Configurations
{
    public interface IApplicableContractProvider
    {
        List<string> Contracts { get; set; }
        bool Exists(string name);
    }
}