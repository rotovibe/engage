namespace Phytel.API.AppDomain.Security
{
    public abstract class SecurityRepositoryFactory<T>
    {
        public static ISecurityRepository<T> GetUserRepository(string productName)
        {
            ISecurityRepository<T> repo = null;

            repo = new C3UserRepository<T>() as ISecurityRepository<T>;

            return repo;
        }

        public static ISecurityRepository<T> GetSecurityRepository(string productName)
        {
            ISecurityRepository<T> repo = null;

            SecurityMongoContext context = new SecurityMongoContext();
            repo = new APISessionRepository<T>(context) as ISecurityRepository<T>;

            return repo;
        }

    }
}
