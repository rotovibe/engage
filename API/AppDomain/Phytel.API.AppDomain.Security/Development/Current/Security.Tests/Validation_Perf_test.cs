using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.Security.DTO;
using System.Diagnostics;

namespace Phytel.API.AppDomain.Security.Services.Test
{
    [TestClass]
    public class Validation_Perf_tests
    {
        [TestMethod]
        public void HitIsUserValidated()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string token = "5327104ad6a4850adcc085ce";
            string securityUrl = "http://localhost:888/Security";
            string PhytelSecurityHeaderKey = "x-Phytel-Security";
            string additionalToken = "Engineer";
            int cycles = 25;
            int[] nums = Enumerable.Range(0, cycles).ToArray();

            Parallel.ForEach(nums, num =>
            //for (int i = 1; i <= 20; i++)
            {
                Stopwatch st = new Stopwatch();
                try
                {
                    
                    st.Start();

                    IRestClient client = new JsonServiceClient();

                    JsonServiceClient.HttpWebRequestFilter = x =>
                        x.Headers.Add(string.Format("{0}: {1}", PhytelSecurityHeaderKey, additionalToken));

                    ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/{2}/{3}/token",
                        securityUrl,
                        "NG",
                        version,
                        contractNumber),
                        new ValidateTokenRequest { Token = token } as object);

                    st.Stop();
                    Debug.WriteLine("thread" + num + " :: " + System.DateTime.Now.ToLongTimeString() + " :: " + st.ElapsedMilliseconds.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("thread" + num + " :: " + System.DateTime.Now.ToLongTimeString() + " :: " + st.ElapsedMilliseconds.ToString() + ": Exception:" + ex.Message);
                }
            }
            );
        }
    }
}
