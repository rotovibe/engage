using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Search
{
    public class SearchUtil : ISearchUtil
    {
        public List<MedicationMap> FilterFieldResultsByParams(GetMedFieldsRequest request, List<MedicationMap> matches)
        {
            try
            {
                if (request.Route != null)
                {
                    matches = matches.Where(l => l.Route == request.Route).ToList();
                }

                if (request.Form != null)
                {
                    matches = matches.Where(l => l.Form == request.Form).ToList();
                }

                if (request.Strength != null)
                {
                    matches = matches.Where(l => l.Strength == request.Strength).ToList();
                }
                return matches;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:FilterFieldResultsByParams()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
