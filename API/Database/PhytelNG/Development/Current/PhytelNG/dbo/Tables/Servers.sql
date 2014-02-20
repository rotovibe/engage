CREATE TABLE [dbo].[Servers] (
    [ServerID]           INT          IDENTITY (1, 1) NOT NULL,
    [Description]        VARCHAR (50) NOT NULL,
    [IpAddress]          VARCHAR (50) NOT NULL,
    [IpName]             VARCHAR (50) NOT NULL,
    [DeleteFlag]         TINYINT      NOT NULL,
    [ServerType]         CHAR (10)    NOT NULL,
    [ReplicationSetName] VARCHAR (50) NULL,
    CONSTRAINT [PK_SERVERS] PRIMARY KEY CLUSTERED ([ServerID] ASC) WITH (FILLFACTOR = 80)
);

