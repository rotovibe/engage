using System;
using System.Linq;
using System.Security.Cryptography;

namespace Phytel.Services.Security
{
    public class AuthenticationManagerPBKDF2 : IAuthenticationManager
    {
        public bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt)
        {
            return PassphraseIsValid(Convert.FromBase64String(key), Convert.FromBase64String(salt), plainTextPassphraseAttempt);
        }

        public bool PassphraseIsValid(byte[] key, byte[] salt, string plainTextPassphraseAttempt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(plainTextPassphraseAttempt, salt))
            {
                byte[] newKey = deriveBytes.GetBytes(20);  // derive a 20-byte key

                return (newKey.SequenceEqual(key));
            }
        }

        public AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase)
        {
            // specify that we want to randomly generate a 20-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(plainTextPassphrase, 20))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(20);  // derive a 20-byte key

                return new AuthenticationData(key, salt, plainTextPassphrase);
            }
        }
    }
}
