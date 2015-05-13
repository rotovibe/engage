

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContractProperty]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ContractProperty](
		[Key] [varchar](50) NOT NULL,
		[Value] [varchar](max) NOT NULL,
	 CONSTRAINT [PK_ContractProperty] PRIMARY KEY CLUSTERED 
	(
		[Key] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END