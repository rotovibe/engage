IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ContractProperty])
BEGIN
	UPDATE [ContractProperty]
	SET [Value] = 0
	WHERE [Key] = 'ENABLE_SEARCH_ALL_GROUPS'
END