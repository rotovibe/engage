using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class RemoveSelectedResponseSpecification<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                var s = obj as Step;
                var pass = s != null && ((s.StepTypeId == 10)|| 
                                         (s.StepTypeId == 3) || 
                                         (s.StepTypeId == 9) || 
                                         (s.StepTypeId == 6) || 
                                         (s.StepTypeId == 8) || 
                                         (s.StepTypeId == 2));
                return pass;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ResponseSpawnAllowed:RemoveSelectedResponseSpecification()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
