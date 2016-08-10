/****** Object:  StoredProcedure [dbo].[spPhy_GetAuditTypes]    Script Date: 02/07/2013 08:39:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_GetAuditTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_GetAuditTypes]
GO

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
GO


