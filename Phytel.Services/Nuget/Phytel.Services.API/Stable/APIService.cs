using Phytel.Services.Security;
using System;
using System.Xml;

namespace Phytel.Services.API
{
    public sealed class APIService
    {
        private static volatile APIService instance;
        private static object syncRoot = new Object();

        #region Instance Methods
        private APIService() { }

        public static APIService Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new APIService();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region API Objects

        public string GetURL(string apiName)
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(DataProtector.GetConfigurationPathAndFile());

            XmlNode returnNode = null;
            XmlNodeList nodeList = configDoc.SelectNodes("/configuration/api");

            foreach (XmlNode x in nodeList)
            {
                if (x.Attributes.GetNamedItem("name").Value.ToUpper() == apiName.ToUpper())
                {
                    returnNode = x;
                    break;
                }
            }
            return returnNode.SelectSingleNode("url").InnerText;
        }
        
        #endregion
    }
}
