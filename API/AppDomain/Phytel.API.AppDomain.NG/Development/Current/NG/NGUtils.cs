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
    }
}
