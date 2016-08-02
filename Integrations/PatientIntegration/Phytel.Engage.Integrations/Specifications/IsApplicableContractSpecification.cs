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
            var result = false;
            try
            {
                if (ContractProvider == null) throw new Exception("ContractProvider is null.");

                var message = (RegistryCompleteMessage) Convert.ChangeType(obj, typeof (T));

                if (message != null)
                    result = ContractProvider.Exists(message.ContractDataBase);
                else
                    throw new Exception("message is null.");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("IsApplicableContractSpecification:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
