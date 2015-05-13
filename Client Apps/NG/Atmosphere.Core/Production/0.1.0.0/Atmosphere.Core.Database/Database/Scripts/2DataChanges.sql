USE [C3]

INSERT INTO [UserContractProperty] ([UserId],[ContractId],[Key],[Value])
SELECT UserId, 6, 'ENABLE_SEARCH_ALL_GROUPS', '1' FROM [dbo].[User]
