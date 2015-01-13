using System;
using System.Collections.Generic;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ReadSeries
    {
        protected readonly Dictionary<string, int> stepIdList = Utils.GetStepIdList();
        protected readonly Dictionary<string, int> userIdList = Utils.GetUserIdList();
        protected readonly Dictionary<string, int> patientIdList = Utils.GetPatientIdList();
        protected readonly Dictionary<string, int> observationIdList = Utils.GetObservationIdsList();

        protected int GetId(Dictionary<string, int> dic, string p)
        {
            try
            {
                var id = 0;
                if (dic.ContainsKey(p))
                {
                    id = dic[p];
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GetId():" + p);
            }
        }

        protected static DateTime? GetNullableDate(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return null;
            }
            else
            {
                return date;
            }
        }
    }
}
