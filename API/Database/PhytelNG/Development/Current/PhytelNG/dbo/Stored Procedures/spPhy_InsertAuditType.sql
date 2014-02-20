/******
* Author: Brent Giesler
* Date: 01/17/2014
* Desc: Procedure inserts new audit types if they don't already exist.
* Just updating this for testing...
*******/
CREATE PROCEDURE [dbo].[spPhy_InsertAuditType] 
    @TypeName varchar(50)
AS 
	IF NOT EXISTS(SELECT AuditTypeId FROM AuditType WHERE [Name] = @TypeName)
	
	BEGIN
	
	INSERT INTO [dbo].[AuditType] ([Name])
	SELECT @TypeName
	
    END