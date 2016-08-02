using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using C3.Domain.Repositories.Abstract;
using C3.Data;
using Atmosphere.Core.Data.Services;


namespace C3.Domain.Repositories.Concrete.RestService
{
    public class RestPageViewRepository : BaseRepository, IPageViewRepository
    {
        #region Implementation of IPageViewRepository

        public IList<PageView> GetPageViews(Guid userId, int contractId, int controlId)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            var uri = GetServiceRequestUri(ServiceUriFormats.PageViewList, ServiceUriFormats.Application, new object[] { contractId, userId, controlId });
            var pageViews = RequestRESTData<List<PageView>>(queryString, uri);
            return pageViews;
        }

        public IList<PageView> GetDefaultPageViews(int contractId, int controlId)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            var uri = GetServiceRequestUri(ServiceUriFormats.DefaultPageViewList, ServiceUriFormats.Application, new object[] { contractId, controlId });
            var pageViews = RequestRESTData<List<PageView>>(queryString, uri);
            return pageViews;
        }

        public PageView Save(PageView pageView)
        {
            if (pageView == null) return null;
            try
            {
                HttpQueryString queryString = GetDefaultQueryString();
                var uri = GetServiceRequestUri(ServiceUriFormats.PageViewSave, ServiceUriFormats.Application, new object[] {pageView.ContractId, pageView.UserId });
                var savedPageView = PostRESTData<PageView>(queryString, uri, pageView);
                return savedPageView;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Delete(PageView pageView)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            var uri = GetServiceRequestUri(ServiceUriFormats.PageViewDelete, ServiceUriFormats.Application, new object[] { pageView.ContractId, pageView.UserId });
            var deleted = PostRESTData<bool>(queryString, uri, pageView);
            return deleted;
        }

        #endregion
    }
}
