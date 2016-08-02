using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;

namespace C3.Business.Interfaces
{
    public interface IPageViewService
    {
        PageView Save(PageView pageView);

        IList<PageView> GetPageViews(Guid userId, int contractId);

        bool Delete(int viewId, Guid userId, int contractId, int controlId);
    }
}
