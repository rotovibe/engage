using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp
{
    public interface ILookUpRepository : IRepository
    {
        object FindProblemByID(string entityID);
        List<ProblemData> GetAllProblems();
        List<ProblemData> SearchProblem(SearchProblemsDataRequest request);
        object FindObjectiveByID(string entityID);
        object FindCategoryByID(string entityID);
        List<IdNamePair> GetAllCommModes();
        List<StateData> GetAllStates();
        List<IdNamePair> GetAllTimesOfDays();
        List<TimeZoneData> GetAllTimeZones();
        List<CommTypeData> GetAllCommTypes();
        List<LanguageData> GetAllLanguages();
        TimeZoneData GetDefaultTimeZone();
        List<IdNamePair> GetLookps(string type);
        List<ObjectiveData> GetAllObjectives();
        List<LookUpDetailsData> GetLookUpDetails(string type);
    }
}
