using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common;

namespace Phytel.Engage.Integrations
{
    public static class PatientInfoUtils
    {
        public static DateTime CstConvert(DateTime time)
        {
            try
            {
                var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                var cstTime = TimeZoneInfo.ConvertTimeFromUtc(time, cstZone);
                return cstTime;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Utils: CstConvert():" + ex.Message);
            }
        }

        public static int GetDeceased(string p)
        {
            var val = 0;
            //None = 0,
            //Yes = 1,
            //No = 2
            if (p.IsNullOrEmpty()) return val;

            switch (p)
            {
                case "Deceased":
                    {
                        val = 1;
                        break;
                    }
                default:
                    {
                        val = 2;
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

        public static string FormatGender(string p)
        {
            try
            {
                var gender = "N";
                if (string.IsNullOrEmpty(p)) return gender;
                gender = p;
                return gender;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
