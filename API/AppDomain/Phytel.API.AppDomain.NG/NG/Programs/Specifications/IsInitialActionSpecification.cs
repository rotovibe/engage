using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class IsInitialActionSpecification<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                bool satisfied = true;
                List<Actions> acts = new List<Actions>();
                Program p = (Program)Convert.ChangeType(obj, typeof(T));

                p.Modules.ForEach(m =>
                {
                    m.Actions.Where(a => a.Completed == true).ToList().ForEach(i => { acts.Add(i); });
                });

                if (acts.Count > 0) satisfied = false;

                return satisfied;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:Completed:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
