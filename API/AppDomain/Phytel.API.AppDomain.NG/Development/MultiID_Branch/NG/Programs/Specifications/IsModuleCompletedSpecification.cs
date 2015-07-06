using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class IsModuleCompletedSpecification<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                var m = obj as Module;
                var pass = m != null && m.Completed;
                return pass;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ResponseSpawnAllowed:IsModuleCompletedSpecification()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
