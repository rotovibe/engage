using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Common;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Phytel.Services;
using System.Security.Cryptography;
using System.Text;

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

        public UserAuthenticateResponse LoginUser(string userName, string password, string securityToken, string apiKey, string productName, string contractNumber)
        {
            try
            {
                UserAuthenticateResponse response = new UserAuthenticateResponse();
                MEAPISession session = null;

                //need to do a lookup against the APIKey collection to see if apiKey/Product combination exists
                MEAPIUser user = (from k in _objectContext.APIUsers where k.UserName == userName && k.ApiKey == apiKey && k.Product == productName.ToUpper() && k.IsActive == true select k).FirstOrDefault();
                if (user != null)
                {
                    //validate password
                    string dbPwd = HashText(password, user.Salt, new SHA1CryptoServiceProvider());
                    if (dbPwd.Equals(user.Password))
                    {
                        session = new MEAPISession
                        {
                            SecurityToken = securityToken,
                            APIKey = apiKey,
                            Product = productName,
                            SessionLengthInMinutes = user.SessionLengthInMinutes,
                            SessionTimeOut = DateTime.UtcNow.AddMinutes(user.SessionLengthInMinutes),
                            UserName = user.UserName,
                            Version = 1.0,
                            UserId = user.Id,
                            ContractNumber = (string.IsNullOrEmpty(contractNumber) ? user.DefaultContract : contractNumber)
                        };

                        _objectContext.APISessions.Collection.Insert(session);
                    }
                    else
                        throw new UnauthorizedAccessException("Login Failed!  Password is incorrect");

                    List<ContractInfo> cts = new List<ContractInfo>();
                    cts.Add(new ContractInfo { Number = session.ContractNumber });

                    response = new UserAuthenticateResponse
                                    {
                                        APIToken = session.Id.ToString(),
                                        Contracts = cts,
                                        Name = user.UserName,
                                        SessionTimeout = user.SessionLengthInMinutes,
                                        UserName = user.UserName
                                    };
                }
                else
                    throw new UnauthorizedAccessException("Login Failed! Incorrect login details like username, apikey or product.");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
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

                if (request != null)
                {
                    if (ObjectId.TryParse(request.Token, out tokenObjectId))
                    {
                        MEAPISession session = _objectContext.APISessions.Collection.FindOneByIdAs<MEAPISession>(tokenObjectId);

                        if (session != null)
                        {
                            if (session.SecurityToken.ToUpper().Equals(securityToken.ToUpper()) &&
                                session.ContractNumber.ToUpper().Equals(request.ContractNumber.ToUpper()) &&
                                session.Product.ToUpper().Equals(request.Context.ToUpper()))
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
                                throw new UnauthorizedAccessException("SD:APISessionRepository:Validate():Invalid Security Authorization Request");

                            return response;
                        }
                        else
                            throw new UnauthorizedAccessException("SD:APISessionRepository:Validate():Security Token does not exist");
                    }
                    else
                        throw new UnauthorizedAccessException("SD:APISessionRepository:Validate():Security Token is not in correct format.");
                }
                else
                    throw new UnauthorizedAccessException("SD:APISessionRepository:Validate():Request is invalid");
            }
            catch (Exception)
            {
                throw;
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
            IRestClient client = new JsonServiceClient();

            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/1.0/{2}/Contact/User/{3}",
                                                    DDContactServiceUrl,
                                                    productContext,
                                                    contractNumber,
                                                    sqlUserID), returnId.ToString());

            contactDataResponse = client.Get<GetContactByUserIdDataResponse>(url);

            if (contactDataResponse != null && contactDataResponse.Contact != null)
                returnId = ObjectId.Parse(contactDataResponse.Contact.Id);
            
            return returnId;
        }

        private string HashText(string text, string salt, System.Security.Cryptography.HashAlgorithm hash)
        {
            byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(text, salt));
            byte[] hashedBytes = hash.ComputeHash(textWithSaltBytes);
            hash.Clear();
            return Convert.ToBase64String(hashedBytes);
        }
        #endregion

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
