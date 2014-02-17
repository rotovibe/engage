using MongoDB.Bson;
using Phytel.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal
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

        internal static List<MAttribute> GetAttributes(List<PatientGoal.DTO.AttributeData> list)
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
                            Values = t.Values
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
                            Values = t.Values
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

        internal static List<AttributeData> ConvertToAttributeDataList(List<MAttribute> meAttributes)
        {
            List<AttributeData> attrDataList = null;
            if (meAttributes != null && meAttributes.Count != 0)
            {
                attrDataList = new List<AttributeData>();
                foreach (MAttribute ma in meAttributes)
                {
                    attrDataList.Add(new AttributeData { Name = ma.Name, Values = ma.Values, Order = ma.Order, ControlType = Enum.GetName(typeof(AttributeControlType), ma.ControlType) });
                }
            }
            return attrDataList;
        }
    }
}
