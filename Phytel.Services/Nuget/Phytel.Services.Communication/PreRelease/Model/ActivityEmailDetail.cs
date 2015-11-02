
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Communication.DTO
{
    public class ActivityEmailDetail
    {
        public List<ActivityMedia> ActivityMedias { get; set; }
        public List<ActivityDetail> ActivityDetails { get; set; }
    }
}
