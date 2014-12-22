using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Phytel.Services.AppSettings.Test.Specs
{
    [Binding]
    public class AppSettingsProviderSteps
    {
        protected AppSettingsProvider _appSettingsProvider;
        protected IDictionary<string, string> _appSettings;
        protected string _resultActual;
        protected int _resultAsIntActual;

        [Before]
        public void Before()
        {
            _appSettings = new Dictionary<string, string>();

        }

        [Given(@"setting ""(.*)"" is defined as ""(.*)""")]
        public void GivenSettingIsDefinedAs(string key, string value)
        {
            _appSettings.Add(key, value);
        }

        [Given(@"setting ""(.*)"" is not defined")]
        public void GivenSettingIsNotDefined(string key)
        {
            if(_appSettings.ContainsKey(key))
            {
                _appSettings.Remove(key);
            }
        }

        [Then(@"the result should be ""(.*)""")]
        public void ThenTheResultShouldBe(string resultExpected)
        {
            Assert.AreEqual(resultExpected, _resultActual);
        }

        [When(@"I get the setting ""(.*)""")]
        public void WhenIGetTheSetting(string key)
        {
            _appSettingsProvider = new AppSettingsProvider(_appSettings);

            _resultActual = _appSettingsProvider.Get(key);
        }

        [When(@"I provide default value ""(.*)"" and get the setting ""(.*)""")]
        public void WhenIProvideDefaultValueAndGetTheSetting(string defaultValue, string key)
        {
            _appSettingsProvider = new AppSettingsProvider(_appSettings);

            _resultActual = _appSettingsProvider.Get(key, defaultValue);
        }

        [When(@"I get the setting ""(.*)"" as int")]
        public void WhenIGetTheSettingAsInt(string key)
        {
            _appSettingsProvider = new AppSettingsProvider(_appSettings);

            _resultAsIntActual = _appSettingsProvider.GetAsInt(key);
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int resultAsIntExpected)
        {
            Assert.AreEqual(resultAsIntExpected, _resultAsIntActual);
        }
    }
}