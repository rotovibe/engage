using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public static class PlanElementUtil
    {
        public static bool SetCompletionStatus<T>(List<T> list)
        {
            bool result = false;
            int completed = list.FindAll(new Completed<T>().IsSatisfiedBy).Count();
            int count = list.Count;

            if (completed.Equals(count))
            {
                result = true;
            }
            return result;
        }

        public static T FindElementById<T>(List<T> list, string id)
        {
            var mod = list.Where(r => ((IPlanElement)Convert.ChangeType(r, typeof(T))).Id == id).First();
            return mod;
        }

        public static void SetEnabledStatus<T>(List<T> actions)
        {
            actions.ForEach(x =>
            {
                ((IPlanElement)Convert.ChangeType(x, typeof(T))).Enabled = true;

                SetEnabledState(actions, x);
            });
        }

        public static void SetEnabledState<T>(List<T> list, T x)
        {
            IPlanElement pe = ((IPlanElement)Convert.ChangeType(x, typeof(T)));

            if (!string.IsNullOrEmpty(pe.Previous))
            {
                var prevElem = list.Find(r => ((IPlanElement)r).Id == pe.Previous);
                if (((IPlanElement)prevElem).Completed != true)
                {
                    ((IPlanElement)x).Enabled = false;
                }
            }
        }
    }
}
