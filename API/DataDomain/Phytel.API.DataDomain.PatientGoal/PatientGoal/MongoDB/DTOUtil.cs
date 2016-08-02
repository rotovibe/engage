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

        internal static List<MAttribute> GetAttributes(List<PatientGoal.DTO.CustomAttributeData> list)
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
                            AttributeId = ObjectId.Parse(t.Id),
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

        internal static List<CustomAttributeData> GetCustomAttributeIdAndValues(List<MAttribute> meAttributes)
        {
            List<CustomAttributeData> customAttrDataList = null;
            if (meAttributes != null && meAttributes.Count != 0)
            {
                customAttrDataList = new List<CustomAttributeData>();
                foreach (MAttribute ma in meAttributes)
                {
                    customAttrDataList.Add( new CustomAttributeData { Id = ma.AttributeId.ToString(), Values = ma.Values });
                }
            }
            return customAttrDataList;
        }
    }
}
