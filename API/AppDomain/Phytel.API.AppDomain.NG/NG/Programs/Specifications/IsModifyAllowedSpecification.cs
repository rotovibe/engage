using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class IsModifyAllowedSpecification<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                var pe = obj as PlanElement;
                var result = false;

                if (pe.ElementState == 2 || pe.ElementState == 4)
                    result = true;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:IsModifyAllowedSpecification()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
