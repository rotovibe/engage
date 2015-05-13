/****** Object:  Table [dbo].[UserContractProperty]    Script Date: 03/18/2013 10:20:41 ******/
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserContractProperty]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserContractProperty](
		[UserId] [uniqueidentifier] NOT NULL,
		[ContractId] [int] NOT NULL,
		[Key] [varchar](50) NOT NULL,
		[Value] [varchar](max) NULL,
	 CONSTRAINT [PK_User_Contract_Key] UNIQUE CLUSTERED 
	(
		[UserId] ASC,
		[ContractId] ASC,
		[Key] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO
