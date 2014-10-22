
namespace Phytel.Services.Security
{
    public interface IAuthenticationManager
    {
        bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt);
        bool PassphraseIsValid(byte[] key, byte[] salt, string plainTextPassphraseAttempt);
        AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase);
    }
}