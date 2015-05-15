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
SET IpAddress='192.168.2.73', IpName='192.168.2.73', Description = 'Model Reporting SQL'
WHERE ServerID= 6;

/** new db server **/
UPDATE [USERCONFIG].[dbo].[Servers]
SET IpAddress='192.168.2.191', IpName='192.168.2.191', Description = 'Model Reporting SQL'
WHERE ServerID= 6;
