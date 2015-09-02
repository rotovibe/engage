using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.PatientSystem.Modifiers
{
    public class SetSystemsAsNonPrimary : ModifierBase<List<DTO.PatientSystem>, List<PatientSystemData>>
    {
        public override List<DTO.PatientSystem> Modify(List<PatientSystemData> param)
        {
            try
            {
                var patientSystems = param;
                patientSystems.ForEach(b => b.Primary = false);
                var dList = new List<DTO.PatientSystem>();
                patientSystems.ForEach(r => dList.Add(Mapper.Map<DTO.PatientSystem>(r)));
                return dList;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:SetPatientSystemsAsNonPrimary():Modify()" + ex.Message + " stacktrace: " + ex.StackTrace);
            }
        }
    }
}
