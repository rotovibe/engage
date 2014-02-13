using MongoDB.Bson;
using Phytel.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.MongoDB;
using Phytel.API.DataDomain.PatientGoal.DTO;

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

        internal static List<MAttribute> GetTaskAttributes(List<PatientGoal.DTO.AttributeData> list)
        {
            List<MAttribute> ta = new List<MAttribute>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(t =>
                    {
                        ta.Add(new MAttribute
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

        internal static List<MAttribute> GetInterventionAttributes(List<PatientGoal.DTO.AttributeData> list)
        {
            List<MAttribute> ta = new List<MAttribute>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(t =>
                    {
                        ta.Add(new MAttribute
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
