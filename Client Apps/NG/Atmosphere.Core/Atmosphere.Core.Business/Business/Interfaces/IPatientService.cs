using System;
using System.Data;
using System.Collections.Generic;

using C3.Data;


namespace C3.Business.Interfaces
{
    public interface IPatientService
    {
        IDictionary<string, DataTable> SearchPatients(Contract contract, string firstName, string lastName, string phone, AuditData auditLog);

        Patient GetPatient(Contract contract, int patientId, AuditData auditLog);
    }
}
