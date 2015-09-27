using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Commands
{
    public class GetSendingApplicationId : Command<string, string>, IIntegrationCommand<string, string>
    {
        public override string Execute(string val)
        {
            var result = string.Empty;
            //AS-OH
            //MYSIS
            //OHMRN

            switch (val)
            {
                case "AS-OH":
                {
                    result = "5604728dfe7a5923a098ac9f";
                    break;
                }
                case "MYSIS":
                {
                    result = "5604728efe7a5923a098aca0";
                    break;
                }
                case "OHMRN":
                {
                    result = "5604728efe7a5923a098aca1";
                    break;
                }
                default:
                {
                    result = val;
                    break;
                }
            }

            return result;
        }
    }
}
