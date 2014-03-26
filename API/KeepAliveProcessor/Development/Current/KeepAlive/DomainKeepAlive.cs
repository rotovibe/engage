using Phytel.Framework.ASE.Process;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;

namespace KeepAlive
{
    public class DomainKeepAlive : QueueProcessBase
    {
        public override void Execute(QueueMessage queueMessage)
        {
            try
            {
                XmlNodeList sites = base.Configuration.SelectNodes("//Phytel.ASE.Process/ProcessConfiguration/Sites/Site");
                foreach (XmlNode site in sites)
                {
                    string currentSite = string.Empty;
                    try
                    {
                        currentSite = site.InnerText;
                        KeepSiteAlive(currentSite);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.Message, Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.Low, currentSite);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.Low);
            }
        }

        private void KeepSiteAlive(string url)
        {
            Uri siteUri = new Uri(url);

            HttpClient getContactClient = GetHttpClient(siteUri);

            var getResponse = getContactClient.GetStringAsync(siteUri);
            var getResponseContent = getResponse.Result;
        }

        private HttpClient GetHttpClient(Uri uri)
        {
            HttpClient client = new HttpClient();

            string userId = "000000000000000000000000";

            client.DefaultRequestHeaders.Host = uri.Host;
            client.DefaultRequestHeaders.Add("x-Phytel-UserID", userId);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

    }
}
