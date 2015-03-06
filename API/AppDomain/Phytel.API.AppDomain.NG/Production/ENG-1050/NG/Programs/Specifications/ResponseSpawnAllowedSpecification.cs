using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class ResponseSpawnAllowed<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T obj)
        {
            try
            {
                Step s = obj as Step;
                bool pass = false;
                if ((s.StepTypeId != 1) && (s.StepTypeId != 2) && (s.StepTypeId != 4)
                    && (s.StepTypeId != 7) && (s.StepTypeId != 11))
                {
                    pass = true;
                }
                return pass;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ResponseSpawnAllowed:IsSatisfiedBy()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
