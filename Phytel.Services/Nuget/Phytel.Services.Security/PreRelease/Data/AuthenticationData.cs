using System;

namespace Phytel.Services.Security
{
    public class AuthenticationData
    {
        //public AuthenticationData(byte[] key, byte[] salt)
        //    : this(key, salt, string.Empty)
        //{ }

        //public AuthenticationData(string key, string salt)
        //    : this(key, salt, string.Empty)
        //{ }

        public AuthenticationData(byte[] key, byte[] salt, string passphrase)
            : this(Convert.ToBase64String(key), Convert.ToBase64String(salt), passphrase)
        { }

        public AuthenticationData(string key, string salt, string passphrase)
        {
            EncodedKey = key;
            EncodedSalt = salt;
            Passphrase = passphrase;
        }

        public string EncodedKey { get; private set; }

        public string EncodedSalt { get; private set; }

        public string Passphrase { get; set; }
    }
}
