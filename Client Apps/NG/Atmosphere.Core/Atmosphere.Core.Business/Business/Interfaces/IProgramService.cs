using System.Collections.Generic;
using C3.Data;

namespace C3.Business.Interfaces
{
    public interface IProgramService
    {
        List<Program> GetReportPrograms(int contractId, int? measureTypeId, string providerIds);
    }
}
