SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_CommTypeCommMode](
	[CommTypeCommModeId] [int] IDENTITY(1,1) NOT NULL,
	[CommModeId] [int] NOT NULL,
	[MongoCommModeLookUpId] [varchar](50) NULL,
	[CommTypeId] [int] NOT NULL,
	[MongoCommTypeLookUpId] [varchar](50) NULL,
 CONSTRAINT [PK_CommTypeCommMode] PRIMARY KEY CLUSTERED 
(
	[CommTypeCommModeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_CommTypeCommMode]  WITH CHECK ADD  CONSTRAINT [FK_CommTypeCommMode_CommModeLookUp] FOREIGN KEY([CommModeId])
REFERENCES [RPT_CommModeLookUp] ([CommModeId])
GO
ALTER TABLE [RPT_CommTypeCommMode] CHECK CONSTRAINT [FK_CommTypeCommMode_CommModeLookUp]
GO
ALTER TABLE [RPT_CommTypeCommMode]  WITH CHECK ADD  CONSTRAINT [FK_CommTypeCommMode_CommTypeLookUp] FOREIGN KEY([CommTypeId])
REFERENCES [RPT_CommTypeLookUp] ([CommTypeId])
GO
ALTER TABLE [RPT_CommTypeCommMode] CHECK CONSTRAINT [FK_CommTypeCommMode_CommTypeLookUp]
GO
