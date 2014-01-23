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
        List<ProblemData> GetAllProblems();
        List<ProblemData> SearchProblem(SearchProblemsDataRequest request);
        object FindObjectiveByID(string entityID);
        object FindCategoryByID(string entityID);
        List<LookUpData> GetAllCommModes();
        List<LookUpData> GetAllStates();
        List<LookUpData> GetAllTimesOfDays();
        List<LookUpData> GetAllTimeZones();
        List<CommTypeData> GetAllCommTypes();
        List<LanguageData> GetAllLanguages();
    }
}
