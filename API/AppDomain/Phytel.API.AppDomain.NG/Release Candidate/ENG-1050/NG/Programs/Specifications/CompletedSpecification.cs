using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class Completed<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                IPlanElement interv = (IPlanElement)Convert.ChangeType(obj, typeof(T));
                return interv.Completed;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:Completed:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
