using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using C3.Domain.Repositories.Abstract;
using C3.Data;


namespace C3.Domain.Repositories.Concrete.RestService
{
    public class RestPageViewRepository : BaseRepository, IPageViewRepository
    {
        #region Implementation of IPageViewRepository

        public IList<PageView> GetPageViews(Guid userId, int contractId, int controlId)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            var uri =
                new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", ContractsServiceBaseAddress, contractId, UsersBaseAddress,
                                      PageViewServiceBaseAddress, userId, controlId));
            var pageViews = RequestRESTData<List<PageView>>(queryString, uri);
            return pageViews;
        }

        public IList<PageView> GetDefaultPageViews(int contractId, int controlId)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            var uri =
                new Uri(string.Format("{0}/{1}/{2}/{3}/{4}", ContractsServiceBaseAddress, contractId, UsersBaseAddress,
                                      PageViewServiceBaseAddress, controlId));
            var pageViews = RequestRESTData<List<PageView>>(queryString, uri);
            return pageViews;
        }

        public PageView Save(PageView pageView)
        {
            if (pageView == null) return null;
            try
            {
                HttpQueryString queryString = GetDefaultQueryString();
                var uri = new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", ContractsServiceBaseAddress, pageView.ContractId, UsersBaseAddress,
                                          PageViewServiceBaseAddress, pageView.UserId, "Save"));
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
            var uri = new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", ContractsServiceBaseAddress, pageView.ContractId, UsersBaseAddress,
                                      PageViewServiceBaseAddress, pageView.UserId, "Delete"));
            var deleted = PostRESTData<bool>(queryString, uri, pageView);
            return deleted;
        }

        #endregion
    }
}
