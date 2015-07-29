using NUnit.Framework;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Phytel.Services.AppSettings.Test.Specs
{
    [Binding]
    public class AppSettingsProviderSteps
    {
        protected IDictionary<string, string> _appSettings;
        protected AppSettingsProvider _appSettingsProvider;
        protected string _resultActual;
        protected bool _resultAsBoolActual;
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
            if (_appSettings.ContainsKey(key))
            {
                _appSettings.Remove(key);
            }
        }

        [Then(@"the bool result should be false")]
        public void ThenTheBoolResultShouldBeFalse()
        {
            Assert.IsFalse(_resultAsBoolActual);
        }

        [Then(@"the bool result should be true")]
        public void ThenTheBoolResultShouldBeTrue()
        {
            Assert.IsTrue(_resultAsBoolActual);
        }

        [Then(@"the result should be ""(.*)""")]
        public void ThenTheResultShouldBe(string resultExpected)
        {
            Assert.AreEqual(resultExpected, _resultActual);
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int resultAsIntExpected)
        {
            Assert.AreEqual(resultAsIntExpected, _resultAsIntActual);
        }

        [When(@"I get the setting ""(.*)""")]
        public void WhenIGetTheSetting(string key)
        {
            _appSettingsProvider = new EagerLoadedAppSettingsProvider(_appSettings);

            _resultActual = _appSettingsProvider.Get(key);
        }

        [When(@"I get the setting ""(.*)"" as bool")]
        public void WhenIGetTheSettingAsBool(string key)
        {
            _appSettingsProvider = new EagerLoadedAppSettingsProvider(_appSettings);

            _resultAsBoolActual = _appSettingsProvider.GetAsBoolean(key);
        }

        [When(@"I get the setting ""(.*)"" as int")]
        public void WhenIGetTheSettingAsInt(string key)
        {
            _appSettingsProvider = new EagerLoadedAppSettingsProvider(_appSettings);

            _resultAsIntActual = _appSettingsProvider.GetAsInt(key);
        }

        [When(@"I provide default value ""(.*)"" and get the setting ""(.*)""")]
        public void WhenIProvideDefaultValueAndGetTheSetting(string defaultValue, string key)
        {
            _appSettingsProvider = new EagerLoadedAppSettingsProvider(_appSettings);

            _resultActual = _appSettingsProvider.Get(key, defaultValue);
        }

        [When(@"I provide default value ""(.*)"" and get the setting ""(.*)"" as bool")]
        public void WhenIProvideDefaultValueAndGetTheSettingAsBool(string defaultValueAsString, string key)
        {
            _appSettingsProvider = new EagerLoadedAppSettingsProvider(_appSettings);

            bool defaultValue = bool.Parse(defaultValueAsString);

            _resultAsBoolActual = _appSettingsProvider.GetAsBoolean(key, defaultValue);
        }

        [When(@"I provide default value (.*) and get the setting ""(.*)"" as int")]
        public void WhenIProvideDefaultValueAndGetTheSettingAsInt(int defaultValue, string key)
        {
            _appSettingsProvider = new EagerLoadedAppSettingsProvider(_appSettings);

            _resultAsIntActual = _appSettingsProvider.GetAsInt(key, defaultValue);
        }
    }
}