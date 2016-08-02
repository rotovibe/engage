using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Search.DTO;
using Phytel.API.Interface;

namespace DataDomain.Search.Repo
{
    public interface IMongoSearchRepository : IRepository
    {
        List<TextValuePair> Search(object entity);
    }
}