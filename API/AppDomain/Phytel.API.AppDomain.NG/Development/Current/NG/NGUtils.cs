using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public static class NGUtils
    {

        internal static bool IsDateValid(string p)
        {
            DateTime date;
            bool result = false;
            if (DateTime.TryParse(p, out date))
            {
                result = true;
            }

            return result;
        }

        public static List<GetPatientResponse> PopulateCohortPatientStubData()
        {
            List<GetPatientResponse> pts = new List<GetPatientResponse>();
            pts.Add(new GetPatientResponse
            {
                Patient = new Patient
                {
                    DOB = "09/12/1993",
                    FirstName = "Oscar",
                    Gender = "M",
                    LastName = "DeLahoya",
                    MiddleName = "Denni",
                    PreferredName = "Punchy",
                    Suffix = "Mr."
                }
            });

            pts.Add(new GetPatientResponse
            {
                Patient = new Patient
                {
                    DOB = "02/16/1980",
                    FirstName = "Jenny",
                    Gender = "F",
                    LastName = "Greer",
                    MiddleName = "Irene",
                    PreferredName = "Jen",
                    Suffix = "Mr."
                }
            });

            return pts;
        }
    }
}
