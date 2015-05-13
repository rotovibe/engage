IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ContractProperty] WHERE [Key] = 'ENABLE_SEARCH_ALL_GROUPS')
BEGIN
	UPDATE [ContractProperty]
	SET [Value] = 1
	WHERE [Key] = 'ENABLE_SEARCH_ALL_GROUPS'
END
ELSE
BEGIN
	INSERT [ContractProperty] ([Key],[Value])
	VALUES ('ENABLE_SEARCH_ALL_GROUPS', '1')
END