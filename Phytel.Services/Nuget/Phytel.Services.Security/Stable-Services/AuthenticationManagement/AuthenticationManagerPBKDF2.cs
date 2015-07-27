using System;
using System.Linq;
using System.Security.Cryptography;

namespace Phytel.Services.Security
{
    public class AuthenticationManagerPBKDF2 : IAuthenticationManager
    {
        private const int PBKDF2_ITERATIONS = 1000;
        private const int PBKDF2_SALT_SIZE = 24;
        private const int PBKDF2_KEY_SIZE = 24;
        private const int ITERATION_INDEX = 0;
        private const int KEY_INDEX = 1;

        /// <summary>
        /// Validates a passphrase given a key (hash)  and salt of the correct one.
        /// </summary>
        /// <param name="key">The correct passphrase hash coupled with key stretching iterations.</param>
        /// <param name="salt">The correct salt for the passphrase.</param>
        /// <param name="plainTextPassphraseAttempt">The passphrase to validate.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt)
        {
            char[] delimiter = { ':' };
            string[] iterationKey = key.Split(delimiter);
            int iterations = Int32.Parse(iterationKey[ITERATION_INDEX]);
            byte[] hash = Convert.FromBase64String(iterationKey[KEY_INDEX]);

            using (var deriveBytes = new Rfc2898DeriveBytes(plainTextPassphraseAttempt, Convert.FromBase64String(salt), iterations))
            {
                byte[] newKey = deriveBytes.GetBytes(PBKDF2_KEY_SIZE);

                return SlowEquals(newKey, hash);
            } 
        }

        /// <summary>
        /// Creates an AuthenticationData object with the salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="plainTextPassphrase">The password to hash.</param>
        /// <returns>An AuthenticationData object with the Base64 Encoded Key (prefixed with key stretching iterations) and Salt</returns>
        public AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase)
        {
            // specify that we want to randomly generate a 20-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(plainTextPassphrase, PBKDF2_SALT_SIZE, PBKDF2_ITERATIONS))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(PBKDF2_KEY_SIZE);

                return new AuthenticationData(PBKDF2_ITERATIONS + ":" + Convert.ToBase64String(key), Convert.ToBase64String(salt), plainTextPassphrase);
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
    }
}
