﻿using NUnit.Framework;
using System;

namespace Phytel.Services.Security.Text
{
    [TestFixture]
    class AuthenticationManagerSHA1Tests
    {
        protected IAuthenticationManager _authenticationManager;

        [TestFixtureSetUp]
        public void Initialize()
        {
            _authenticationManager = new AuthenticationManagerSHA1();
        }

        [TestCase]
        public void GenerateAuthenticationDataFromPassphrase_ReturnData_NotNull()
        {
            string passphrase = "TestPassphrase";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.EncodedKey);
            Assert.IsNotNull(data.EncodedSalt);
            Assert.AreEqual(passphrase, data.Passphrase);
        }

        [TestCase]
        public void GenerateAuthenticationDataFromPassphrase_ReturnData_PassphraseAreEqual()
        {
            string passphrase = "TestPassphrase";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);
            Assert.IsNotNull(data);
            Assert.AreEqual(passphrase, data.Passphrase);
        }

        [TestCase]
        public void PassphraseIsValid_AttemptValidation_Success()
        {
            string passphrase = "TestPassphrase";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);

            Assert.IsTrue(_authenticationManager.PassphraseIsValid(data.EncodedKey, data.EncodedSalt, passphrase));
        }

        [TestCase]
        public void PassphraseIsValid_AttemptValidationChangePassphraseLength_Success()
        {
            string passphrase = "Test Passphrase for joe";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);

            Assert.IsTrue(_authenticationManager.PassphraseIsValid(data.EncodedKey, data.EncodedSalt, passphrase));
        }

        [TestCase]
        public void PassphraseIsValid_AttemptValidationNonAlphaNumeric_Success()
        {
            string passphrase = "Test Passphrase !@#$%^&*()_+1234567890";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);

            Assert.IsTrue(_authenticationManager.PassphraseIsValid(data.EncodedKey, data.EncodedSalt, passphrase));
        }

        [TestCase]
        public void PassphraseIsValid_AttemptValidation_Failure()
        {
            string passphrase = "TestPassphrase";
            string incorrectPassphrase = "FAIL";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);

            Assert.IsFalse(_authenticationManager.PassphraseIsValid(data.EncodedKey, data.EncodedSalt, incorrectPassphrase));
        }

        [TestCase]
        public void PassphraseIsValid_AttemptValidationChangePassphraseLength_Failure()
        {
            string passphrase = "Test Passphrase for joe";
            string incorrectPassphrase = "This passphrase is incorrect";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);

            Assert.IsFalse(_authenticationManager.PassphraseIsValid(data.EncodedKey, data.EncodedSalt, incorrectPassphrase));
        }

        [TestCase]
        public void PassphraseIsValid_AttemptValidationNonAlphaNumeric_Failure()
        {
            string passphrase = "Test Passphrase !@#$%^&*()_+1234567890";
            string incorrectPassphrase = "This passsphrase is incorrect and !@)(*^524823459";
            AuthenticationData data = _authenticationManager.GenerateAuthenticationDataForPassphrase(passphrase);

            Assert.IsFalse(_authenticationManager.PassphraseIsValid(data.EncodedKey, data.EncodedSalt, incorrectPassphrase));
        }
    }
}
