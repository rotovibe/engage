using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Phytel.Services.Security
{
    public class AuthenticationManagerSHA1 : IAuthenticationManager
    {
        public bool PassphraseIsValid(string key, string salt, string plainTextPassphraseAttempt)
        {
            return PassphraseIsValid(Convert.FromBase64String(key), Convert.FromBase64String(salt), plainTextPassphraseAttempt);
        }

        public bool PassphraseIsValid(byte[] key, byte[] salt, string plainTextPassphraseAttempt)
        {
            byte[] passphraseAttemptKey = HashText(plainTextPassphraseAttempt, Convert.ToBase64String(salt), new SHA1CryptoServiceProvider());
            
            return (passphraseAttemptKey.SequenceEqual(key));
        }

        public AuthenticationData GenerateAuthenticationDataForPassphrase(string plainTextPassphrase)
        {
            string randomSaltKey = new Random().Next(100, 999999999).ToString();
            DataProtector protector = new DataProtector(DataProtector.Store.USE_SIMPLE_STORE);
            string salt = EncodeTo64(protector.Encrypt(randomSaltKey));
            byte[] key = HashText(plainTextPassphrase, salt, new SHA1CryptoServiceProvider());

            return new AuthenticationData(Convert.ToBase64String(key), salt, plainTextPassphrase);
        }

        private byte[] HashText(string passphrase, string salt, HashAlgorithm hash)
        {
            string base64PassphraseSalt = EncodeTo64(string.Concat(passphrase, salt));
            byte[] textWithSaltBytes = Convert.FromBase64String(base64PassphraseSalt);
            byte[] hashedBytes = hash.ComputeHash(textWithSaltBytes);
            hash.Clear();

            return hashedBytes;
        }

        private static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
