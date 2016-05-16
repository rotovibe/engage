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
                    result = "55e47fb5d433232058923e86";
                    break;
                }
                case "MYSIS":
                {
                    result = "55e47fb3d433232058923e85";
                    break;
                }
                case "OHMRN":
                {
                    result = "55e47fb0d433232058923e84";
                    break;
                }
                case "Atmosphere":
                {
                    result = "55e47fb9d433232058923e87";
                    break;
                }
                case "Insurer":
                {
                    result = "55e47fbcd433232058923e88";
                    break;
                }
                case "Engage":
                {
                    result = "559d8453d433232ca04b3131";
                    break;
                }
                case "IDXFC":
                {
                    result = "5726a91898a47c2894af45b2";
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
