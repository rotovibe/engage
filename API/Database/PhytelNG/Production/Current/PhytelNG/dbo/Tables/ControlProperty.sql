CREATE TABLE [dbo].[ControlProperty] (
    [ControlId]       INT          NOT NULL,
    [IsTab]           BIT          NOT NULL,
    [DisplayOrder]    INT          CONSTRAINT [DF_ControlProperty_DisplayOrder] DEFAULT ((0)) NOT NULL,
    [IsSubTab]        BIT          CONSTRAINT [DF_ControlProperty_IsSubTab] DEFAULT ((0)) NOT NULL,
    [ParentControlId] INT          NULL,
    [IsVisible]       BIT          CONSTRAINT [DF_ControlProperty_IsVisible] DEFAULT ((1)) NOT NULL,
    [TabType]         VARCHAR (50) NULL,
    CONSTRAINT [PK_ControlPropery] PRIMARY KEY CLUSTERED ([ControlId] ASC),
    CONSTRAINT [FK_ControlProperty_Control] FOREIGN KEY ([ControlId]) REFERENCES [dbo].[Control] ([ControlId]),
    CONSTRAINT [FK_ControlProperty_Control1] FOREIGN KEY ([ParentControlId]) REFERENCES [dbo].[Control] ([ControlId])
);


GO
CREATE NONCLUSTERED INDEX [IDX_ControlProperty_ParentControlID]
    ON [dbo].[ControlProperty]([ParentControlId] ASC);

