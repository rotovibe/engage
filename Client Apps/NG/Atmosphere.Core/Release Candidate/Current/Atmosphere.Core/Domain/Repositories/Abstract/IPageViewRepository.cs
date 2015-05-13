using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;

namespace C3.Domain.Repositories.Abstract
{
    public interface IPageViewRepository
    {
        IList<PageView> GetPageViews(Guid userId, int contractId, int controlId);
        IList<PageView> GetDefaultPageViews(int contractId, int controlId);
        PageView Save(PageView pageView);
        bool Delete(PageView pageView);
    }
}
