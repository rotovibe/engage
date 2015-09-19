using System;
using Phytel.Engage.Integrations.Configurations;
using Phytel.Engage.Integrations.DTO;

namespace Phytel.Engage.Integrations.Specifications
{
    public class IsApplicableContractSpecification<T> : Specification<T>, IIsApplicableContract<T>
    {
        public IApplicableContractProvider ContractProvider { get; set; }

        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                var message = (RegistryCompleteMessage) Convert.ChangeType(obj, typeof (T));
                var result = ContractProvider.Exists( message.ContractDataBase);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("IsApplicableContractSpecification:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
