using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Atmosphere.Core.Data.DTO
{
	[Serializable]
	[XmlRoot(ElementName = "pl")]
	public class PatientSearchResultList
	{
		public enum SearchTypeEnum
		{
			NameSearchType,
			PhoneSearchType,
			EmailSearchType,
			AttributeSearchType,
			BirthdaySearchType,
			InvalidSearchType,
			MongoFailure
		}

		[XmlElement("p")]
		public List<PatientSearchResult> PatientSearchList { get; set; }

		[XmlElement("x")]
		public bool ResultsExceededMax { get; set; }

		[XmlElement("st")]
		public SearchTypeEnum SearchType { get; set; }

		[XmlElement("m")]
		public string Message { get; set; }
	}
}
