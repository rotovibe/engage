CREATE PROCEDURE [dbo].[spPhy_GetAuditTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [AuditTypeId]
      ,[Name]
  FROM [dbo].[AuditType]

END
