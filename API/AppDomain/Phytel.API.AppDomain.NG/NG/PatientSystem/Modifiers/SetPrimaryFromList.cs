using System;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.AppDomain.NG.PatientSystem.Modifiers
{
    public class SetPrimaryFromList : ModifierBase<List<DTO.PatientSystem>, List<DTO.PatientSystem>>
    {
        public override List<DTO.PatientSystem> Modify(List<DTO.PatientSystem> param)
        {
            try
            {
                var psList = param;
                SeedWithTempId(psList);

                var pList = psList.Where(r => r.Primary == true).ToArray();

                if (pList.Length == 0) return psList;

                var primary = pList.Last();
                psList.ForEach(r => { if (r != null && !r.Id.Equals(primary.Id)) r.Primary = false; });
                return psList;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:SetPrimaryFromList:Modify()" + ex.Message + " stacktrace: " + ex.StackTrace);
            }
        }

        public void SeedWithTempId(List<DTO.PatientSystem> psList)
        {
            try
            {
                var tid = -1;
                psList.ForEach(r => { if (r.Id == null) r.Id = tid--.ToString(); });
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:SetPrimaryFromList:SeedWithTempId()" + ex.Message + " stacktrace: " + ex.StackTrace);
            }
        }
    }
}
