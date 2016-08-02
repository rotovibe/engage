using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.LookUp
{
    public interface ILookUpDataManager
    {
        GetProblemDataResponse GetProblem(GetProblemDataRequest request);
        GetAllProblemsDataResponse GetAllProblems(GetAllProblemsDataRequest request);
        SearchProblemsDataResponse SearchProblem(SearchProblemsDataRequest request);
        GetObjectiveDataResponse GetObjectiveByID(GetObjectiveDataRequest request);
        GetCategoryDataResponse GetCategoryByID(GetCategoryDataRequest request);
        GetAllCommModesDataResponse GetAllCommModes(GetAllCommModesDataRequest request);
        GetAllStatesDataResponse GetAllStates(GetAllStatesDataRequest request);
        GetAllTimesOfDaysDataResponse GetAllTimesOfDays(GetAllTimesOfDaysDataRequest request);
        GetAllTimeZonesDataResponse GetAllTimeZones(GetAllTimeZonesDataRequest request);
        GetAllCommTypesDataResponse GetAllCommTypes(GetAllCommTypesDataRequest request);
        GetAllLanguagesDataResponse GetAllLanguages(GetAllLanguagesDataRequest request);
        GetTimeZoneDataResponse GetDefaultTimeZone(GetTimeZoneDataRequest request);
        GetLookUpsDataResponse GetLookUpsByType(GetLookUpsDataRequest request);
        GetLookUpDetailsDataResponse GetLookUpDetails(GetLookUpDetailsDataRequest request);
        GetAllObjectivesDataResponse GetAllObjectives(GetAllObjectivesDataRequest request);
        GetAllSettingsDataResponse GetAllSettings(GetAllSettingsDataRequest request);
    }
}
