using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.Services;

namespace C3.Framework
{
    public class PhytelEncrypter
    {
        private DataProtector DataProtector;

        public PhytelEncrypter()
        {
            DataProtector = new DataProtector(Store);
        }

        public DataProtector.Store Store
        {
            get { return DataProtector.Store.USE_SIMPLE_STORE; }
        }

        public string Encrypt(string toEncrypt)
        {
            return DataProtector.Encrypt(toEncrypt);
        }

        public string Decrypt(string toDecrypt)
        {
            return DataProtector.Decrypt(toDecrypt);
        }
    }
}
