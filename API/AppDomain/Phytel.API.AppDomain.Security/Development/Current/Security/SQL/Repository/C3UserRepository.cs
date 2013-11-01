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

        public DTO.AuthenticateResponse LoginUser(string token)
        {
            Services.ParameterCollection parmColl = new Services.ParameterCollection();
            parmColl.Add(new Services.Parameter("@Token", token, System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, 100));
            SqlDataReader reader = Phytel.Services.SQLDataService.Instance.GetReader("C3", false, "spPhy_ProcessUserToken", parmColl);

            AuthenticateResponse response = new AuthenticateResponse();

            int colUserId = reader.GetOrdinal("UserID");
            int colUserName = reader.GetOrdinal("UserName");
            int colFirstName = reader.GetOrdinal("FirstName");
            int colLastName = reader.GetOrdinal("LastName");
            int colSessionTimeout = reader.GetOrdinal("SessionTimeout");

            while (reader.Read())
            {
                response.UserID = reader.GetGuid(colUserId);
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
                int colContractName = reader.GetOrdinal("Name");

                while (reader.Read())
                {
                    response.Contracts.Add(new ContractInfo { Id = reader.GetInt32(colContractId), Name = reader.GetString(colContractName) });
                }
            }

            if (reader != null)
                reader.Close();

            return response;
        }

        public DTO.AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string apiKey, string productName)
        {
            throw new NotImplementedException();
        }

        public DTO.ValidateTokenResponse Validate(string token)
        {
            throw new NotImplementedException();
        }
    }
}
