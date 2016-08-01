using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    public enum RepositoryType
    {
        ToDo,
        Schedule
    }

    public enum Status
    {
        Open = 1,
        Met,
        NotMet,
        Abandoned
    }

    public enum Priority
    {
        NotSet = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum ScheduleType
    {
       ToDo = 1     
    }
}
