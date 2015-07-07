using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class TakeModifier : ModifierBase
    {
        private int Count { get; set; }

        public TakeModifier(int take)
        {
            Count = take;
        }

        public override List<PatientNote> Modify(List<PatientNote> result)
        {
            try
            {
                List<PatientNote> list = result;

                if (Count > 0)
                    list = result.Take(Count).ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("TakeModifier:Modify()" + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
