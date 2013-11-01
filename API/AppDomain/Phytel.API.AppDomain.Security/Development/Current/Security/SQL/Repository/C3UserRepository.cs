using Phytel.API.DataDomain.C3User.DTO;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.DataDomain.C3User
{
    public class C3UserRepository<T> : IC3UserRepository<T>
    {
        public C3UserRepository()
        {
        }

        public C3UserDataResponse ProcessUserToken(string userToken)
        {
            Services.ParameterCollection parmColl = new Services.ParameterCollection();
            parmColl.Add(new Services.Parameter("@Token", userToken, System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, 100));
            SqlDataReader reader = Phytel.Services.SQLDataService.Instance.GetReader("C3", false, "spPhy_ProcessUserToken", parmColl);

            C3UserDataResponse response = new C3UserDataResponse();

            int colUserId = reader.GetOrdinal("UserID");
            int colUserName = reader.GetOrdinal("UserName");
            int colDisplayName = reader.GetOrdinal("DisplayName");

            while(reader.Read())
            {
                response = new C3UserDataResponse();
                response.UserID = reader.GetGuid(colUserId);
                response.UserName = reader.GetString(colUserName);
                response.FullName = reader.GetString(colDisplayName);
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
