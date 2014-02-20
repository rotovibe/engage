CREATE TABLE [dbo].[Profile] (
    [UserId]               UNIQUEIDENTIFIER NOT NULL,
    [PropertyNames]        NTEXT            NOT NULL,
    [PropertyValuesString] NTEXT            NOT NULL,
    [PropertyValuesBinary] IMAGE            NOT NULL,
    [LastUpdatedDate]      DATETIME         NOT NULL,
    CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED ([UserId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Profile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

