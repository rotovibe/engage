using System;
using Phytel.Engage.Integrations.Configurations;
using Phytel.Engage.Integrations.DTO;

namespace Phytel.Engage.Integrations.Specifications
{
    public class ParseToDosSpecification<T> : Specification<T>, IParseToDosSpecification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            var result = false;
            try
            {
                var contract = obj.ToString().ToLower();
                switch (contract)
                {
                    case "hillcrest001":
                        result = false;
                        break;
                    case "orlandohealth001":
                        result = true;
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("ParseToDosSpecification:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
