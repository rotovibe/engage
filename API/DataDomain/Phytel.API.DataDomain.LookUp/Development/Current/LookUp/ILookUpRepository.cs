using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp
{
    public interface ILookUpRepository<T> : IRepository<T>
    {
        object FindProblemByID(string entityID);
        IQueryable<T> GetAllProblems();
        IQueryable<T> SearchProblem(SearchProblemsDataRequest request);
    }
}
