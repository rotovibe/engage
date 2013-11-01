using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.AppDomain.Security.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.AppDomain.Security
{
    public class APISessionRepository<T> : ISecurityRepository<T>
    {
        protected SecurityMongoContext _objectContext;

        public APISessionRepository(SecurityMongoContext context)
        {
            _objectContext = context;
        }

        public AuthenticateResponse LoginUser(string token)
        {
            throw new NotImplementedException();
        }

        public AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string apiKey, string productName)
        {
            AuthenticateResponse response = new AuthenticateResponse();

            //need to do a lookup against the APIKey collection to see if apiKey/Product combination exists
            MEAPIKey key = (from k in _objectContext.APIKeys where k.ApiKey == apiKey && k.Product == productName && k.IsActive == true select k).FirstOrDefault();
            if(key != null)
            {
                MEAPISession session = new MEAPISession { APIKey = apiKey, 
                                                            Product = productName, 
                                                            SessionLengthInMinutes = existingReponse.SessionTimeout, 
                                                            SessionTimeOut = DateTime.Now.AddMinutes(existingReponse.SessionTimeout), 
                                                            UserName = existingReponse.UserName };

                _objectContext.APISessions.Collection.Insert(session);

                response = existingReponse;
                response.APIToken = session.Id.ToString();
            }

            return response;
        }

        public ValidateTokenResponse Validate(string token)
        {
            ValidateTokenResponse response = new ValidateTokenResponse();
            response.IsValid = false;

            MEAPISession session = _objectContext.APISessions.Collection.FindOneById(ObjectId.Parse(token));
            if(session != null)
            {
                response.IsValid = true;
                session.SessionTimeOut = DateTime.Now.AddMinutes(session.SessionLengthInMinutes);
                _objectContext.APISessions.Collection.Save(session);
            }

            return response;
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
