using System;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Http;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Diagnostics;
using System.Text;
using Atmosphere.Core;

namespace C3.Domain.Repositories.Concrete.RestService
{
    public class BaseRepository
    {
        #region Public Variables

        private string BaseServiceBaseAddress;
        private string ApiKey = ""; //"bda11d91-7ade-4da1-855d-24adfe39d174" //TODO: This needs to be pulled from webconfig

        #endregion

        #region Constructor

        public BaseRepository()
        {
            BaseServiceBaseAddress = ConfigurationManager.AppSettings.Get("Rest");
            ApiKey = ConfigurationManager.AppSettings.Get("ApiKey");
            Timeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("RestTimeout"));
        }

        #endregion

        #region Protected Methods
        protected HttpQueryString GetDefaultQueryString()
        {
            HttpQueryString queryString = new HttpQueryString();
            queryString.Add("apiKey", ApiKey);
            return queryString;
        }

        #endregion

        #region Public Properties
        public HttpWebResponse lastResponse = null;

        private int Timeout
        {
            get;
            set;
        }

        public void Output(string sWrite)
        {
            Debug.Write(sWrite + "\r\n");
        }

        public HttpContent ConvertDictionaryToHttpContent(Dictionary<string, string> dictionary)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, dictionary);
            byte[] b = stream.ToArray();
            return HttpContent.Create(b, "application/octet-stream");
        }

        #endregion

        #region Rest Base Method Calls
        protected T RequestRESTJsonData<T>(HttpQueryString queryString, Uri resourceURI)
        {
            try
            {
                //create your request with your base URL and QS
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(HttpQueryString.MakeQueryString(resourceURI, queryString));
                req.ContentType = "application/json";

                Output(req.Address.ToString());
                HttpWebResponse wr = req.GetResponse() as HttpWebResponse;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

                //T data = (T)ser.ReadObject((wr.GetResponseStream()));
                T data = wr.ReadAsJsonDataContract<T>(); // JsonContentExtensions.ReadAsJsonDataContract();

                return data;
            }
            catch (WebException exc)
            {
                lastResponse = (HttpWebResponse)exc.Response;
                Output("RequestRESTData error: " + exc.Message);

                if (exc.Response != null)
                {
                    throw new ApplicationException(((HttpWebResponse)exc.Response).StatusDescription);
                }
                else
                {
                    throw new ApplicationException(exc.Message); // added to deal with timeout errors where response is null
                }
            }

        }

        protected T RequestRESTData<T>(HttpQueryString queryString, Uri resourceURI)
        {
            try
            {
                //create your request with your base URL and QS
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(HttpQueryString.MakeQueryString(resourceURI, queryString));

                Output(req.Address.ToString());
                HttpWebResponse wr = req.GetResponse() as HttpWebResponse;

                T data = wr.ReadAsXmlDataContract<T>();

                return data;
            }
            catch (WebException exc)
            {
                lastResponse = (HttpWebResponse)exc.Response;
                Output("RequestRESTData error: " + exc.Message);

                if (exc.Response != null)
                {
                    throw new ApplicationException(((HttpWebResponse)exc.Response).StatusDescription);
                }
                else
                {
                    throw new ApplicationException(exc.Message); // added to deal with timeout errors where response is null
                }
            }

        }

        protected T PostRESTData<T>(HttpQueryString queryString, Uri resourceURI, object postObject)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(postObject.GetType());
                serializer.Serialize(stream, postObject);
                byte[] b = stream.ToArray();

                stream.Position = 0;
                //create your request with your base URL and Querystring
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(HttpQueryString.MakeQueryString(resourceURI, queryString));
                req.Timeout = Timeout;
                req.ServicePoint.ConnectionLeaseTimeout = 0;
                Output(req.Address.ToString());
                req.Credentials = CredentialCache.DefaultCredentials;
                req.ContentType = "text/xml";
                req.ContentLength = b.Length;
                req.Method = "POST";
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(b, 0, b.Length);
                dataStream.Close();
                HttpWebResponse wr = req.GetResponse() as HttpWebResponse;

                T data = wr.ReadAsXmlDataContract<T>();

                return data;
            }
            catch (WebException exc)
            {

                lastResponse = (HttpWebResponse)(exc.Response);

                //lastResponse.StatusDescription = exc.Message;
                Output("PostRESTData error: " + exc.Message);
                throw new ApplicationException(exc.Message);
            }

        }

        protected T PutRESTData<T>(HttpQueryString queryString, Uri resourceURI, object postObject)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(postObject.GetType());
                serializer.Serialize(stream, postObject);
                byte[] b = stream.ToArray();

                //create your request with your base URL and QS
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(HttpQueryString.MakeQueryString(resourceURI, queryString));
                req.Credentials = CredentialCache.DefaultCredentials;
                req.ServicePoint.ConnectionLeaseTimeout = 0;
                req.ContentType = "text/xml";
                req.ContentLength = b.Length;
                req.Method = "PUT";
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(b, 0, b.Length);
                dataStream.Close();
                Output(req.Address.ToString());
                HttpWebResponse wr = req.GetResponse() as HttpWebResponse;


                T data = wr.ReadAsXmlDataContract<T>();
                return data;
            }
            catch (WebException exc)
            {
                lastResponse = (HttpWebResponse)exc.Response;
                Output("PutRESTData error: " + exc.Message);

                if (exc.Response != null)
                {
                    throw new ApplicationException(((HttpWebResponse)exc.Response).StatusDescription);
                }
                else
                {
                    throw new ApplicationException(exc.Message); // added to deal with timeout errors where response is null
                }
            }

        }

        protected T DeleteRESTData<T>(HttpQueryString queryString, Uri resourceURI)
        {
            try
            {
                //create your request with your base URL and QS
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(HttpQueryString.MakeQueryString(resourceURI, queryString));
                req.Credentials = CredentialCache.DefaultCredentials;
                req.ContentType = "text/xml";
                req.Method = "DELETE";
                Output(req.Address.ToString());
                HttpWebResponse wr = req.GetResponse() as HttpWebResponse;

                T data = wr.ReadAsDataContract<T>();
                return data;
            }
            catch (WebException exc)
            {
                lastResponse = (HttpWebResponse)exc.Response;
                Output("DeleteRESTData error: " + exc.Message);

                if (exc.Response != null)
                {
                    throw new ApplicationException(((HttpWebResponse)exc.Response).StatusDescription);
                }
                else
                {
                    throw new ApplicationException(exc.Message); // added to deal with timeout errors where response is null
                }
            }

        }

        protected Uri GetServiceRequestUri(string serviceUriFormat, string application, object[] args = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(BaseServiceBaseAddress);

            if (!BaseServiceBaseAddress.EndsWith("/"))
            {
                builder.Append("/");
            }

            builder.Append(application);

            if (!string.IsNullOrWhiteSpace(application))
            {
                builder.Append("/");
            }

            if (args.HasItems())
            {
                builder.Append(string.Format(serviceUriFormat, args));
            }
            else 
            {
                builder.Append(serviceUriFormat);
            }

            return new Uri(builder.ToString());
        }

        #endregion
    }
    
    
    public static class HttpWebExtensions
    {
        #region Public Methods

        public static T ReadAsDataContract<T>(this HttpWebResponse response, DataContractSerializer serializer)
        {
            using (var stream = response.GetResponseStream())
            {

                if (response.ContentLength > 0)
                    return (T)serializer.ReadObject(stream);
                else
                    return default(T);

            }
        }

        public static T ReadAsDataContract<T>(this HttpWebResponse content)
        {
            return ReadAsDataContract<T>(content, new DataContractSerializer(typeof(T)));
        }

        public static T ReadAsDataContract<T>(this HttpWebResponse content, params Type[] extraTypes)
        {
            return ReadAsDataContract<T>(content, new DataContractSerializer(typeof(T), extraTypes));
        }

        public static T ReadAsXmlDataContract<T>(this HttpWebResponse response, XmlSerializer serializer)
        {
            using (var stream = response.GetResponseStream())
            {
                if (response.ContentLength > 0)
                    return (T)serializer.Deserialize(stream);
                else
                    return default(T);
            }
        }

        public static T ReadAsXmlDataContract<T>(this HttpWebResponse content)
        {
            return ReadAsXmlDataContract<T>(content, new XmlSerializer(typeof(T)));
        }

        public static T ReadAsXmlDataContract<T>(this HttpWebResponse content, params Type[] extraTypes)
        {
            return ReadAsXmlDataContract<T>(content, new XmlSerializer(typeof(T), extraTypes));
        }

        public static T ReadAsJsonDataContract<T>(this HttpWebResponse response, DataContractJsonSerializer serializer)
        {
            using (var stream = response.GetResponseStream())
            {
                if (response.ContentLength > 0)
                    return (T)serializer.ReadObject(stream);
                else
                    return default(T);
            }
        }

        public static T ReadAsJsonDataContract<T>(this HttpWebResponse content)
        {
            return ReadAsJsonDataContract<T>(content, new DataContractJsonSerializer(typeof(T)));
        }

        public static T ReadAsJsonDataContract<T>(this HttpWebResponse content, params Type[] extraTypes)
        {
            return ReadAsJsonDataContract<T>(content, new DataContractJsonSerializer(typeof(T), extraTypes));
        }

        #endregion

    }
}
