Feature: AppSettingsProvider
	As an application
	I would like to generically retrieve my application settings
	So that my code is more testable and independent
	
Scenario: Returns defined setting value when setting defined
	Given setting "timeout" is defined as "100"
	When I get the setting "timeout"
	Then the result should be "100"

Scenario: Returns defined setting value when setting defined and getting as int
	Given setting "timeout" is defined as "100"
	When I get the setting "timeout" as int
	Then the result should be 100

Scenario: Returns empty string when setting not defined
	Given setting "timeout" is not defined
	When I get the setting "timeout"
	Then the result should be ""

Scenario: Returns 0 when setting not defined and gettting as int
	Given setting "timeout" is not defined
	When I get the setting "timeout" as int
	Then the result should be 0

Scenario: Returns 0 when setting not defined as int and gettting as int
	Given setting "timeout" is defined as "foo"
	When I get the setting "timeout" as int
	Then the result should be 0

Scenario: Returns defined setting value when setting defined and default value provided
	Given setting "timeout" is defined as "100"
	When I provide default value "50" and get the setting "timeout"
	Then the result should be "100"

Scenario: Returns provided default value when setting not defined and default value provided
	Given setting "timeout" is not defined
	When I provide default value "50" and get the setting "timeout"
	Then the result should be "50"
