
namespace Phytel.Services.Security
{
    public interface IAuthenticationManager
    {
        bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt);
        AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase);
    }
}