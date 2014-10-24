using System;

namespace Phytel.Services.Security
{
    public class AuthenticationData
    {
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
