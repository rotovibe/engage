CREATE TABLE [dbo].[UserContract] (
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [ContractId]      INT              NOT NULL,
    [DefaultContract] BIT              CONSTRAINT [DF__UserContr__Defau__45F365D3] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserContract] PRIMARY KEY CLUSTERED ([UserId] ASC, [ContractId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_UserContract_Contract] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([ContractId]),
    CONSTRAINT [FK_UserContract_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

