CREATE TABLE [dbo].[RoleType] (
    [RoleTypeId]   TINYINT      NOT NULL,
    [RoleTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_RoleType] PRIMARY KEY CLUSTERED ([RoleTypeId] ASC) WITH (FILLFACTOR = 90)
);

