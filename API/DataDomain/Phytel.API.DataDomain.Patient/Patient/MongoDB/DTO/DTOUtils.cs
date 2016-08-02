using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.MongoDB.DTO
{
    public class DTOUtils : IDTOUtils
    {
        public List<SearchField> CloneAppDomainCohortPatientViews(List<Patient.DTO.SearchFieldData> list)
        {
            List<SearchField> melist = new List<SearchField>();

            try
            {
                if (list != null)
                {
                    list.ForEach(l =>
                    {
                        melist.Add(new SearchField
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
                throw new Exception("PatientDD:CloneAppDomainCohortPatientViews()::" + ex.Message, ex.InnerException);
            }
        }

    }
}
