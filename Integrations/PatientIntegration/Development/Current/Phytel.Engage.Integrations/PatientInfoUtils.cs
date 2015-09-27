using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations
{
    public static class PatientInfoUtils
    {
        public static int GetDeceased(string p)
        {
            var val = 0;
            //None = 0,
            //Yes = 1,
            //No = 2
            if (!p.Equals("Deceased")) return val;

            switch (p)
            {
                case "Deceased":
                    {
                        val = 1;
                        break;
                    }
                default:
                    {
                        val = 0;
                        break;
                    }
            }
            return val;
        }

        public static int GetStatus(string p)
        {
            int val;
            //Active = 1,
            //Inactive = 2,
            //Archived = 3

            switch (p)
            {
                case "Active":
                    {
                        val = 1;
                        break;
                    }
                case "Inactive":
                    {
                        val = 2;
                        break;
                    }
                case "Deceased":
                    {
                        val = 1;
                        break;
                    }
                case "Bad Debt":
                    {
                        val = 1;
                        break;
                    }
                default:
                    {
                        val = 1;
                        break;
                    }
            }
            return val;
        }
    }
}
