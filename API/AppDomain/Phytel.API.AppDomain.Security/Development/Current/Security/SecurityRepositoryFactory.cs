
namespace Phytel.API.DataDomain.Security
{
    public abstract class SecurityRepositoryFactory<T>
    {
        public static ISecurityRepository<T> GetAPISessionRepository(string productName)
        {
            ISecurityRepository<T> repo = null;

            if (productName.ToLower().Equals("NG"))
            {
                SecurityMongoContext context = new SecurityMongoContext();
                repo = new APISessionMongoRepository<T>(context) as ISecurityRepository<T>;
            }

            return repo;
        }

        public static ISecurityRepository<T> GetAPIUserRepository(string productName)
        {
            ISecurityRepository<T> repo = null;

            if (productName.ToLower().Equals("NG"))
            {
                SecurityMongoContext context = new SecurityMongoContext();
                repo = new APIUsersMongoRepository<T>(context) as ISecurityRepository<T>;
            }

            return repo;
        }

    }
}
