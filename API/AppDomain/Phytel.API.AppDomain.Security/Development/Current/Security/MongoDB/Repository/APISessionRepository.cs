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

        public UserAuthenticateResponse LoginUser(string userName, string password, string apiKey, string productName)
        {
            try
            {
                UserAuthenticateResponse response = new UserAuthenticateResponse();
                MEAPISession session = null;

                //need to do a lookup against the APIKey collection to see if apiKey/Product combination exists
                MEAPIUser user = (from k in _objectContext.APIUsers where k.UserName == userName && k.ApiKey == apiKey && k.Product == productName && k.IsActive == true select k).FirstOrDefault();
                if (user != null)
                {
                    //validate password
                    Phytel.Services.DataProtector protector = new Services.DataProtector(Services.DataProtector.Store.USE_SIMPLE_STORE);
                    string dbPwd = protector.Decrypt(user.Password);
                    if (dbPwd.Equals(password))
                    {
                        session = new MEAPISession
                        {
                            APIKey = apiKey,
                            Product = productName,
                            SessionLengthInMinutes = user.SessionTimeout,
                            SessionTimeOut = DateTime.Now.AddMinutes(user.SessionTimeout),
                            UserName = user.UserName
                        };

                        _objectContext.APISessions.Collection.Insert(session);
                    }
                    else
                        throw new Exception("Login Failed!  Username and/or Password is incorrect");

                    response = new UserAuthenticateResponse { APIToken = session.Id.ToString(), Contracts = new List<ContractInfo>(), Name = user.UserName, SessionTimeout = user.SessionTimeout, UserName = user.UserName };
                }
                else
                    throw new Exception("Login Failed! Unknown Username/Password");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string apiKey, string productName)
        {
            try
            {
                AuthenticateResponse response = new AuthenticateResponse();

                //need to do a lookup against the APIKey collection to see if apiKey/Product combination exists
                MEAPIKey key = (from k in _objectContext.APIKeys where k.ApiKey == apiKey && k.Product == productName && k.IsActive == true select k).FirstOrDefault();
                if (key != null)
                {
                    MEAPISession session = new MEAPISession
                    {
                        APIKey = apiKey,
                        Product = productName,
                        SessionLengthInMinutes = existingReponse.SessionTimeout,
                        SessionTimeOut = DateTime.Now.AddMinutes(existingReponse.SessionTimeout),
                        UserName = existingReponse.UserName
                    };

                    _objectContext.APISessions.Collection.Insert(session);

                    response = existingReponse;
                    response.APIToken = session.Id.ToString();
                }
                else
                    throw new Exception("Login Failed! Unknown Username/Password");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ValidateTokenResponse Validate(string token, string productName)
        {
            try
            {
                ValidateTokenResponse response = new ValidateTokenResponse();
                response.IsValid = false;

                MEAPISession session = _objectContext.APISessions.Collection.FindOneById(ObjectId.Parse(token));

                var query = Query<MEAPISession>.EQ(e => e.Id, ObjectId.Parse(token));
                var update = Update<MEAPISession>.Set(e => e.SessionTimeOut, DateTime.Now.AddMinutes(session.SessionLengthInMinutes));

                if (session != null && session.Product.ToUpper().Equals(productName.ToUpper()))
                {
                    response.IsValid = true;
                    //session.SessionTimeOut = DateTime.Now.AddMinutes(session.SessionLengthInMinutes);
                    _objectContext.APISessions.Collection.Update(query, update);
                    //_objectContext.APISessions.Collection.Save(session);
                }
                else
                    throw new UnauthorizedAccessException("Security Token does not exist");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ValidateTokenResponse Validate(string token, string productName)
        //{
        //    try
        //    {
        //        ValidateTokenResponse response = new ValidateTokenResponse();
        //        response.IsValid = false;

        //        MEAPISession session = _objectContext.APISessions.Collection.FindOneById(ObjectId.Parse(token));
        //        if (session != null && session.Product.ToUpper().Equals(productName.ToUpper()))
        //        {
        //            response.IsValid = true;
        //            session.SessionTimeOut = DateTime.Now.AddMinutes(session.SessionLengthInMinutes);
        //            //_objectContext.APISessions.Update()
        //            _objectContext.APISessions.Collection.Save(session);
        //        }
        //        else
        //            throw new UnauthorizedAccessException("Security Token does not exist");

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public LogoutResponse Logout(string token, string context)
        {
            LogoutResponse response = new LogoutResponse();
            response.SuccessfulLogout = false;
            try
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEAPISession.IdProperty, ObjectId.Parse(token)));
                queries.Add(Query.EQ(MEAPISession.ProductProperty, context.ToUpper()));
                IMongoQuery mQuery = Query.And(queries);

                MEAPISession session = _objectContext.APISessions.Collection.FindOneById(ObjectId.Parse(token));
                if (session != null && session.Product.ToUpper().Equals(context.ToUpper()))
                {
                    _objectContext.APISessions.Collection.Remove(mQuery);
                    response.SuccessfulLogout = true;
                }
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        public object Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<T> entities)
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

        public Tuple<string, IQueryable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
