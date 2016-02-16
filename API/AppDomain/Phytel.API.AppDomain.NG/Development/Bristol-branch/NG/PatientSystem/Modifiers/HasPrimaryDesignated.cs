using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.PatientSystem.Modifiers
{
    public class HasPrimaryDesignated : ModifierBase<bool, List<DTO.PatientSystem>>
    {
        public override bool Modify(List<DTO.PatientSystem> param)
        {
            try
            {
                var result = false;
                var count = 0;
                param.ForEach(r => { if (r.Primary == true) count = 1; });

                if (count > 0) result = true;
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:HasPrimaryDesignated:Modify()" + ex.Message + " stacktrace: " + ex.StackTrace);
            }
        }
    }
}
