using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Atmosphere.Search.Data.DTO;
using Atmosphere.Core.Data.DTO;

namespace Atmosphere.Core.Domain.Repositories.Abstract
{
	public interface IPatientSearchRepository
	{
		PatientSearchResultList SearchPatients(Guid userId, string contractId, string searchTerm);
	}
}
