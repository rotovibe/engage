﻿using Phytel.API.DataDomain.Security.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.DataDomain.Security
{
    public class APISessionMongoRepository<T> : ISecurityRepository<T>
    {
        protected SecurityMongoContext _objectContext;

        public APISessionMongoRepository(SecurityMongoContext context)
        {
            _objectContext = context;
        }

        public APIUser GetUser(string userName, string apiKey, string product)
        {
            throw new NotImplementedException();
        }

        public T Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public T InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Select(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
