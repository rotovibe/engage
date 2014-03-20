﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Phytel.API.AppDomain.Security
{
    public class APISessionRepository<T> : ISecurityRepository<T>
    {
        #region Endpoint addresses

        static readonly string PhytelUserIDHeaderKey = "x-Phytel-UserID";
        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        #endregion

        protected SecurityMongoContext _objectContext;

        public APISessionRepository(SecurityMongoContext context)
        {
            _objectContext = context;
        }

        public AuthenticateResponse LoginUser(string token, string securityToken)
        {
            throw new NotImplementedException();
        }

        public UserAuthenticateResponse LoginUser(string userName, string password, string securityToken, string apiKey, string productName)
        {
            throw new NotImplementedException();

            //try
            //{
            //    UserAuthenticateResponse response = new UserAuthenticateResponse();
            //    MEAPISession session = null;

            //    //need to do a lookup against the APIKey collection to see if apiKey/Product combination exists
            //    MEAPIUser user = (from k in _objectContext.APIUsers where k.UserName == userName && k.ApiKey == apiKey && k.Product == productName && k.IsActive == true select k).FirstOrDefault();
            //    if (user != null)
            //    {
            //        //validate password
            //        Phytel.Services.DataProtector protector = new Services.DataProtector(Services.DataProtector.Store.USE_SIMPLE_STORE);
            //        string dbPwd = protector.Decrypt(user.Password);
            //        if (dbPwd.Equals(password))
            //        {
            //            session = new MEAPISession
            //            {
            //                SecurityToken = securityToken,
            //                APIKey = apiKey,
            //                Product = productName,
            //                SessionLengthInMinutes = user.SessionLengthInMinutes,
            //                SessionTimeOut = DateTime.UtcNow.AddMinutes(user.SessionLengthInMinutes),
            //                UserName = user.UserName,
            //                SQLUserId = user.Id.ToString()
            //            };

            //            _objectContext.APISessions.Collection.Insert(session);
            //        }
            //        else
            //            throw new UnauthorizedAccessException("Login Failed!  Username and/or Password is incorrect");

            //        response = new UserAuthenticateResponse 
            //                        {
            //                            APIToken = session.Id.ToString(), 
            //                            Contracts = new List<ContractInfo>(), 
            //                            Name = user.UserName, 
            //                            SessionTimeout = user.SessionLengthInMinutes, 
            //                            UserName = user.UserName 
            //                        };
            //    }
            //    else
            //        throw new UnauthorizedAccessException("Login Failed! Unknown Username/Password");

            //    return response;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string securityToken, string apiKey, string productName)
        {
            try
            {
                AuthenticateResponse response = new AuthenticateResponse();

                //need to do a lookup against the APIKey collection to see if apiKey/Product combination exists
                MEAPIKey key = (from k in _objectContext.APIKeys where k.ApiKey == apiKey && k.Product == productName && k.IsActive == true select k).FirstOrDefault();
                if (key != null)
                {
                    string contractNumber = existingReponse.Contracts[0].Number;
                    ObjectId UserId = GetUserId(contractNumber, productName, existingReponse.SQLUserID);
                    if (UserId != ObjectId.Empty)
                    {
                        MEAPISession session = new MEAPISession
                        {
                            SecurityToken = securityToken,
                            APIKey = apiKey,
                            Product = productName,
                            SessionLengthInMinutes = existingReponse.SessionTimeout,
                            SessionTimeOut = DateTime.UtcNow.AddMinutes(existingReponse.SessionTimeout),
                            UserName = existingReponse.UserName,
                            UserId = UserId,
                            ContractNumber = contractNumber,
                            SQLUserId = existingReponse.SQLUserID,
                            Version = 1.0
                        };

                        _objectContext.APISessions.Collection.Insert(session);

                        response = existingReponse;
                        response.UserId = UserId.ToString();
                        response.APIToken = session.Id.ToString();
                    }
                    else
                        throw new UnauthorizedAccessException("Login Failed! User does not have a valid contact card");
                }
                else
                    throw new UnauthorizedAccessException("Login Failed! Unknown Username/Password");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ValidateTokenResponse Validate(ValidateTokenRequest request, string securityToken)
        {
            try
            {
                ValidateTokenResponse response = null;
                ObjectId tokenObjectId;

                if (ObjectId.TryParse(request.Token, out tokenObjectId))
                {
                    MEAPISession session = _objectContext.APISessions.Collection.FindOneByIdAs<MEAPISession>(tokenObjectId);

                    if (session != null)
                    {
                        if (session.SecurityToken.Equals(securityToken) &&
                            session.ContractNumber.Equals(request.ContractNumber) &&
                            session.Product.Equals(request.Context))
                        {
                            session.SessionTimeOut = DateTime.UtcNow.AddMinutes(session.SessionLengthInMinutes);
                            response = new ValidateTokenResponse
                            {
                                SessionLengthInMinutes = session.SessionLengthInMinutes,
                                SessionTimeOut = session.SessionTimeOut,
                                TokenId = session.Id.ToString(),
                                SQLUserId = session.SQLUserId,
                                UserId = session.UserId.ToString(),
                                UserName = session.UserName
                            };
                            _objectContext.APISessions.Collection.Save(session);
                        }
                        else
                            throw new UnauthorizedAccessException("Invalid Security Authorization Request");

                        return response;
                    }
                    else
                        throw new UnauthorizedAccessException("Security Token does not exist (can't find security token)");
                }
                else
                    throw new UnauthorizedAccessException("Security Token is not in correct format.");
            }
            catch (Exception ex)
            {
                throw new Exception("SD:APISessionRepository:Validate()::" + ex.Message, ex.InnerException);
            }
        }

        public LogoutResponse Logout(string token, string securityToken, string context, string contractNumber)
        {
            LogoutResponse response = new LogoutResponse();
            response.SuccessfulLogout = false;
            ObjectId tokenObjectId;
            try
            {
                if (ObjectId.TryParse(token, out tokenObjectId))
                {
                    IMongoQuery removeQ = Query.EQ(MEAPISession.IdProperty, ObjectId.Parse(token));

                    WriteConcernResult wcr = _objectContext.APISessions.Collection.Remove(removeQ);
                    response.SuccessfulLogout = (wcr.Ok);
                }
            }
            catch (Exception) { throw; }
            return response;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
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

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }

        #region Private Methods
        private ObjectId GetUserId(string contractNumber, string productContext, string sqlUserID)
        {
            ObjectId returnId = ObjectId.Empty;

            GetContactByUserIdDataResponse contactDataResponse = null;
            IRestClient client = GetJsonServiceClient(returnId.ToString());

            contactDataResponse = client.Get<GetContactByUserIdDataResponse>(string.Format("{0}/{1}/1.0/{2}/Contact/User/{3}",
                                                    DDContactServiceUrl,
                                                    productContext,
                                                    contractNumber,
                                                    sqlUserID));

            if (contactDataResponse != null && contactDataResponse.Contact != null)
                returnId = ObjectId.Parse(contactDataResponse.Contact.ContactId);
            
            return returnId;
        }

        public static IRestClient GetJsonServiceClient(string userId)
        {
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", PhytelUserIDHeaderKey, userId));

            return client;
        }
        #endregion
    }
}
