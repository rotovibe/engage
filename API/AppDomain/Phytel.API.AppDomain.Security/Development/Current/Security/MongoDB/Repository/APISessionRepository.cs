using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
                            SessionLengthInMinutes = user.SessionLengthInMinutes,
                            SessionTimeOut = DateTime.Now.AddMinutes(user.SessionLengthInMinutes),
                            UserName = user.UserName,
                            UserId = user.Id.ToString()
                        };

                        _objectContext.APISessions.Collection.Insert(session);
                    }
                    else
                        throw new Exception("Login Failed!  Username and/or Password is incorrect");

                    response = new UserAuthenticateResponse 
                                    { 
                                        APIToken = session.Id.ToString(), 
                                        Contracts = new List<ContractInfo>(), 
                                        Name = user.UserName, 
                                        SessionTimeout = user.SessionLengthInMinutes, 
                                        UserName = user.UserName 
                                    };
                }
                else
                    throw new Exception("Login Failed! Unknown Username/Password");

                return response;
            }
            catch (Exception)
            {
                throw;
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
                        UserName = existingReponse.UserName,
                        UserId = existingReponse.UserID.ToString()
                    };

                    _objectContext.APISessions.Collection.Insert(session);

                    response = existingReponse;
                    response.APIToken = session.Id.ToString();
                }
                else
                    throw new Exception("Login Failed! Unknown Username/Password");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ValidateTokenResponse Validate(string token, string productName)
        {
            try
            {
                ValidateTokenResponse response = null;

                int sessionLengthInMinutes = (from s in _objectContext.APISessions
                                              where s.Id == ObjectId.Parse(token)
                                              select s.SessionLengthInMinutes).FirstOrDefault();

                if (sessionLengthInMinutes > 0)
                {
                    FindAndModifyResult result = _objectContext.APISessions.Collection.FindAndModify(Query.EQ(MEAPISession.IdProperty, ObjectId.Parse(token)), SortBy.Null,
                                                MongoDB.Driver.Builders.Update.Set(MEAPISession.SessionTimeOutProperty, DateTime.Now.AddMinutes(sessionLengthInMinutes)), true);

                    if (result != null && result.ModifiedDocument != null)
                    {
                        MEAPISession session = BsonSerializer.Deserialize<MEAPISession>(result.ModifiedDocument);
                        response = new ValidateTokenResponse
                                        {
                                            SessionLengthInMinutes = session.SessionLengthInMinutes,
                                            SessionTimeOut = session.SessionTimeOut,
                                            TokenId = session.Id.ToString(),
                                            UserId = session.UserId,
                                            UserName = session.UserName
                                        };
                    }
                    else
                        throw new UnauthorizedAccessException("Security Token does not exist");

                    return response;
                }
                else
                    throw new UnauthorizedAccessException("Security Token does not exist");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LogoutResponse Logout(string token, string context)
        {
            LogoutResponse response = new LogoutResponse();
            response.SuccessfulLogout = false;
            try
            {
                IMongoQuery removeQ = Query.And(
                                            Query.EQ(MEAPISession.IdProperty, ObjectId.Parse(token)),
                                            Query.EQ(MEAPISession.ProductProperty, context.ToUpper()));

                MEAPISession session = _objectContext.APISessions.Collection.FindOneById(ObjectId.Parse(token));
                if (session != null && session.Product.ToUpper().Equals(context.ToUpper()))
                {
                    _objectContext.APISessions.Collection.Remove(removeQ);
                    response.SuccessfulLogout = true;
                }
            }
            catch (Exception) { throw; }
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

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
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
