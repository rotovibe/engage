using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Atmosphere.Core.Data.DTO
{
	/// <summary>
	/// The schema for this class matches the schema for the PatientDetails collection in the Mongo database.
	/// Any changes to that schema should be reflected here.
	/// </summary>
	[Serializable]
	[XmlRoot(ElementName = "patient")]
	public class PatientSearchResult
	{
		/// <summary>
		/// This is the Mongo ID for the patient, should NOT be used
		/// as it could change at any point
		/// </summary>
		//public int Id { get; set; }

		[XmlElement("p")]
		public int PatientId { get; set; }

		[XmlElement("f")]
		public string FirstName { get; set; }

		[XmlElement("m")]
		public string MiddleInitial { get; set; }

		[XmlElement("l")]
		public string LastName { get; set; }

		[XmlElement("fn")]
		public string FullName { get; set; }

		[XmlElement("b")]
		public string BirthDate { get; set; }

		[XmlElement("g")]
		public string Gender { get; set; }

		[XmlElement("n")]
		public string PhoneNumber { get; set; }

		[XmlElement("e")]
		public string EmailAddress { get; set; }

		[XmlElement("i")]
		public string PMSID { get; set; }

		[XmlElement("s")]
		public string LastCallStatus { get; set; }

		[XmlElement("d")]
		public string LastCallDate { get; set; }

		[XmlElement("t")]
		public string LastCallTime { get; set; }

		[XmlElement("c")]
		public bool Communications { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is PatientSearchResult))
            {
                return false;
            }
            var searchResult = (PatientSearchResult)obj;
            return this.PatientId == searchResult.PatientId;
        }

        public override int GetHashCode()
        {
            return this.PatientId;
        }
	}
}
