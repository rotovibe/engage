using MongoDB.Bson;
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
            //                SessionTimeOut = DateTime.Now.AddMinutes(user.SessionLengthInMinutes),
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
                            SessionTimeOut = DateTime.Now.AddMinutes(existingReponse.SessionTimeout),
                            UserName = existingReponse.UserName,
                            UserId = UserId,
                            ContractNumber = contractNumber,
                            SQLUserId = existingReponse.SQLUserID,
                            Version = 1.0
                        };

                        _objectContext.APISessions.Collection.Insert(session);

                        response = existingReponse;
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
                //IdProperty, SecurityTokenProperty, ContractNumberProperty, ProductProperty
                if (ObjectId.TryParse(request.Token, out tokenObjectId))
                {
                    int sessionLengthInMinutes = (from s in _objectContext.APISessions
                                                  where s.Id == tokenObjectId
                                                    && s.SecurityToken == securityToken
                                                    && s.ContractNumber == request.ContractNumber
                                                    && s.Product == request.Context.ToUpper()
                                                  select s.SessionLengthInMinutes).FirstOrDefault();

                    if (sessionLengthInMinutes > 0)
                    {
                        FindAndModifyResult result = _objectContext.APISessions.Collection.FindAndModify(Query.EQ(MEAPISession.IdProperty, tokenObjectId), SortBy.Null,
                                                    MongoDB.Driver.Builders.Update.Set(MEAPISession.SessionTimeOutProperty, DateTime.Now.AddMinutes(sessionLengthInMinutes)), true);

                        if (result != null && result.ModifiedDocument != null)
                        {
                            MEAPISession session = BsonSerializer.Deserialize<MEAPISession>(result.ModifiedDocument);
                            response = new ValidateTokenResponse
                                            {
                                                SessionLengthInMinutes = session.SessionLengthInMinutes,
                                                SessionTimeOut = session.SessionTimeOut,
                                                TokenId = session.Id.ToString(),
                                                SQLUserId = session.SQLUserId,
                                                UserId = session.UserId.ToString(),
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
                else
                    throw new UnauthorizedAccessException("Security Token is not in correct format.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LogoutResponse Logout(string token, string securityToken, string context, string contractNumber)
        {
            LogoutResponse response = new LogoutResponse();
            response.SuccessfulLogout = false;
            try
            {
                IMongoQuery removeQ = Query.And(
                                            Query.EQ(MEAPISession.IdProperty, ObjectId.Parse(token)),
                                            Query.EQ(MEAPISession.SecurityTokenProperty, securityToken),
                                            Query.EQ(MEAPISession.ContractNumberProperty, contractNumber),
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

        #region Private Methods
        private ObjectId GetUserId(string contractNumber, string productContext, string sqlUserID)
        {
            ObjectId returnId = ObjectId.Empty;

            GetContactByUserIdDataResponse contactDataResponse = null;
            IRestClient client = new JsonServiceClient();
            
            //[Route("/{Context}/{Version}/{ContractNumber}/Contact/User/{UserId}", "GET")]

            contactDataResponse = client.Get<GetContactByUserIdDataResponse>(string.Format("{0}/{1}/1.0/{2}/Contact/User/{3}",
                                                    DDContactServiceUrl,
                                                    productContext,
                                                    contractNumber,
                                                    sqlUserID));

            if (contactDataResponse != null && contactDataResponse.Contact != null)
                returnId = ObjectId.Parse(contactDataResponse.Contact.ContactId);
            
            return returnId;
        }
        #endregion
    }
}
