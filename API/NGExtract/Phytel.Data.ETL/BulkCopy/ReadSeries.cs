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
            var id = 0;
            if (dic.ContainsKey(p))
            {
                id = dic[p];
            }
            return id;
        }
    }
}
