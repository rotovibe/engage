CREATE PROC [dbo].[spPhy_GetAllStatusTypes] 
AS 
	SET NOCOUNT ON 	

	SELECT 
		[StatusTypeId]
		, [Description]
		, [Name] 
	FROM   
		[dbo].[StatusType] with(nolock)
