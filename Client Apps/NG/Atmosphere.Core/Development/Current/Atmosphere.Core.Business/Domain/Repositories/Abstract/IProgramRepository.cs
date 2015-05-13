using System.Collections.Generic;
using System.Linq;
using C3.Data;
using C3.Data.Enum;

namespace C3.Domain.Repositories.Abstract
{
    public interface IProgramRepository
    {   
        IList<Program> GetReportFilters(int contractId, List<Group> groups, MeasureTypes type);
    }
}
