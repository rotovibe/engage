using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace PhytelCommService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class OCQEResultService : System.Web.Services.WebService
    {

        [WebMethod]
        public respObject_Array sendOCQEResults(reqObject_Array parameters)
        {
            respObject_Array returnResponse = new respObject_Array();
            returnResponse.gfResponse = new respObject[parameters.gfRequest.Length-1];
            int i = 0;

            foreach (reqObject rObject in parameters.gfRequest)
            {
                respObject resp = new respObject();
                resp.CallID = rObject.CallID;
                resp.returnStatus = "ACK";
                resp.returnString = "ACK";

                //Drop message on Queue

                returnResponse.gfResponse[i] = resp;
                i++;
            }

            return returnResponse;
        }
    }
}
