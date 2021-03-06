SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ToDo](
	[ToDoId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoSourceId] [varchar](50) NOT NULL,
	[MongoPatientId] [varchar](50) NULL,
	[PatientId] [int] NULL,
	[MongoAssignedToId] [varchar](50) NULL,
	[AssignedToId] [int] NULL,
	[Description] [varchar](500) NULL,
	[Title] [varchar](500) NULL,
	[DueDate] [datetime] NULL,
	[ClosedDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[MongoCategory] [varchar](50) NULL,
	[ToDoCategoryId] [int] NULL,
	[Priority] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[DeleteFlag] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_ToDo] PRIMARY KEY CLUSTERED 
(
	[ToDoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ToDo]  WITH CHECK ADD  CONSTRAINT [FK_ToDo_ToDoCategoryLookUp] FOREIGN KEY([ToDoCategoryId])
REFERENCES [RPT_ToDoCategoryLookUp] ([ToDoCategoryId])
GO
ALTER TABLE [RPT_ToDo] CHECK CONSTRAINT [FK_ToDo_ToDoCategoryLookUp]
GO
