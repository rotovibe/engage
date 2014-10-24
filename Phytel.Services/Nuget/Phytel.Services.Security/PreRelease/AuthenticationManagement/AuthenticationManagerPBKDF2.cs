using System;
using System.Linq;
using System.Security.Cryptography;

namespace Phytel.Services.Security
{
    public class AuthenticationManagerPBKDF2 : IAuthenticationManager
    {
        public const int PBKDF2_ITERATIONS = 1000;
        public const int PBKDF2_SALT_SIZE = 20;

        /// <summary>
        /// Validates a passphrase given a hash and salt of the correct one.
        /// </summary>
        /// <param name="key">The correct passphrase hash.</param>
        /// <param name="salt">The correct salt for the passphrase.</param>
        /// <param name="plainTextPassphraseAttempt">The passphrase to validate.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt)
        {
            return PassphraseIsValid(Convert.FromBase64String(key), Convert.FromBase64String(salt), plainTextPassphraseAttempt);
        }

        /// <summary>
        /// Validates a passphrase given a hash and salt (in bytes) of the correct one.
        /// </summary>
        /// <param name="key">The correct passphrase hash (in bytes).</param>
        /// <param name="salt">The correct salt for the passphrase (in bytes).</param>
        /// <param name="plainTextPassphraseAttempt">The passphrase to validate.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public bool PassphraseIsValid(byte[] key, byte[] salt, string plainTextPassphraseAttempt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(plainTextPassphraseAttempt, salt, PBKDF2_ITERATIONS))
            {
                byte[] newKey = deriveBytes.GetBytes(20);  // derive a 20-byte key

                return SlowEquals(newKey, key);
            }
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint difference = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                // The result of XORing two integers will be zero if and only if they are exactly the same
                difference |= (uint)(a[i] ^ b[i]); 
            }

            return difference == 0;
        }

        /// <summary>
        /// Creates an AuthenticationData object with the salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="plainTextPassphrase">The password to hash.</param>
        /// <returns>An AuthenticationData object with the Base64 Encoded Key and Salt</returns>
        public AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase)
        {
            // specify that we want to randomly generate a 20-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(plainTextPassphrase, PBKDF2_SALT_SIZE))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(20);  // derive a 20-byte key

                return new AuthenticationData(key, salt, plainTextPassphrase);
            }
        }
    }
}
