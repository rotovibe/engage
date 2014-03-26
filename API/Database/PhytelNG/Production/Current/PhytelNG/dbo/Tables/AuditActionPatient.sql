CREATE TABLE [dbo].[AuditActionPatient] (
    [AuditActionId] INT          NOT NULL,
    [PatientId]     VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AuditViewPatients] PRIMARY KEY CLUSTERED ([AuditActionId] ASC, [PatientId] ASC) WITH (FILLFACTOR = 80)
);

