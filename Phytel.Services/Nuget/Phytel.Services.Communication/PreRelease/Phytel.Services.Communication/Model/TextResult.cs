using System;

namespace Phytel.Services.Communication
{
    public class TextResult
    {
        public string ReferenceID { get; set; }
        public string ResultCode { get; set; }
        public string ResultStatus { get; set; }
        public string ResultDescription { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string FromNumber { get; set; }
        public string ToPhoneNumber { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string ResultType { get; set; }

        /* Vendor Fields */
        public string VendorTransactionID { get; set; }
        public string VendorStatus { get; set; }
        public string VendorErrorCode { get; set; }
        public string VendorErrorDescription { get; set; }

        /* These should be depreciated once we're using the Bus */
        public string ApplicationURL { get; set; }
    }
}
