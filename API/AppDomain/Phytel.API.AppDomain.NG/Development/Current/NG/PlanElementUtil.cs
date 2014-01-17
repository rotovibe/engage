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
        /// <summary>
        /// Checks to see if completion status count is equal to the list count. Return true if equal.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">PlanElement list</param>
        /// <returns></returns>
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
            var mod = list.Where(r => ((IPlanElement)Convert.ChangeType(r, typeof(T))).Id == id).FirstOrDefault();
            return mod;
        }

        public static void SetEnabledStatusByPrevious<T>(List<T> actions)
        {
            try
            {
                if (actions != null)
                {
                    actions.ForEach(x =>
                    {
                        SetEnabledState(actions, x);
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:SetEnabledStatusByPrevious():" + ex.Message, ex.InnerException);
            }
        }

        public static void SetEnabledState<T>(List<T> list, T x)
        {
            try
            {
                if (list != null && x != null)
                {
                    IPlanElement pe = ((IPlanElement)Convert.ChangeType(x, typeof(T)));

                    if (!string.IsNullOrEmpty(pe.Previous))
                    {
                        var prevElem = list.Find(r => ((IPlanElement)r).Id == pe.Previous);
                        if (prevElem != null)
                        {
                            if (((IPlanElement)prevElem).Completed != true)
                            {
                                ((IPlanElement)x).Enabled = false;
                            }
                            else
                            {
                                ((IPlanElement)x).Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:SetEnabledState():" + ex.Message, ex.InnerException);
            }
        }

        internal static void DisableCompleteButtonForAction(List<Step> list)
        {
            try
            {
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        if (s.StepTypeId.Equals(7))
                        {
                            s.Enabled = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:DisableCompleteButtonForAction():" + ex.Message, ex.InnerException);
            }
        }

        internal static void SpawnElementsInList(List<SpawnElement> list, Program program)
        {
            try
            {
                list.ForEach(r =>
                {
                    SetEnabledStateRecursion(r.ElementId, program);
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:SpawnElementsInList():" + ex.Message, ex.InnerException);
            }
        }

        private static void SetEnabledStateRecursion(string p, Program program)
        {
            foreach (Module m in program.Modules)
            {
                if (m.Id.ToString().Equals(p))
                {
                    SetInitialProperties(m);
                }
                else
                {
                    FindIdInActions(p, m);
                }
            }
        }

        private static void FindIdInActions(string p, Module m)
        {
            if (m.Actions != null)
            {
                foreach (Actions a in m.Actions)
                {
                    if (a.Id.ToString().Equals(p))
                    {
                        SetInitialProperties(a);
                    }
                    else
                    {
                        FindIdInSteps(p, a);
                    }
                }
            }
        }

        private static void FindIdInSteps(string p, Actions a)
        {
            if (a.Steps != null)
            {
                foreach (Step s in a.Steps)
                {
                    if (s.Id.ToString().Equals(p))
                    {
                        SetInitialProperties(s);
                    }
                }
            }
        }

        private static void SetInitialProperties(IPlanElement m)
        {
            m.Enabled = true;
            m.AssignDate = System.DateTime.UtcNow;
            m.ElementState = 0;
            m.AssignBy = "System";
        }
    }
}
