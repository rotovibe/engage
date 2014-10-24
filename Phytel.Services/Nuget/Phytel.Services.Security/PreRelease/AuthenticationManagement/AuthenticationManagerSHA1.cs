using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Phytel.Services.Security
{
    public class AuthenticationManagerSHA1 : IAuthenticationManager
    {
        /// <summary>
        /// Validates a passphrase given a hash and salt of the correct one.
        /// </summary>
        /// <param name="key">The correct passphrase hash.</param>
        /// <param name="salt">The correct salt for the passphrase.</param>
        /// <param name="plainTextPassphraseAttempt">The passphrase to validate.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt)
        {
            byte[] passphraseAttemptKey = HashText(plainTextPassphraseAttempt, salt, new SHA1CryptoServiceProvider());

            return (SlowEquals(passphraseAttemptKey, Convert.FromBase64String(key)));
        }

        /// <summary>
        /// Creates an AuthenticationData object with the salted SHA1 hash of the password.
        /// </summary>
        /// <param name="plainTextPassphrase">The password to hash.</param>
        /// <returns>An AuthenticationData object with the Base64 Encoded Key and Salt</returns>
        public AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase)
        {
            string randomSaltKey = new Random().Next(100, 999999999).ToString();
            DataProtector protector = new DataProtector(DataProtector.Store.USE_SIMPLE_STORE);
            string salt = EncodeTo64(protector.Encrypt(randomSaltKey));
            byte[] key = HashText(plainTextPassphrase, salt, new SHA1CryptoServiceProvider());

            return new AuthenticationData(Convert.ToBase64String(key), salt, plainTextPassphrase);
        }

        private static byte[] HashText(string passphrase, string salt, HashAlgorithm hash)
        {
            string base64PassphraseSalt = EncodeTo64(string.Concat(passphrase, salt));
            byte[] textWithSaltBytes = Convert.FromBase64String(base64PassphraseSalt);
            byte[] hashedBytes = hash.ComputeHash(textWithSaltBytes);
            hash.Clear();

            return hashedBytes;
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

        private static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
