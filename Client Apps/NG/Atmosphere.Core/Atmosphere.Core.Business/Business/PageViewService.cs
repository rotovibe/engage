using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using C3.Data;
using C3.Business.Interfaces;
using Phytel.Framework.SQL.Data;
using Phytel.Framework.SQL;

namespace C3.Business
{
    public class PageViewService : ServiceBase, IPageViewService
    {
        private static volatile PageViewService _instance;
        private static object _syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public PageViewService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public static PageViewService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new PageViewService();
                    }
                }
                return _instance;
            }
        }

        #region Implementation of IPageViewService

        public PageView Save(PageView pageView)
        {
            try
            {
                pageView.ViewContainer = CleanEncodedXml(pageView.ViewContainer);
                
                pageView.ViewId = new SqlDataExecutor().Execute<int>(null, _dbConnName, StoredProcedure.SavePageView, Converter.ToInt, new object[]
                                                                                             {
                                                                                                 pageView.ViewId,
                                                                                                 pageView.Name,
                                                                                                 pageView.Description,
                                                                                                 pageView.UserId,
                                                                                                 pageView.ControlId,
                                                                                                 pageView.ContractId,
                                                                                                 pageView.ViewContainer,
                                                                                                 pageView.IsPageDefaultView,
                                                                                                 pageView.IsUserDefaultView
                                                                                             });


            }
            catch
            {
                return null;
            }

            return pageView;
        }

        private string CleanEncodedXml(string viewContainer)
        {
            string cleanVersion = viewContainer.Replace("lt;", "<");
            cleanVersion = cleanVersion.Replace("gt;", ">");
            cleanVersion = cleanVersion.Replace("quot;", "\"");
            cleanVersion = cleanVersion.Replace("amp;", "&");
            cleanVersion = cleanVersion.Replace("apos;", "'");
            return cleanVersion;
        }

        public IList<PageView> GetPageViews(Guid userId, int contractId)
        {
            var pageViews = new List<PageView>();
            try
            {
                var reader = QueryReader(null, _dbConnName, StoredProcedure.GetPageViews, userId, contractId);
                while (reader.Read())
                {
                    pageViews.Add(PageView.Build(reader));
                }
            }
            catch
            {
                //suppress error and return empty collection for now while developing infrastructure for this
                //TODO: handle this properly when everything is built out.
            }

            return pageViews;
        }

        public bool Delete(int viewId, Guid userId, int contractId, int controlId)
        {
            try
            {
                return new SqlDataExecutor().Execute<bool>(null, _dbConnName, StoredProcedure.DeletePageView, Converter.ToBool, new object[]
                                                                                             {
                                                                                                 viewId,
                                                                                                 userId,
                                                                                                 contractId,
                                                                                                 controlId
                                                                                             });
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion
    }
}
