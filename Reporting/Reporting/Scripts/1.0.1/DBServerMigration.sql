/**
**
	1) CHANGE REGISTRATION FOR REPORTING SERVER
	2) TEST CONNECTION WITH SERVICES MANAGER
	3) MODIFY DATASOURCES IN JASPERSERVER
**
**/

SELECT * FROM [USERCONFIG].[dbo].[ContractDatabase]
SELECT * FROM [USERCONFIG].[dbo].[Servers]
  
/** old db server **/
UPDATE [USERCONFIG].[dbo].[Servers]
SET IpAddress='10.90.1.10', IpName='10.90.1.10', Description = 'Model Reporting SQL'
WHERE ServerID= 6;

/** new db server **/
UPDATE [USERCONFIG].[dbo].[Servers]
SET IpAddress='10.90.1.191', IpName='10.90.1.191', Description = 'Model Reporting SQL'
WHERE ServerID= 6;

192.168.2.73