using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember
{
    public abstract class CareMemberRepositoryFactory<T>
    {
        public static ICareMemberRepository<T> GetCareMemberRepository(string dbName, string productName, string userId)
        {
            try
            {
                ICareMemberRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoCareMemberRepository<T>(dbName) as ICareMemberRepository<T>;
                repo.UserId = userId;
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
