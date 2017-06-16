using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Specifications;

namespace Phytel.API.AppDomain.NG.Programs.Specifications
{
    public class NotEnabled<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                IPlanElement interv = (IPlanElement)Convert.ChangeType(obj, typeof(T));
                return !interv.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:Enabled:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
