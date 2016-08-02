using Phytel.Engage.Integrations.Configurations;

namespace Phytel.Engage.Integrations.Specifications
{
    public interface IIsApplicableContract<T>
    {
        IApplicableContractProvider ContractProvider { get; set; }
        bool IsSatisfiedBy(T obj);
    }
}