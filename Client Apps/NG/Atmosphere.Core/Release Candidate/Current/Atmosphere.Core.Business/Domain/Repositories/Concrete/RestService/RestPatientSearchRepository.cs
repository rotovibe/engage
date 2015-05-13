using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Domain.Repositories.Concrete.RestService;
using Atmosphere.Core.Domain.Repositories.Abstract;
using Microsoft.Http;
using Atmosphere.Core.Data.Services;
//using Atmosphere.Search.Data.DTO;
//using Atmosphere.Core.Data.Services;
using Atmosphere.Core.Data.DTO;

namespace Atmosphere.Core.Domain.Repositories.Concrete.RestService
{
	public class RestPatientSearchRepository : BaseRepository, IPatientSearchRepository
	{
		public PatientSearchResultList SearchPatients(Guid userId, string contractId, string searchTerm)
		{
			HttpQueryString queryString = GetDefaultQueryString();
			//Use the right URL here (get from sample)
			//http://localhost:3333/patientSearch?apikey=bda11d91-7ade-4da1-855d-24adfe39d174&userID=B8013B40-7812-408C-999B-F0E7F07DD871&contractId=JCMR001&searchTerm=kimber
			
			queryString.Add("userId", userId.ToString());
			//queryString.Add("userId", "B8013B40-7812-408C-999B-F0E7F07DD871");

			queryString.Add("contractId", contractId);
			//queryString.Add("contractId", @"JCMR001");
			
			queryString.Add("searchTerm", searchTerm);

		//var uri = GetServiceRequestUri(ServiceUriFormats.DefaultPageViewList, ServiceUriFormats.Application, new object[] { contractId, controlId });

			Uri uri = GetServiceRequestUri(ServiceUriFormats.PatientSearch, ServiceUriFormats.Application);


			PatientSearchResultList listing = RequestRESTData<PatientSearchResultList>(queryString, uri);
			return listing;
		}
	}
}
