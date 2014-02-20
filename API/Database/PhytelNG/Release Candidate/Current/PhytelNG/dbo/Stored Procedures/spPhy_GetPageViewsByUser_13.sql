CREATE PROCEDURE [dbo].[spPhy_GetPageViewsByUser_13]
	@UserId uniqueidentifier
	, @ContractId int
AS
BEGIN

SET NOCOUNT ON

SELECT pv.ViewId
	, pv.ViewName
	, pv.[Description]
	, pv.UserId
	, pv.ControlId
	, pv.ContractId
	, pv.ViewContainer
	, pv.IsPageDefaultView
	, pv.IsUserDefaultView
	, pv.IsDeleted
	, pv.CreateDate
	, pv.UpdateDate
	, c.[Path_13] AS PagePath
	, c.[Name] AS PageName
FROM PageView pv
JOIN [Control] c ON c.ControlId = pv.ControlId
WHERE (pv.UserId = @UserId OR pv.UserId IS NULL)
AND pv.ContractId = @ContractId
AND pv.IsDeleted = 0
END
