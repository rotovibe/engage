CREATE PROC [dbo].[spPhy_GetUserRolesByContract]
@UserId uniqueidentifier,
@ContractId INT
AS

Select * from fnPhy_GetUserRolesByContract(@UserId, @ContractId)
