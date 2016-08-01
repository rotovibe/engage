using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Step.DTO;

namespace Phytel.API.DataDomain.Step
{
    public class Helper
    {
        /// <summary>
        /// Converts the name of the Enum to a friendly name that can be shown on the UI
        /// </summary>
        /// <param name="s">Status enum object</param>
        /// <returns>friendly string</returns>
        public static string ToFriendlyString(Status s)
        {
            switch (s)
            {
                case Status.Active:
                    return "Active";
                case Status.Inactive:
                    return "Inactive";
                case Status.InReview:
                    return "In Review";
                case Status.Met:
                    return "Met";
                case Status.NotMet:
                    return "Not Met";
            }
            return null;
        }
    }
}
