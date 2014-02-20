CREATE TABLE [dbo].[TermsOfService] (
    [TermsOfServiceID] INT           IDENTITY (1, 1) NOT NULL,
    [Version]          INT           NOT NULL,
    [Text]             VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_TermsOfService] PRIMARY KEY CLUSTERED ([TermsOfServiceID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_TermsOfService_Version]
    ON [dbo].[TermsOfService]([Version] ASC);

