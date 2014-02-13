using MongoDB.Bson;
using Phytel.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.MongoDB
{
    public static class DTOUtil
    {

        internal static List<ObjectId> ConvertObjectId(List<string> list)
        {
            List<ObjectId> objIds = new List<ObjectId>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(l =>
                    {
                        objIds.Add(ObjectId.Parse(l));
                    });
                }
                return objIds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static List<MongoDB.DTO.TaskAttribute> GetTaskAttributes(List<PatientGoal.DTO.TaskAttribute> list)
        {
            List<MongoDB.DTO.TaskAttribute> ta = new List<MongoDB.DTO.TaskAttribute>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(t =>
                    {
                        ta.Add(new MongoDB.DTO.TaskAttribute
                        {
                            ControlType = (AttributeControlType)Enum.Parse(typeof(AttributeControlType), t.ControlType),
                            Name = t.Name,
                            Value = t.Value
                        });
                    });
                }
                return ta;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal static List<DTO.InterventionAttribute> GetInterventionAttributes(List<PatientGoal.DTO.InterventionAttribute> list)
        {
            List<MongoDB.DTO.InterventionAttribute> ta = new List<MongoDB.DTO.InterventionAttribute>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(t =>
                    {
                        ta.Add(new MongoDB.DTO.InterventionAttribute
                        {
                            ControlType = (AttributeControlType)Enum.Parse(typeof(AttributeControlType), t.ControlType),
                            Name = t.Name,
                            Value = t.Value
                        });
                    });
                }
                return ta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
