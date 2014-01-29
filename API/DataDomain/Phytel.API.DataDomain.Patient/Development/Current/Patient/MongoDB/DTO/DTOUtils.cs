using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.MongoDB.DTO
{
    public static class DTOUtils
    {
        internal static List<MESearchField> CloneAppDomainCohortPatientViews(List<Patient.DTO.SearchFieldData> list)
        {
            List<MESearchField> melist = new List<MESearchField>();

            try
            {
                if (list != null)
                {
                    list.ForEach(l =>
                    {
                        melist.Add(new MESearchField
                        {
                            Active = l.Active,
                            FieldName = l.FieldName,
                            Value = l.Value
                        });
                    });
                }
                return melist;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:CloneAppDomainCohortPatientViews():" + ex.Message, ex.InnerException);
            }
        }
    }
}
