using System;
using System.Collections.Generic;
using C3.Data;
using C3.Data.Enum;
using Microsoft.Http;
using C3.Domain.Repositories.Abstract;
using Atmosphere.Insight.Data.Services;

namespace C3.Domain.Repositories.Concrete.RestService
{
    public class RestProgramRepository : BaseRepository, IProgramRepository
    {
        #region IProgramRepository Members

        public IList<Program> GetReportFilters(int contractId, List<Group> groups, MeasureTypes type)
        {
            HttpQueryString queryString = GetDefaultQueryString();

            // ###################################### This service NEEDS to be moved to Rest.Site!!!! ##############################################
            Uri uri = GetServiceRequestUri(ServiceUriFormats.ComparisonReportFilters, "Insight", new object[] { contractId, (int)type });
            List<Program> programs = PostRESTData<List<Program>>(queryString, uri, groups);

            return programs;
        }

        #endregion
    }
}
