using Phytel.API.AppDomain.Security.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.AppDomain.Security
{
    public class C3UserRepository<T> : ISecurityRepository<T>
    {
        public C3UserRepository()
        {
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

        public DTO.AuthenticateResponse LoginUser(string token, string securityToken)
        {
            try
            {
                string dbConnName = System.Configuration.ConfigurationManager.AppSettings["PhytelServicesConnName"];
                Services.ParameterCollection parmColl = new Services.ParameterCollection();
                parmColl.Add(new Services.Parameter("@Token", token, System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, 100));
                SqlDataReader reader = Phytel.Services.SQLDataService.Instance.GetReader(dbConnName, false, "spPhy_ProcessUserToken", parmColl);

                AuthenticateResponse response = new AuthenticateResponse();

                int colUserId = reader.GetOrdinal("UserID");
                int colUserName = reader.GetOrdinal("UserName");
                int colFirstName = reader.GetOrdinal("FirstName");
                int colLastName = reader.GetOrdinal("LastName");
                int colSessionTimeout = reader.GetOrdinal("SessionTimeout");

                while (reader.Read())
                {
                    response.SQLUserID = reader.GetGuid(colUserId).ToString();
                    response.UserName = reader.GetString(colUserName);
                    response.FirstName = reader.GetString(colFirstName);
                    response.LastName = reader.GetString(colLastName);
                    response.SessionTimeout = reader.GetInt16(colSessionTimeout);
                }

                if (response != null)
                {
                    response.Contracts = new List<ContractInfo>();

                    reader.NextResult();

                    int colContractId = reader.GetOrdinal("ContractId");
                    int colContractNumber = reader.GetOrdinal("Number");
                    int colContractName = reader.GetOrdinal("Name");

                    while (reader.Read())
                    {
                        response.Contracts.Add(new ContractInfo { Id = reader.GetInt32(colContractId), Name = reader.GetString(colContractName), Number = reader.GetString(colContractNumber) });
                    }
                }

                if (reader != null)
                    reader.Close();

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DTO.AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string securityToken, string apiKey, string productName)
        {
            throw new NotImplementedException();
        }

        public DTO.UserAuthenticateResponse LoginUser(string userName, string password, string securityToken, string apiKey, string productName, string contractNumber)
        {
            throw new NotImplementedException();
        }

        public DTO.ValidateTokenResponse Validate(ValidateTokenRequest request, string securityToken)
        {
            throw new NotImplementedException();
        }

        public DTO.LogoutResponse Logout(string token, string securityToken, string productName, string contractNumber)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
