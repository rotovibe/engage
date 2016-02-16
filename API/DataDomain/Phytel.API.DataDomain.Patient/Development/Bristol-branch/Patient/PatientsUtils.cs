using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientsUtils
    {
        public static SortByBuilder BuildSortByBuilder(string sortbyExpression)
        {
            try
            {
                // create sort order builder
                SortByBuilder builder = new SortByBuilder();
                string[] splitSortExpr = sortbyExpression.Split(',');
                foreach (string s in splitSortExpr)
                {
                    string[] splitSort = s.Split(':');
                    if (splitSort[1].Equals("1"))
                    {
                        builder.Ascending(new string[] { splitSort[0].ToString() });
                    }
                    else
                    {
                        builder.Descending(new string[] { splitSort[0].ToString() });
                    }
                }
                return builder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static List<DTO.SearchFieldData> GetSearchFields(List<DTO.SearchField> list)
        {
            try
            {
                List<DTO.SearchFieldData> sfd = new List<DTO.SearchFieldData>();
                if (list != null)
                {
                    list.ForEach(sf =>
                    {
                        sfd.Add(new DTO.SearchFieldData
                        {
                            Active = sf.Active,
                            FieldName = sf.FieldName,
                            Value = sf.Value
                        });
                    });
                }
                return sfd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Source, ex.InnerException);
            }
        }
    }
}
