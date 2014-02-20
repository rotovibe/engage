CREATE TABLE [dbo].[UserContractProperty] (
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [ContractId] INT              NOT NULL,
    [Key]        VARCHAR (50)     NOT NULL,
    [Value]      VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_User_Contract_Key] UNIQUE CLUSTERED ([UserId] ASC, [ContractId] ASC, [Key] ASC) WITH (FILLFACTOR = 90)
);

