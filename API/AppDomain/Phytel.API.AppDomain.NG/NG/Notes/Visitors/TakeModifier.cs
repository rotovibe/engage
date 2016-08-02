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

        public override List<PatientNote> Modify(ref List<PatientNote> result)
        {
            try
            {
                // might not be honoring the order by in the reference list, so clone it.
                List<PatientNote> list = result.ToList();

                if (result.Count > 0)
                {
                    if (Count > 0)
                    {
                        list = list.Take(Count).ToList();
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("TakeModifier:Modify()" + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
