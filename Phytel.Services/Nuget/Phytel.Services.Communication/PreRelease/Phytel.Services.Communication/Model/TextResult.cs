using System;
using System.Xml.Serialization;

namespace Phytel.Services.Communication
{
    [XmlRoot("TextResult")]
    [Serializable]
    public class TextResult
    {
        [XmlElement(ElementName = "ReferenceID")]
        public string ReferenceID { get; set; }
        [XmlElement(ElementName = "ResultCode")]
        public string ResultCode { get; set; }
        [XmlElement(ElementName = "ResultStatus")]
        public string ResultStatus { get; set; }
        [XmlElement(ElementName = "ResultDescription")]
        public string ResultDescription { get; set; }
        [XmlElement(ElementName = "ErrorCode")]
        public string ErrorCode { get; set; }
        [XmlElement(ElementName = "ErrorDescription")]
        public string ErrorDescription { get; set; }
        [XmlElement(ElementName = "FromNumber")]
        public string FromNumber { get; set; }
        [XmlElement(ElementName = "ToPhoneNumber")]
        public string ToPhoneNumber { get; set; }
        [XmlElement(ElementName = "ProcessedDate")]
        public DateTime? ProcessedDate { get; set; }
        [XmlElement(ElementName = "ResultType")]
        public string ResultType { get; set; }

        /* Vendor Fields */
        [XmlElement(ElementName = "VendorTransactionID")]
        public string VendorTransactionID { get; set; }
        [XmlElement(ElementName = "VendorStatus")]
        public string VendorStatus { get; set; }
        [XmlElement(ElementName = "VendorErrorCode")]
        public string VendorErrorCode { get; set; }
        [XmlElement(ElementName = "VendorErrorDescription")]
        public string VendorErrorDescription { get; set; }

        /* These should be depreciated once we're using the Bus */
        [XmlElement(ElementName = "ApplicationURL")]
        public string ApplicationURL { get; set; }
    }
}
